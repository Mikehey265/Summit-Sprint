using UnityEngine;
using UnityEngine.Serialization;

public class DevGhostRecorder : MonoBehaviour
{
    public DevGhost devGhost;
    public Transform leftArm;
    public Transform rightArm;
    public Transform leftFoot;
    public Transform rightFoot;
    public float rigWeight;
    public FeetControl feetControl;
    
    private float timer;
    private float timeValue;
    

    void Update()
    { 
        rigWeight = feetControl.rigWeight;
       RecordGhost();

        if (devGhost.clearData)
        {
            devGhost.ResetData();
        }

        if (GameManager.Instance.isGameWon)
            devGhost.isRecording = false;
    }


    private void RecordGhost()
    {
        if (!GameManager.Instance.isGameRunning || !devGhost.isRecording) return;
        timer += Time.deltaTime;
        timeValue += Time.deltaTime;

        if (!(timer >= 1 / devGhost.recordFrequency)) return;
        devGhost.timeStamp.Add(timeValue);
        devGhost.position.Add(transform.position);
        devGhost.rotation.Add(transform.rotation);
        devGhost.rightArmPosition.Add(rightArm.position);
        devGhost.rightArmRotation.Add(rightArm.rotation);
        devGhost.leftArmPosition.Add(leftArm.position);
        devGhost.leftArmRotation.Add(leftArm.rotation);
        devGhost.rightFootPosition.Add(rightFoot.position);
        devGhost.rightFootRotation.Add(rightFoot.rotation);
        devGhost.leftFootPosition.Add(leftFoot.position);
        devGhost.leftFootRotation.Add(leftFoot.rotation);
        devGhost.rigWeight.Add(feetControl.rigWeight);

        timer = 0;
    }
    private void ResetDataOnNewGame()
    {
        if ((!GameManager.Instance.isGameRunning || devGhost.isRecording) && !GameManager.Instance.isGameLost) return;
        devGhost.ResetData();
        timeValue = 0;
        timer = 0;
        devGhost.isRecording = true;
    }
}
