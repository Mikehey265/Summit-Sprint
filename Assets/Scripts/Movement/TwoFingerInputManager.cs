using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TwoFingerInputManager : MonoBehaviour
{
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] public BodyPhysics character;
    [SerializeField] public HandTarget handTarget;

    //public bool FlyingMode;

    private InputAction _touchLeftAction;
    private InputAction _touchRightAction;
    private InputAction _touchPositionAction;
    private InputAction _touchPressAction;

    private Vector3 _tempStartPosLeft;
    private Vector3 _tempStartPosRight;

    private void Awake()
    {
        _touchLeftAction = playerInput.actions["MoveLeftHand"];
        _touchRightAction = playerInput.actions["MoveRightHand"];
        _touchPressAction = playerInput.actions["TouchPress"];
        _touchPositionAction = playerInput.actions["TouchPosition"];
    }

    private void OnEnable()
    {
        //_touchLeftAction.started += LeftStartTouch;
        //_touchLeftAction.performed += LeftTouching;
        //_touchLeftAction.canceled += LeftEndTouch;
        //
        //_touchRightAction.started += RightStartTouch;
        //_touchRightAction.performed += RightTouching;
        //_touchRightAction.canceled += RightEndTouch;

        _touchPressAction.performed += TouchPressed;
        _touchPositionAction.performed += TouchPositionStart;
    }

    private void OnDisable()
    {
        //_touchLeftAction.started -= LeftStartTouch;
        //_touchLeftAction.performed -= LeftTouching;
        //_touchLeftAction.canceled -= LeftEndTouch;
        //
        //_touchRightAction.started -= RightStartTouch;
        //_touchRightAction.performed -= RightTouching;
        //_touchRightAction.canceled -= RightEndTouch;
        _touchPressAction.performed -= TouchPressed;
        _touchPositionAction.performed -= TouchPositionStart;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void TouchPressed(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        // print("Touch: " + value);
    }
    private void TouchPositionStart(InputAction.CallbackContext context)
    {
        // print("Start Touch Screen at " + context.ReadValue<Vector2>());
    }

    private void LeftStartTouch(InputAction.CallbackContext context)
    {
        // Vector2 value = context.ReadValue<Vector2>();
        // Debug.Log(value);
        //_tempStartPosLeft = character.leftHandtargetTransform.position;
        //character.ControlBodyPart(character.leftHandRB);
        //handTarget.SelectFollowingHand(character.leftHandtargetTransform);
    }

    private void LeftTouching(InputAction.CallbackContext context)
    {
        //Vector2 value = context.ReadValue<Vector2>();
        //Vector3 newPosition = new Vector3(
        //    _tempStartPosLeft.x + value.x,
        //    _tempStartPosLeft.y + value.y,
        //    _tempStartPosLeft.z);
        //handTarget.transform.position = newPosition;
        // Debug.Log(value);
    }

    private void LeftEndTouch(InputAction.CallbackContext context)
    {
        //Vector2 value = context.ReadValue<Vector2>();
        //Debug.Log(value);
        //
        //if (handTarget.nearest)
        //{
        //    handTarget.selectedHandPos.position = handTarget.nearest.transform.position;
        //}
        //else
        //{
        //    character.FreeBodyPart(character.leftHandRB);
        //}
        //
        //handTarget.selectedHandPos = null;
        // StartCoroutine(BackToPreviousSnap(handTarget.selectedHandPos, handTarget.snapTarget));
    }
    
    private void RightStartTouch(InputAction.CallbackContext context)
    {
        // Vector2 value = context.ReadValue<Vector2>();
        // Debug.Log(value);
        //_tempStartPosRight = character.rightHandtargetTransform.position;
        //handTarget.SelectFollowingHand(character.rightHandtargetTransform);
    }
    
    private void RightTouching(InputAction.CallbackContext context)
    {
        //Vector2 value = context.ReadValue<Vector2>();
        //Vector3 newPosition = new Vector3(
        //    _tempStartPosRight.x + value.x,
        //    _tempStartPosRight.y + value.y,
        //    _tempStartPosRight.z);
        //handTarget.transform.position = newPosition;
        // Debug.Log(value);
    }
    
    private void RightEndTouch(InputAction.CallbackContext context)
    {
        //Vector2 value = context.ReadValue<Vector2>();
        //Debug.Log(value);
        //
        //if (handTarget.nearest)
        //{
        //    handTarget.selectedHandPos.position = handTarget.nearest.transform.position;
        //}
        //
        //handTarget.selectedHandPos = null;
    }

    IEnumerator BackToPreviousSnap(Transform currentHand, Transform targetSnap)
    {
        while (currentHand.position != targetSnap.position)
        {
            print("back to previous snap");
            currentHand.position = Vector3.Lerp(currentHand.position, targetSnap.position, Time.deltaTime * 15);
            yield return new WaitForEndOfFrame();
        }    
    }
    
}