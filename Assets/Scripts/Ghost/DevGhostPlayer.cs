using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public class DevGhostPlayer : MonoBehaviour
{
    public DevGhost devGhost;
    public Transform leftArm;
    public Transform rightArm;
    public Transform leftFoot;
    public Transform rightFoot;

    public Rig rig;

    private float timeValue;
    private int index1;
    private int index2;
    

    private void Awake()
    {
        timeValue = 0; 
        index2 = 0;
        index1 = 0;
    }

    private void Start()
    {
       devGhost.isReplaying = false;
    }

    void Update()
    {

        if(!devGhost.isUnlocked) return;
        if (!devGhost.shouldPlayDevGhost) return;
        if (!GameManager.Instance.isGameRunning) return;
        devGhost.isReplaying = true;
        if (devGhost.isReplaying)
        {
            timeValue += Time.deltaTime;
            GetIndex();
            SetTransform();
        }
    }
    

    private void GetIndex()
    {
        var min = 0;
        var max = devGhost.timeStamp.Count - 1;
        while (min <= max)
        {
            var mid = (min + max) / 2;
            if (devGhost.timeStamp[mid] < timeValue)
            {
                min = mid + 1;
            }
            else if (devGhost.timeStamp[mid] > timeValue)
            {
                max = mid - 1;
            }
            else
            {
                index1 = index2 = mid;
                return;
            }
        }
        if (max < 0) max = 0;
        index1 = max;
        index2 = Mathf.Min(max + 1, devGhost.timeStamp.Count - 1);
    }

    private void SetTransform()
    {
        if (devGhost.position.Count == 0 || devGhost.rotation.Count == 0) return;
        
        float mainInterpolationFactor = 0;
        if (index1 != index2)
        {
            mainInterpolationFactor = (timeValue - devGhost.timeStamp[index1]) / (devGhost.timeStamp[index2] - devGhost.timeStamp[index1]);
        }
        
        this.transform.position = Vector3.Lerp(devGhost.position[index1], devGhost.position[Mathf.Min(index2, devGhost.position.Count - 1)], mainInterpolationFactor);
        this.transform.rotation = Quaternion.Slerp(devGhost.rotation[index1], devGhost.rotation[Mathf.Min(index2, devGhost.rotation.Count - 1)], mainInterpolationFactor);

        
        UpdateBodyPartTransform(leftArm, devGhost.leftArmPosition, devGhost.leftArmRotation, mainInterpolationFactor);
        
        UpdateBodyPartTransform(rightArm, devGhost.rightArmPosition, devGhost.rightArmRotation, mainInterpolationFactor);
        
        UpdateBodyPartTransform(leftFoot, devGhost.leftFootPosition, devGhost.leftFootRotation, mainInterpolationFactor);
        
        UpdateBodyPartTransform(rightFoot, devGhost.rightFootPosition, devGhost.rightFootRotation, mainInterpolationFactor);

        if (devGhost.rigWeight.Count > index1 && devGhost.rigWeight.Count > index2)
        {
            var interpolatedRigWeight = Mathf.Lerp(devGhost.rigWeight[index1], devGhost.rigWeight[Mathf.Min(index2, devGhost.rigWeight.Count - 1)], mainInterpolationFactor );
            rig.weight = interpolatedRigWeight;
        }
    }

    private void UpdateBodyPartTransform(Transform bodyPartTransform, List<Vector3> positionList, List<Quaternion> rotationList, float interpolationFactor)
    {
        if (positionList.Count <= index1 || rotationList.Count <= index1) return;
        Vector3 interpolatedPosition = Vector3.Lerp(positionList[index1], positionList[Mathf.Min(index2, positionList.Count - 1)], interpolationFactor);
        Quaternion interpolatedRotation = Quaternion.Slerp(rotationList[index1], rotationList[Mathf.Min(index2, rotationList.Count - 1)], interpolationFactor);
        bodyPartTransform.position = interpolatedPosition;
        bodyPartTransform.rotation = interpolatedRotation;
    }
    



    public void StartReplay() {
        if (GameManager.Instance.isGameRunning) { 
            timeValue = 0f;
            index1 = 0;
            index2 = 1;
            ReplayRecording();
        }
    }


    private void ReplayRecording()
    {
        timeValue = 0;
        index1 = 0;
        index2 = 0;
        devGhost.isReplaying = true;
    }
}