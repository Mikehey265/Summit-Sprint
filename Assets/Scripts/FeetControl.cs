using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FeetControl : MonoBehaviour
{
    [Header("Controllable Part")] public Transform leftFootTarget;
    public Transform leftFootPos;
    public Transform leftFootHint;
    public Transform rightFootTarget;
    public Transform rightFootPos;
    public Transform rightFootHint;
    public float footLerpSpeed = 5;
    public float footLength = 0.1f;
    private Vector3 _leftFootTargetProjection;
    private Vector3 _rightFootTargetProjection;


    [Header("Reference From Character")] public Transform hips;
    public Rig[] rigs;
    [Range(0, 1)] public float rigWeight = 1;
    [SerializeField] private float _rigWeightTarget = 1;
    public SlingShot slingShot;
    public BodyPhysics bodyPhysics;

    [Header("Foot Moving Range")] public float movingAngleInDgree = 30f;
    public float offsetRatio = 0.8f;
    public float footResetRangeMin = 0.5f;
    public float footResetRangeMax = 0.8f;
    public float footLimitRange = 1.2f;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hips.position, footResetRangeMin);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hips.position, footResetRangeMax);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hips.position, footLimitRange);
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if (InTheAir())
        {
            FreeFeetTarget();
        }
        else if (!slingShot.playerIsTouched) // keep feet placement when slingshotting
        {
            ControlFeetTarget();
        }

        SetFeetTarget();
        LerpFootPos();
        LerpFootRigWeight();

        // Debug.DrawRay(hips.position, hips.forward * 10, Color.red);
        // Debug.DrawRay(hips.position, leftFootTarget.forward * 10, Color.yellow);
        // Debug.DrawRay(hips.position, initLeftFootRotation * hips.forward * 10, Color.green);
    }

    void FreeFeetTarget()
    {
        // foreach (var rig in rigs)
        // {
        //     rigWeight = 0;
        //     rig.weight = rigWeight;
        // }
        _rigWeightTarget = 0;
    }

    void ControlFeetTarget()
    {
        // foreach (var rig in rigs)
        // {
        //     rigWeight = 1;
        //     rig.weight = rigWeight;
        // }
        _rigWeightTarget = 1;
    }

    bool InTheAir()
    {
        // if has jumped and both hands are not grabbing
        //if ((slingShot != null && slingShot.hasJumped) &&
        //    (!bodyPhysics.IsGrabbing[0] && !bodyPhysics.IsGrabbing[1]) &&
        //    !bodyPhysics.HasTouchedWallAfterJump) return true;
        if ((!bodyPhysics.IsGrabbing[0] && !bodyPhysics.IsGrabbing[1])) return true;
        return false;
    }

    void SetFeetTarget()
    {
        if (!IsFootInSideLimitedRange(leftFootTarget, 0))
        {
            leftFootTarget.position = FindRandomPosForFoot(movingAngleInDgree, footResetRangeMin, footResetRangeMax, 0);

            Ray ray = new Ray(leftFootTarget.position,
                Vector3.Cross(Vector3.Cross(Vector3.up, hips.forward), Vector3.up));
            if (Physics.Raycast(ray, out RaycastHit wallHit, Mathf.Infinity, 1))
            {
                _leftFootTargetProjection = wallHit.point -
                                            Vector3.Cross(Vector3.Cross(Vector3.up, hips.forward), Vector3.up)
                                                .normalized * footLength;
            }

            leftFootPos.forward = hips.forward;
        }

        if (!IsFootInSideLimitedRange(rightFootTarget, 1))
        {
            rightFootTarget.position =
                FindRandomPosForFoot(movingAngleInDgree, footResetRangeMin, footResetRangeMax, 1);

            Ray ray = new Ray(rightFootTarget.position,
                Vector3.Cross(Vector3.Cross(Vector3.up, hips.forward), Vector3.up));
            if (Physics.Raycast(ray, out RaycastHit wallHit, Mathf.Infinity, 1))
            {
                _rightFootTargetProjection = wallHit.point -
                                             Vector3.Cross(Vector3.Cross(Vector3.up, hips.forward), Vector3.up)
                                                 .normalized * footLength;
            }

            rightFootPos.forward = hips.forward;
        }
    }

    void LerpFootPos()
    {
        // leftFootPos.position =
        //     Vector3.Lerp(leftFootPos.position, leftFootTarget.position, footLerpSpeed * Time.deltaTime);
        // rightFootPos.position =
        //     Vector3.Lerp(rightFootPos.position, rightFootTarget.position, footLerpSpeed * Time.deltaTime);

        leftFootPos.position =
            Vector3.Lerp(leftFootPos.position, _leftFootTargetProjection, footLerpSpeed * Time.deltaTime);
        rightFootPos.position =
            Vector3.Lerp(rightFootPos.position, _rightFootTargetProjection, footLerpSpeed * Time.deltaTime);
    }

    void LerpFootRigWeight()
    {
        rigWeight = Mathf.Lerp(rigWeight, _rigWeightTarget, footLerpSpeed / 5 * Time.deltaTime);
        foreach (var rig in rigs)
        {
            rig.weight = rigWeight;
        }
    }

    bool IsFootInSideLimitedRange(Transform footTargetTransform, int leftOrRight)
    {
        bool result = true;

        // distance test, if too far away
        float dist;
        dist = Vector3.Distance(footTargetTransform.position, hips.position);
        if (dist > footLimitRange || dist < footResetRangeMin) return false;

        // angle, if angle is to much for a human body
        Vector3 footDir = (footTargetTransform.position - hips.position).normalized;
        float offsetAngle = movingAngleInDgree * offsetRatio;
        if (leftOrRight == 0) offsetAngle *= -1;
        Vector3 forward = hips.forward;
        Vector3 downWithOffset = Quaternion.AngleAxis(offsetAngle, forward) * Vector3.down;
        if (Vector3.Dot(footDir, downWithOffset) < Mathf.Cos(movingAngleInDgree * Mathf.Deg2Rad)) return false;


        return result;
    }

    Vector3 FindRandomPosForFoot(float movingAngleInDgree, float minRadiusFromHip, float maxRadiusFromHip,
        int leftOrRIght)
    {
        float offsetAngle = 0.8f * movingAngleInDgree;
        if (leftOrRIght == 0) offsetAngle *= -1;

        // find a position on the vertical surface
        float randomAngle = Random.Range(-movingAngleInDgree, movingAngleInDgree) + offsetAngle;
        print(randomAngle);
        Vector3 forward = hips.forward;
        Vector3 dir = Quaternion.AngleAxis(randomAngle, forward) * Vector3.down;

        Vector3 newPos = new Vector3();
        newPos = hips.position + dir * Random.Range(minRadiusFromHip, maxRadiusFromHip);
        return newPos;
    }
}