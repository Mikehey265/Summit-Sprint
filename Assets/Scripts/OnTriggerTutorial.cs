using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerTutorial : MonoBehaviour
{
    public GameObject otherCoach;
    public GameObject bodyPhysics;
    private bool hasBeenTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered && other.gameObject.CompareTag("Player"))
        {
            otherCoach.SetActive(true);

            BodyPhysics bodyPhysicsScript = bodyPhysics.GetComponent<BodyPhysics>();
            if (bodyPhysicsScript != null)
            {
                // Enable the input by setting these booleans to true
                bodyPhysicsScript.InputIsEnabled = false;
                bodyPhysicsScript.InputIsEnabledLeftHand = false;
                bodyPhysicsScript.InputIsEnabledRightHand = false;
                bodyPhysicsScript.InputIsEnabledSlingShot = false;
                Debug.Log("Inputs disabled");
            }

            hasBeenTriggered = true; // Set the flag to true to prevent further triggers
        }
    }
}
