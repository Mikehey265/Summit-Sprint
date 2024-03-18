using System;
using System.Collections;
using System.Collections.Generic;
using StaminaSystem;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

public enum HandIndex
{
    Left = 0,
    Right = 1,
}

public class BodyPhysics : MonoBehaviour
{
    public BodyPhysicsParameters Parameters;

    public Transform rightHandtargetTransform;
    public Transform leftHandtargetTransform;
    public HandTarget handTarget;
    public VFXController vfxController;

    public Rigidbody rightHandRB;
    public Rigidbody leftHandRB;
    public Rigidbody pelvisRB;
    private PlayerStamina stamina;

    public bool isClimbing = false;
    public bool HasTouchedWallAfterJump = false;
    
    public bool ShouldAutoGrab = true;
    public bool[] IsGrabbing = new bool[2] { true, true }; // use HandIndex to index into this array
    public Grabbable[] LastGrabbedHold = new Grabbable[2] { null, null }; // use HandIndex to index into this array
    private Grabbable[] _previousGrabbedHold = new Grabbable[2] { null, null }; // Track the last grabbed hold for each hand
    
    public bool InputIsEnabled = true; // controls all input kinds at once
    public bool InputIsEnabledSlingShot = true;
    public bool InputIsEnabledLeftHand = true;
    public bool InputIsEnabledRightHand = true;

    public SlingShot slingShot;

    private float _leaningRotation = 0f;
    private float _leaningRotationTarget = 0f;

    // touch input state
    private bool[] _fingerIDIsTouching = new bool[2] { false, false };
    private bool[] _fingerIDIsSuccessfullyTouching = new bool[2] { false, false };
    private Vector3[] _touchStartHandRelativePos = new Vector3[2] { Vector3.zero, Vector3.zero };
    private Vector2[] _touchOldPosition = new Vector2[2] { Vector2.zero, Vector2.zero }; // between (0, 0) and (1, 1)
    private HandIndex[] _touchHandIndex = new HandIndex[2] { HandIndex.Left, HandIndex.Left };


    public Vector3[] _handLerpTargetPositions = null;
    private float[] _armLengths = new float[2] { 1f, 1f };
    
    // side objective variables
    public int armMoves;

    // Start is called before the first frame update
    void Start()
    {
        // Detach target transforms from the body so that they move independently from the body.
        rightHandtargetTransform.parent = null;
        leftHandtargetTransform.parent = null;
        _handLerpTargetPositions = new Vector3[2] { leftHandtargetTransform.position, rightHandtargetTransform.position };

        //handTarget.transform.parent = null;
        stamina = GetComponent<PlayerStamina>();

        armMoves = 0;
    }

    private Transform GetHandTargetTransform(HandIndex handIndex)
    {
        return handIndex == HandIndex.Left ? leftHandtargetTransform : rightHandtargetTransform;
    }

    public float GetRotationAtPoint(Vector3 point)
    {
        return Mathf.Atan2(-point.x, -point.z);
    }

    private Vector3 GetRightVectorAtPoint(Vector3 point)
    {
        Vector3 dir = (new Vector3(point.x, 0, point.z)).normalized;
        return new Vector3(-dir.z, 0, dir.x);
    }

    private void OnBeginTouch(int fingerId, Vector2 touchPositionScreenSpace)
    {
        Vector2 touchPosition = new Vector2(touchPositionScreenSpace.x / (float)Screen.width,
            touchPositionScreenSpace.y / (float)Screen.width);
        HandIndex handIndex = touchPosition.x < 0.5f ? HandIndex.Left : HandIndex.Right;

        // ignore this touch if the same side is already touched
        //if (fingerId == 1)
        //{
        //    int _ = 0;
        //}
        if (_fingerIDIsSuccessfullyTouching[1 - fingerId] && _touchHandIndex[1 - fingerId] == handIndex) return;
        
        if (handIndex == HandIndex.Left && !InputIsEnabledLeftHand) return;
        if (handIndex == HandIndex.Right && !InputIsEnabledRightHand) return;

    armMoves++;
        Debug.Log($"TUCH BEGIN: {touchPosition}");
        _fingerIDIsSuccessfullyTouching[fingerId] = true;

        slingShot.hasJumped = false;
        isClimbing = true;

        Transform handTransform =
            handIndex == HandIndex.Left ? leftHandtargetTransform : rightHandtargetTransform;

        _touchHandIndex[fingerId] = handIndex;
        _touchOldPosition[fingerId] = touchPosition;
        _touchStartHandRelativePos[fingerId] = handTransform.position - transform.position;

        handTarget.selectedHandPos = handTransform;

        IsGrabbing[(int)handIndex] = false;
    }

    public void StartGrabbingHoldWithBothHands(Grabbable grabbable)
    {
        IsGrabbing[0] = true;
        IsGrabbing[1] = true;
        LastGrabbedHold[0] = grabbable;
        LastGrabbedHold[1] = grabbable;
        _handLerpTargetPositions[0] = grabbable.transform.position;
        _handLerpTargetPositions[1] = grabbable.transform.position;
    }

    private void OnEndTouch(int fingerId)
    {
        int randomIndex = UnityEngine.Random.Range(0, 3);
        string soundToPlay;

        // Assign soundToPlay based on the randomIndex
        switch (randomIndex)
        {
            case 0:
                soundToPlay = "RockGrab";
                break;
            case 1:
                soundToPlay = "RockGrab2";
                break;
            case 2:
                soundToPlay = "NoSound";
                break;
            default:
                soundToPlay = "RockGrab"; // Default case, should not be needed but added for safety
                break;
        }

       
        //Debug.Log($"TUCH END: {Time.time}      {fingerId}");
        _fingerIDIsSuccessfullyTouching[fingerId] = false;

        
        isClimbing = false;

        //Transform handTargetTransform = _touchHandIndex[touchIndex] == HandIndex.Left
        //    ? leftHandtargetTransform
        //    : rightHandtargetTransform;

        if (handTarget.nearest && ShouldAutoGrab)
        {
            stamina.DepleteChalk();
            AudioManager.Instance.PlayFX(soundToPlay);
            HandIndex handIndex = _touchHandIndex[fingerId];

            _handLerpTargetPositions[(int)handIndex] = handTarget.nearest.transform.position;
            //GetHandTargetTransform(handIndex).position = handTarget.nearest.transform.position;
            
            vfxController.ActivateDustAt(handTarget.nearest.transform.position);

            //handTargetTransform.position = handTarget.nearest.transform.position;
            IsGrabbing[(int)handIndex] = true;
            LastGrabbedHold[(int)handIndex] = handTarget.nearest;
            Grabbable currentGrabbable = handTarget.nearest;

            // If flying, then both hands should grab to the same hold
            bool isFlying = IsGrabbing[1 - (int)handIndex] == false;
            if (isFlying)
            {
                StartGrabbingHoldWithBothHands(handTarget.nearest);
            }
            else
            {
                // Update the grabbable object if the hand has moved to a new object
                if (LastGrabbedHold[(int)handIndex] != currentGrabbable)
                {
                    _previousGrabbedHold[(int)handIndex] = LastGrabbedHold[(int)handIndex];
                    LastGrabbedHold[(int)handIndex] = currentGrabbable;
                }

                // Check if both hands have moved to a new grabbable object
                if (LastGrabbedHold[0] != _previousGrabbedHold[0] && LastGrabbedHold[1] != _previousGrabbedHold[1])
                {
                    stamina.ResetStamina();
                    _previousGrabbedHold[0] = LastGrabbedHold[0]; // Update the previous grabbable objects after resetting stamina
                    _previousGrabbedHold[1] = LastGrabbedHold[1];
                }
            }
        }
    }

    private void UpdateTouchInputs()
    {
        if (slingShot && slingShot.playerIsTouched) return;

        bool[] newFingerIDIsTouching = new bool[2] { false, false };
        Vector2[] touchPositions = new Vector2[2] { Vector2.zero, Vector2.zero };
        foreach (UnityEngine.Touch touch in Input.touches)
        {
            if (touch.fingerId >= 2) continue;
            newFingerIDIsTouching[touch.fingerId] = true;
            touchPositions[touch.fingerId] = touch.position;
        }

        for (int i = 0; i < 2; i++)
        {
            //Touch touch = 
            if (newFingerIDIsTouching[i] && !_fingerIDIsTouching[i])
            {
                OnBeginTouch(i, touchPositions[i]);
            }
            if (!newFingerIDIsTouching[i] && _fingerIDIsSuccessfullyTouching[i])
            {
                OnEndTouch(i);
            }
        }

        _fingerIDIsTouching[0] = newFingerIDIsTouching[0];
        _fingerIDIsTouching[1] = newFingerIDIsTouching[1];

        Rigidbody rb = GetComponent<Rigidbody>();

        for (int fingerId = 0; fingerId < 2; fingerId++)
        {
            if (!_fingerIDIsSuccessfullyTouching[fingerId]) continue;

            Vector2 touchPosition = new Vector2(touchPositions[fingerId].x / (float)Screen.width,
                touchPositions[fingerId].y / (float)Screen.width);

            HandIndex handIndex = _touchHandIndex[fingerId];
            if (!IsGrabbing[1 - (int)handIndex]) continue;  // Only move this hand if the other hand is grabbing.

            // Only move this hand if this hand is not grabbing. Sometimes we might force IsGrabbing to be true even if you're still touching the screen, i.e.
            // when grabbing with both hands after flying.
            if (IsGrabbing[(int)handIndex]) continue;

            // we want to set this relative to the current position
            Vector3 handTargetPos = _handLerpTargetPositions[(int)handIndex];
            //Vector3 handTargetPos = GetHandTargetTransform(handIndex).position;

            Vector2 delta2 = touchPosition - _touchOldPosition[fingerId];

            Vector3 right = GetRightVectorAtPoint(handTargetPos);
            Vector3 delta = Parameters.HandMovementInputSpeed * (right * delta2.x + Vector3.up * delta2.y); //new Vector3(delta2.x, delta2.y, 0);

            handTargetPos += delta;

            // Clamp the hand to be on the cylinder
            Vector2 handTargetPosition2D = (new Vector2(handTargetPos.x, handTargetPos.z)).normalized * Parameters.WorldCylinderRadius;
            handTargetPos = new Vector3(handTargetPosition2D.x, handTargetPos.y, handTargetPosition2D.y);

            //GetHandTargetTransform(handIndex).position = handTargetPos;
            _handLerpTargetPositions[(int)handIndex] = handTargetPos;

            // add "swinging" force
            Vector3 force = right * delta.x * Parameters.SwingForceIntensityX + Vector3.up * Mathf.Max(delta.y, 0f) * Parameters.SwingForceIntensityY; // * Vector3.Dot(delta.normalized, bodyToHand.normalized);
            force = new Vector3(force.x, Mathf.Max(force.y, 0f), 0); // never add downwards force
            rb.AddForce(force);

            _touchOldPosition[fingerId] = touchPosition;
        }

        //_prevTouchCount = Input.touchCount;
    }

    private void Update()
    {
        
    }

    private void FlyingUpdatePhysics()
    {
        Ray ray = new Ray(transform.position - 1.5f*transform.forward, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit wallHit, Mathf.Infinity, 1))
        {
            Vector3 hitPoint = ray.GetPoint(wallHit.distance);

            //Debug.Log($"We have a hit! {Time.time}");
            
            Vector3 targetPos = transform.position + transform.forward * (wallHit.distance - 1.5f);
            targetPos -= transform.forward * 0.3f;
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
        }
        //else Debug.Log($"We don't have a hit... {Time.time}");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check win condition
        if (IsGrabbing[0] && IsGrabbing[1] &&
            LastGrabbedHold[0] != null && LastGrabbedHold[1] != null &&
            LastGrabbedHold[0].isPeak && LastGrabbedHold[1].isPeak)
        {
            GameManager.Instance.UpdateGameState(GameStateSO.State.Win);
        }

        // Disable auto-grab only if the player has jumped with no chalk
        // Disable auto-grab when there is no chalk and the player has jumped


        Rigidbody rb = GetComponent<Rigidbody>();

        if (InputIsEnabled)
        {
            if (InputIsEnabledSlingShot)
            {
                slingShot.UpdateSlingShot();
            }
            UpdateTouchInputs();
        }

        // orient the body to face outwards from the cylinder
        float rotation = GetRotationAtPoint(transform.position);

        _leaningRotation = Mathf.Lerp(_leaningRotation, _leaningRotationTarget, 0.1f);
        _leaningRotationTarget = 0f;

        transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * rotation, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(_leaningRotation, new Vector3(0, 0, 1));

        if (!IsGrabbing[0] && !IsGrabbing[1])
        {
            FlyingUpdatePhysics();
            //rb.AddForce(transform.forward * Parameters.OnFlyingCylinderGravity, ForceMode.Force);
        }

        // apply hand constraints
        // so, if we're grabbing with one hand, we don't want to constraint the body but to move the hand instead

        Vector3[] constrainedBodyPos = new Vector3[2] { transform.position, transform.position };

        for (int i = 0; i < 2; i++)
        {
            HandIndex handIndex = (HandIndex)i;
            Transform handTransform = GetHandTargetTransform(handIndex);

            Vector3 bodyToHand = handTransform.position - transform.position;
            Vector3 bodyToHandLerpTarget = _handLerpTargetPositions[i] - transform.position;
            
            Vector3 right = GetRightVectorAtPoint(transform.position);
            Vector3 openSide = right * (handIndex == HandIndex.Left ? -1f : +1f);

            float targetArmLength = Parameters.MaxArmLength;
            if (IsGrabbing[i])
            {
                targetArmLength *= IsGrabbing[1 - i] ? Parameters.GrabArmLengthMultiplier : Parameters.GrabArmLengthMultiplierButItsTheOnlyHandGrabbing;
            }
            else
            {
                float okCrossRange = Mathf.Clamp(Parameters.ArmCrossShrinkFactor * Vector3.Dot(bodyToHand, openSide) + 1f, 0f, 1f);
                targetArmLength *= okCrossRange; // shrink arm on the wrong side
            }

            _armLengths[i] = Mathf.Lerp(_armLengths[i], targetArmLength, Parameters.ArmLengthSmoothInterpolationSpeed);

            if (IsGrabbing[i])
            {
                // rope constraint
                if (bodyToHand.magnitude > _armLengths[i])
                {
                    Vector3 targetPos = handTransform.position - bodyToHand.normalized * _armLengths[i];
                    constrainedBodyPos[i] = Vector3.Lerp(constrainedBodyPos[i], targetPos, Parameters.ArmConstraintStiffness);
                    
                    Vector3 targetVel = rb.velocity - bodyToHand.normalized * Vector3.Dot(bodyToHand.normalized, rb.velocity);// Mathf.Min(Vector3.Dot(bodyToHand.normalized, rb.velocity), 0);
                    rb.velocity = Vector3.Lerp(rb.velocity, targetVel, Parameters.ArmConstraintStiffness);
                }
            }
            else
            {
                // move hand towards the body
                if (bodyToHandLerpTarget.magnitude > _armLengths[i])
                {
                    _handLerpTargetPositions[i] = transform.position + bodyToHandLerpTarget.normalized * Mathf.Min(bodyToHandLerpTarget.magnitude, _armLengths[i]);
                }

                // If flying, move hand towards velocity
                if (!IsGrabbing[1 - i])
                {
                    // Debug.DrawRay(transform.position, slingShot.lastJumpDirection * 10, Color.red);
                    // Debug.DrawRay(transform.position, transform.TransformDirection(slingShot.lastJumpDirectionLocal) * 10, Color.green);
                    Vector3 fixedLocalPos = slingShot.lastJumpDirectionLocal * Parameters.MaxArmLength + openSide * 0.1f + Vector3.forward * 0.5f;
                    // print($"local pos {i}: {localPos}");
                    Vector3 worldPos = transform.TransformPoint(fixedLocalPos);
                    // Debug.DrawRay(transform.position, worldPos - transform.position, Color.yellow);
                    _handLerpTargetPositions[i] = worldPos;
                    // _handLerpTargetPositions[i] = transform.position + slingShot.lastJumpDirection * Parameters.MaxArmLength + openSide * 0.1f;
                    _leaningRotationTarget = slingShot.lastJumpDirection.x*-5f;
                }
                else
                {
                    // add "reaching" force

                    Vector3 otherHandPos = _handLerpTargetPositions[1 - i];
                    Vector3 offsettedBodyTargetPos = otherHandPos + Parameters.ReachingRadiusScale * bodyToHandLerpTarget;
                    Vector3 reachingForce = (offsettedBodyTargetPos - transform.position) * Parameters.ReachingForceIntensity;

                    constrainedBodyPos[i] = Vector3.Lerp(constrainedBodyPos[i], offsettedBodyTargetPos, Parameters.ReachingForceSmoothInterpSpeed);
                    rb.AddForce(reachingForce);

                    _leaningRotationTarget = bodyToHandLerpTarget.x * -26f;
                }
            }
        }

        transform.position = (constrainedBodyPos[0] + constrainedBodyPos[1]) * 0.5f;

        leftHandtargetTransform.position = Vector3.Lerp(leftHandtargetTransform.position, _handLerpTargetPositions[0], Parameters.ArmSmoothInterpolationSpeed);
        rightHandtargetTransform.position = Vector3.Lerp(rightHandtargetTransform.position, _handLerpTargetPositions[1], Parameters.ArmSmoothInterpolationSpeed);
    }

    public void OnCollisionEnter(Collision collision)
    {
        HasTouchedWallAfterJump = true;
    }

    public void ControlBodyPart(Rigidbody bodyPartRB)
    {
        bodyPartRB.isKinematic = true;
        bodyPartRB.useGravity = false;
    }

    public void FreeBodyPart(Rigidbody bodyPartRB)
    {
        bodyPartRB.isKinematic = false;
        bodyPartRB.useGravity = true;
    }
}