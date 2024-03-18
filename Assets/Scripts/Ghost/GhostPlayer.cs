using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GhostPlayer : MonoBehaviour
{
    public Ghost ghost;
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
       ghost.isReplaying = false;
    }

    void Update()
    {


        if (!ghost.shouldPlayGhost) return;
        if (!GameManager.Instance.isGameRunning) return;
        ghost.isReplaying = true;
        if (ghost.isReplaying)
        {
            timeValue += Time.deltaTime;
            GetIndex();
            SetTransform();
        }
    }
    

    private void GetIndex()
    {
        var min = 0;
        var max = ghost.lastTimeStamp.Count - 1;
        while (min <= max)
        {
            var mid = (min + max) / 2;
            if (ghost.lastTimeStamp[mid] < timeValue)
            {
                min = mid + 1;
            }
            else if (ghost.lastTimeStamp[mid] > timeValue)
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
        index2 = Mathf.Min(max + 1, ghost.lastTimeStamp.Count - 1);
    }

    private void SetTransform()
    {
        if (ghost.lastPosition.Count == 0 || ghost.lastRotation.Count == 0) return;
        
        float mainInterpolationFactor = 0;
        if (index1 != index2)
        {
            mainInterpolationFactor = (timeValue - ghost.lastTimeStamp[index1]) / (ghost.lastTimeStamp[index2] - ghost.lastTimeStamp[index1]);
        }
        
        this.transform.position = Vector3.Lerp(ghost.lastPosition[index1], ghost.lastPosition[Mathf.Min(index2, ghost.lastPosition.Count - 1)], mainInterpolationFactor);
        this.transform.rotation = Quaternion.Slerp(ghost.lastRotation[index1], ghost.lastRotation[Mathf.Min(index2, ghost.lastRotation.Count - 1)], mainInterpolationFactor);

        
        UpdateBodyPartTransform(leftArm, ghost.leftArmLastPosition, ghost.leftArmLastRotation, mainInterpolationFactor);
        
        UpdateBodyPartTransform(rightArm, ghost.rightArmLastPosition, ghost.rightArmLastRotation, mainInterpolationFactor);
        
        UpdateBodyPartTransform(leftFoot, ghost.leftFootLastPosition, ghost.leftFootLastRotation, mainInterpolationFactor);
        
        UpdateBodyPartTransform(rightFoot, ghost.rightFootLastPosition, ghost.rightFootLastRotation, mainInterpolationFactor);

        if (ghost.lastRigWeight.Count > index1 && ghost.lastRigWeight.Count > index2)
        {
            var interpolatedRigWeight = Mathf.Lerp(ghost.lastRigWeight[index1], ghost.lastRigWeight[Mathf.Min(index2, ghost.lastRigWeight.Count - 1)], mainInterpolationFactor );
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
        ghost.isReplaying = true;
    }
}