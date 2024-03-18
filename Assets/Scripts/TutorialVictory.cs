using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVictory : MonoBehaviour
{
    public GameObject VictoryPanel;
    public GameObject bodyPhysics; // GameObject that contains the BodyPhysics script

    private bool hasBeenTriggered2 = false;
    void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered2 && other.gameObject.CompareTag("Player"))
        {
            VictoryPanel.SetActive(true);
            BodyPhysics bodyPhysicsScript = bodyPhysics.GetComponent<BodyPhysics>();
            if (bodyPhysicsScript != null)
                {
                    // Enable the input by setting these booleans to true
                    bodyPhysicsScript.InputIsEnabled = false;
                    bodyPhysicsScript.InputIsEnabledLeftHand = false;
                    bodyPhysicsScript.InputIsEnabledRightHand = false;
                    bodyPhysicsScript.InputIsEnabledSlingShot = false;
                }
        }
    }
}
