using System;
using System.Collections;
using System.Collections.Generic;
using StaminaSystem;
using UnityEngine;

public class MountainPeak : MonoBehaviour
{
    private GameObject character;
    private GameObject leftHand;
    private GameObject rightHand;
    private Vector3 playerPosition;

    private BodyPhysics bodyPhysics;

    private bool hasPlayerReachedPeak;

    private void Awake()
    {
        bodyPhysics = FindObjectOfType<BodyPhysics>();
    }

    private void Start()
    {
        hasPlayerReachedPeak = false;
    }

    private void Update()
    {
		// this code is now handled in BodyPhysics instead
        /*DistanceToPlayer();

        if (hasPlayerReachedPeak)
        {
			GameManager.Instance.UpdateGameState(GameStateSO.State.Win);
        }*/
    }

    private void DistanceToPlayer()
    {
        float distanceLeftHand = Vector3.Distance(transform.position, bodyPhysics.leftHandtargetTransform.position);
        float distanceRightHand = Vector3.Distance(transform.position, bodyPhysics.rightHandtargetTransform.position);

		if (distanceLeftHand < 0.1 || distanceRightHand < 0.1)
        {
            hasPlayerReachedPeak = true;
        }
        else if(distanceLeftHand > 0.1 && distanceRightHand > 0.1)
        {
            hasPlayerReachedPeak = false;
        }
    }
}
