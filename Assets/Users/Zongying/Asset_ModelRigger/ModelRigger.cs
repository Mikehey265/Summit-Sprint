using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.XR;

namespace ModelRigger
{
#if UNITY_EDITOR
    public class ModelRigger : EditorWindow
    {
        private GameObject modelToRig;
        private GameObject prefabToAlter;

        [MenuItem("Window/Model Rigger")]
        public static void ShowWindow()
        {
            GetWindow<ModelRigger>("Model Rigger");
        }

        void OnGUI()
        {
            GUILayout.Label("Drag and drop the model you want to Animation Rig:", EditorStyles.boldLabel);
            modelToRig = EditorGUILayout.ObjectField(modelToRig, typeof(GameObject), true) as GameObject;

            GUILayout.Label("Drag and drop the prefab you want to modify:", EditorStyles.boldLabel);
            prefabToAlter = EditorGUILayout.ObjectField(prefabToAlter, typeof(GameObject), true) as GameObject;

            if (GUILayout.Button("Animation Rig Model") && modelToRig != null && prefabToAlter != null)
            {
                RigModel();
            }
        }

        void RigModel()
        {
            // PHASE 1

            // Duplicate the model and place it at the same position
            GameObject riggedModel = Instantiate(modelToRig, modelToRig.transform.position,
                modelToRig.transform.rotation);

            // Setup Rigs for hands and feet and head
            RigBuilder rigBuilder = riggedModel.AddComponent<RigBuilder>();

            Rig handRig = CreatAndAddRig("HandRig", rigBuilder);
            TwoBoneIKConstraint leftHandBone = CreateHandMover(riggedModel, handRig, 0);
            TwoBoneIKConstraint rightHandBone = CreateHandMover(riggedModel, handRig, 1);

            Rig footRig = CreatAndAddRig("FootRig", rigBuilder);
            TwoBoneIKConstraint leftFootBone = CreateFootMover(riggedModel, footRig, 0);
            TwoBoneIKConstraint rightFootBone = CreateFootMover(riggedModel, footRig, 1);

            Rig headRig = CreatAndAddRig("HeadRig", rigBuilder);
            MultiAimConstraint headAim = CreatHeadRotator(riggedModel, headRig);

            // Optionally, you can reset the name of the duplicated model
            riggedModel.name = "AnimationRigged_" + modelToRig.name;


            // PHASE 2

            // put the rigged model in to an old prefab
            GameObject characterPrefab = PrefabUtility.InstantiatePrefab(prefabToAlter) as GameObject;
            GameObject oldRiggedModel = characterPrefab.GetComponentInChildren<RigBuilder>().gameObject;

            // move rigged model into old prefab
            riggedModel.transform.SetParent(characterPrefab.transform);
            riggedModel.transform.localPosition = oldRiggedModel.transform.localPosition;

            // reset hand bone target
            leftHandBone.data.target = characterPrefab.GetComponent<BodyPhysics>().leftHandtargetTransform;
            rightHandBone.data.target = characterPrefab.GetComponent<BodyPhysics>().rightHandtargetTransform;


            // copy and use old correct foot rig in old model
            GameObject oldFootRigCopy = Instantiate(FindDeepChild(oldRiggedModel.transform, "FootRig").gameObject);
            oldFootRigCopy.name = "FootRig";
            oldFootRigCopy.transform.SetParent(riggedModel.transform);
            oldFootRigCopy.transform.localPosition = Vector3.zero;

            // target is correct in the copy,
            // so only need to reset the joints for IK.
            ResetFootRig(oldFootRigCopy.transform, riggedModel.transform);
            rigBuilder.layers[1] = new RigLayer(oldFootRigCopy.GetComponent<Rig>());
            

            // reset feet controller
            FeetControl feetControl = characterPrefab.GetComponentInChildren<FeetControl>();
            feetControl.hips = FindDeepChild(riggedModel.transform, "Pelvis");
            feetControl.rigs[0] = oldFootRigCopy.GetComponent<Rig>();

            // hand target setting
            HandTarget handTarget = characterPrefab.GetComponentInChildren<HandTarget>();
            handTarget.bodyPhysics = characterPrefab.GetComponentInChildren<BodyPhysics>();

            // destroy the useless foot rig
            DestroyImmediate(footRig.gameObject);

            // set head aim
            Transform headTarget = characterPrefab.transform.Find("HeadTarget");
            Debug.Log(headTarget.name);

            var data = headAim.data.sourceObjects;
            // data.SetTransform(0,headTarget);
            // data.SetWeight(0,1);
            data.Add(new WeightedTransform(headTarget, 1));
            headAim.data.sourceObjects = data;

            // WeightedTransform weightedHeadAim = new WeightedTransform(headTarget, 1);
            // headAim.data.sourceObjects.Add(weightedHeadAim);
            Debug.Log(headAim.data.sourceObjects[0].transform.name);

            // set up animation rig controller
            AnimationRigController arc = characterPrefab.GetComponentInChildren<AnimationRigController>();
            if (arc)
            {
                arc.animationRiggedModel = riggedModel.transform;
                arc.handRig = handRig;
                arc.leftHandHint = FindDeepChild(leftHandBone.transform,"LeftHandHint");
                arc.rightHandHint = FindDeepChild(rightHandBone.transform,"RightHandHint");

                arc.leftFootPos = feetControl.leftFootTarget;
                arc.rightFootPos = feetControl.rightFootTarget;
                arc.leftFootHint = FindDeepChild(oldFootRigCopy.transform, "LeftFootHint");
                arc.rightFootHint = FindDeepChild(oldFootRigCopy.transform, "RightFootHint");
            }

            // destroy the useless rigged model
            DestroyImmediate(oldRiggedModel);

            characterPrefab.name += "_Altered";
        }

        Rig CreatAndAddRig(string rigName, RigBuilder rigBuilder)
        {
            GameObject rigObj = new GameObject(rigName);
            Rig rig = rigObj.AddComponent<Rig>();
            rigBuilder.layers.Add(new RigLayer(rig, true));
            rigObj.transform.SetParent(rigBuilder.transform);
            rigObj.transform.localPosition = Vector3.zero;
            return rig;
        }

        TwoBoneIKConstraint CreateHandMover(GameObject riggedModel, Rig handRig, int leftOrRight)
        {
            string direction;
            string dir;
            Vector3 elbowOffset;
            if (leftOrRight == 0)
            {
                direction = "Left";
                dir = "L";
                elbowOffset = new Vector3(-3, -1, 0);
            }
            else if (leftOrRight == 1)
            {
                direction = "Right";
                dir = "R";
                elbowOffset = new Vector3(3, -1, 0);
            }
            else return null;

            // Set up right hand
            GameObject handMover = new GameObject(direction + "HandMover");
            handMover.transform.SetParent(handRig.transform);
            handMover.transform.localPosition = Vector3.zero;
            TwoBoneIKConstraint handBone = handMover.AddComponent<TwoBoneIKConstraint>();

            // set up joints
            Transform armTransform = FindDeepChild(riggedModel.transform, "Arm." + dir);
            Transform elbowTransform = armTransform.transform.Find("Elbow." + dir);
            Transform handTransform = elbowTransform.transform.Find("Hand." + dir);

            handBone.data.root = armTransform;
            handBone.data.mid = elbowTransform;
            handBone.data.tip = handTransform;

            //set up  target and hint
            GameObject handTarget = new GameObject(direction + "HandTarget");
            handTarget.transform.SetParent(handMover.transform);
            handTarget.transform.position = handTransform.position;
            handTarget.transform.rotation = handTransform.rotation;
            handBone.data.target = handTarget.transform;

            GameObject handHint = new GameObject(direction + "HandHint");
            handHint.transform.SetParent(handMover.transform);
            handHint.transform.localPosition = elbowOffset;
            handBone.data.hint = handHint.transform;

            // try to show effector but failed
            // RigEffectorData effectorData = new RigEffectorData();
            // RigEffectorData.Style targetStyle = new RigEffectorData.Style();
            // targetStyle.color = Color.red;
            // targetStyle.shape = AssetDatabase.LoadAssetAtPath<Mesh>("Packages/com.unity.animation.rigging/Editor/Shapes/BoxEffector.asset");
            // effectorData.Initialize(rightHandTarget.transform, targetStyle);

            return handBone;
        }

        TwoBoneIKConstraint CreateFootMover(GameObject riggedModel, Rig footRig, int leftOrRight)
        {
            string direction;
            string dir;
            Vector3 elbowOffset;

            if (leftOrRight == 0)
            {
                direction = "Left";
                dir = "L";
                elbowOffset = new Vector3(-100, 0, -100);
            }
            else if (leftOrRight == 1)
            {
                direction = "Right";
                dir = "R";
                elbowOffset = new Vector3(100, 0, -100);
            }
            else return null;

            // Set up right hand
            GameObject footMover = new GameObject(direction + "FootMover");
            footMover.transform.SetParent(footRig.transform);
            TwoBoneIKConstraint footBone = footMover.AddComponent<TwoBoneIKConstraint>();

            // set up joints
            Transform hipsTransform = FindDeepChild(riggedModel.transform, "Hips." + dir);
            Transform kneeTransform = hipsTransform.transform.Find("Knee." + dir);
            Transform footTransform = kneeTransform.transform.Find("Foot." + dir);

            footBone.data.root = hipsTransform;
            footBone.data.mid = kneeTransform;
            footBone.data.tip = footTransform;

            //set up  target and hint
            GameObject footTarget = new GameObject(direction + "FootPos");
            footTarget.transform.SetParent(footMover.transform);
            footTarget.transform.position = footTransform.position;
            footTarget.transform.rotation = footTransform.rotation;
            footBone.data.target = footTarget.transform;

            GameObject footHint = new GameObject(direction + "FootHint");
            footHint.transform.SetParent(footMover.transform);
            footHint.transform.position = kneeTransform.position;
            footBone.data.hint = footHint.transform;

            // try to show effector but failed
            // RigEffectorData effectorData = new RigEffectorData();
            // RigEffectorData.Style targetStyle = new RigEffectorData.Style();
            // targetStyle.color = Color.red;
            // targetStyle.shape = AssetDatabase.LoadAssetAtPath<Mesh>("Packages/com.unity.animation.rigging/Editor/Shapes/BoxEffector.asset");
            // effectorData.Initialize(rightHandTarget.transform, targetStyle);

            return footBone;
        }

        MultiAimConstraint CreatHeadRotator(GameObject riggedModel, Rig headRig)
        {
            GameObject headRotator = new GameObject("HeadRotator");
            headRotator.transform.SetParent(headRig.transform);
            MultiAimConstraint headAim = headRotator.AddComponent<MultiAimConstraint>();
            headAim.data.constrainedObject = FindDeepChild(riggedModel.transform, "Head");
            // no aim set here, set later in phase 2
            headAim.data.sourceObjects.Add(new WeightedTransform());
            return headAim;
        }

        void ResetFootRig(Transform footRig, Transform riggedModel)
        {
            string direction;
            string dir;
            TwoBoneIKConstraint[] footBones = footRig.GetComponentsInChildren<TwoBoneIKConstraint>();
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    direction = "Left";
                    dir = "L";
                }
                else if (i == 1)
                {
                    direction = "Right";
                    dir = "R";
                }
                else
                {
                    return;
                }

                TwoBoneIKConstraint footBone = footBones[i];
                Transform hipsTransform = FindDeepChild(riggedModel.transform, "Hips." + dir);
                Transform kneeTransform = hipsTransform.transform.Find("Knee." + dir);
                Transform footTransform = kneeTransform.transform.Find("Foot." + dir);

                footBone.data.target.position = footTransform.position;

                footBone.data.root = hipsTransform;
                footBone.data.mid = kneeTransform;
                footBone.data.tip = footTransform;
            }
        }

        Transform FindDeepChild(Transform parent, string name)
        {
            // Check if the current parent has a child with the specified name
            Transform child = parent.Find(name);

            if (child != null)
            {
                // Child found, return it
                return child;
            }
            else
            {
                // Child not found at this level, so search deeper in the hierarchy
                for (int i = 0; i < parent.childCount; i++)
                {
                    child = FindDeepChild(parent.GetChild(i), name);
                    if (child != null)
                    {
                        // Child found in a deeper level, return it
                        return child;
                    }
                }

                // Child not found anywhere in the hierarchy
                return null;
            }
        }
    }
#endif
}