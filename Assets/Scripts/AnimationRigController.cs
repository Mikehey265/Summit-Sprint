using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationRigController : MonoBehaviour
{
    [Header("Hand")] public Rig handRig;
    public Transform leftHandPos;
    public Transform rightHandPos;
    public Transform leftHandHint;
    public Transform rightHandHint;

    [Header("Foot")] public Transform leftFootPos;
    public Transform rightFootPos;
    public Transform leftFootHint;
    public Transform rightFootHint;

    [Header("Head")] public Transform headTarget;
    public float headTargetLerpSpeed = 1;

    [Header("Animation Rigged Model")] public Transform animationRiggedModel;
    public float modelLerpSpeed = 20;
    public float dragRange = 0.3f;
    public Vector3 initLocalPos;
    public float animatorBlendValue = 0;
    private bool _awakened = false;
    private Animator _animator;


    private SlingShot _slingShot;
    private BodyPhysics _bodyPhysics;

    private void OnDrawGizmos()
    {
        if (!_awakened)
        {
            initLocalPos = animationRiggedModel.localPosition;
        }
    }

    private void Awake()
    {
        _awakened = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        _bodyPhysics = transform.parent.GetComponent<BodyPhysics>();
        _slingShot = transform.parent.GetComponent<SlingShot>();
        _animator = animationRiggedModel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HeadAiming();
        CorrectHandRotation();
        SetModelTargetAndFollow();
        //print("Delta: " + _slingShot.inputDelta);
        UpdateAniamtionBlendValue();
    }

    void UpdateAniamtionBlendValue()
    {
        float blendTarget;
        if (!_bodyPhysics.IsGrabbing[0] && !_bodyPhysics.IsGrabbing[1] && !_bodyPhysics.handTarget.nearest)
        {
            blendTarget = 1;
            if (GameManager.Instance.isGameLost)
            {
                _animator.SetBool("Die", true);
            }
        }
        else
        {
            blendTarget = 0;
        }

        animatorBlendValue = Mathf.Lerp(animatorBlendValue, blendTarget, Time.deltaTime * 10) ;
        _animator.SetFloat("Blend", animatorBlendValue);
        handRig.weight = 1 - animatorBlendValue;
    }


    void HeadAiming()
    {
        if (_bodyPhysics.IsGrabbing[0] == false && _bodyPhysics.IsGrabbing[1] == true)
        {
            headTarget.position = Vector3.Lerp(headTarget.position, leftHandPos.position,
                headTargetLerpSpeed * Time.deltaTime);
        }

        if (_bodyPhysics.IsGrabbing[0] == true && _bodyPhysics.IsGrabbing[1] == false)
        {
            headTarget.position = Vector3.Lerp(headTarget.position, rightHandPos.position,
                headTargetLerpSpeed * Time.deltaTime);
        }

        if (_slingShot.inputDelta.magnitude > 0.01)
        {
            headTarget.position =
                transform.TransformPoint(-_slingShot.inputDelta * 5 + new Vector3(0, 0, 3));
        }
    }

    void CorrectHandRotation()
    {
        leftHandPos.forward = rightHandPos.forward = transform.forward;
    }

    void SetModelTargetAndFollow()
    {
        animationRiggedModel.localPosition = Vector3.Lerp(animationRiggedModel.localPosition,
            initLocalPos + _slingShot.inputDelta * dragRange,
            modelLerpSpeed * Time.deltaTime);
    }
}