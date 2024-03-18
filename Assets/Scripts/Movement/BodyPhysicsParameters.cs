using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BodyPhysicsParameters", menuName = "ScriptableObjects/BodyPhysicsParameters")]
public class BodyPhysicsParameters : ScriptableObject
{
    public float WorldCylinderRadius = 1f;
    public float MaxArmLength = 0.9f;

    public float GrabArmLengthMultiplier = 0.5f;
    public float GrabArmLengthMultiplierButItsTheOnlyHandGrabbing = 0.25f;

    // Controls how fast the arm length goes towards the target arm length. Must be between 0 and 1
    public float ArmLengthSmoothInterpolationSpeed = 0.1f;

    // Controls how fast the arm goes torwards the controlled target position. Must be between 0 and 1
    public float ArmSmoothInterpolationSpeed = 0.3f;

    public float SwingForceIntensityX = 50f;
    public float SwingForceIntensityY = 50f;

    public float ArmConstraintStiffness = 0.3f;

    public float ReachingForceIntensity = 13f;
    public float ReachingForceSmoothInterpSpeed = 0.5f;
    public float ReachingRadiusScale = 2f;

    public float ArmCrossShrinkFactor = 1f;

    public float HandMovementInputSpeed = 10f;

    // add extra radius around each hand when reaching for a hold
    public float GrabbableExtraReachDistance = 0.5f;

    public float JumpForce = 10f;
    public float JumpBackwardForce = 10f;

    public float OnFlyingCylinderGravity = 3f;

    public float SlingshotInputRadius = 0.7f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
