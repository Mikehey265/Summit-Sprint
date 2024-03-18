using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    public Ghost ghost;

    public Transform leftArm;
    public Transform rightArm;
    public Transform leftFoot;
    public Transform rightFoot;
    public float rigWeight;
    public FeetControl feetControl;
    
    private float timer;
    private float timeValue;
    
    void Start()
    {
       
        ghost.ResetData();
    }

    void Update()
    {
        rigWeight = feetControl.rigWeight;
       ResetDataOnNewGame();
       SaveLastDataOnWin(); 
       RecordGhost();

        if (ghost.clearData)
        {
            ghost.ResetData();
        }
        
        // if (ghost.clearLastData)
        // {
        //     ghost.ResetLastData();
        // }
    }


    private void RecordGhost()
    {
        if (!GameManager.Instance.isGameRunning || !ghost.isRecording) return;
        timer += Time.deltaTime;
        timeValue += Time.deltaTime;

        if (!(timer >= 1 / ghost.recordFrequency)) return;
        ghost.timeStamp.Add(timeValue);
        ghost.position.Add(transform.position);
        ghost.rotation.Add(transform.rotation);
        ghost.rightArmPosition.Add(rightArm.position);
        ghost.rightArmRotation.Add(rightArm.rotation);
        ghost.leftArmPosition.Add(leftArm.position);
        ghost.leftArmRotation.Add(leftArm.rotation);
        ghost.rightFootPosition.Add(rightFoot.position);
        ghost.rightFootRotation.Add(rightFoot.rotation);
        ghost.leftFootPosition.Add(leftFoot.position);
        ghost.leftFootRotation.Add(leftFoot.rotation);
        ghost.rigWeight.Add(feetControl.rigWeight);

        timer = 0;
    }

    private void SaveLastDataOnWin()
    {
        if (!GameManager.Instance.isGameWon) return;
        ghost.isRecording = false;
        if(ghost.timeStamp.Count < ghost.lastTimeStamp.Count || ghost.saveLastData)
        {
            ghost.SaveLastData();
        }
        else if(ghost.lastTimeStamp.Count == 0)
        {
            ghost.SaveLastData();
        }
    }

    private void ResetDataOnNewGame()
    {
        if ((!GameManager.Instance.isGameRunning || ghost.isRecording) && !GameManager.Instance.isGameLost) return;
        ghost.ResetData();
        timeValue = 0;
        timer = 0;
        ghost.isRecording = true;
    }
   

}
