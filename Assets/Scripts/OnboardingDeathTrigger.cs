using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingDeathTrigger : MonoBehaviour
{
    public Grabbable ResetToHold;
    public BodyPhysics BodyPhysics;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BodyPhysics.StartGrabbingHoldWithBothHands(ResetToHold);
        }
    }
}
