using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FollowingTargetForFreelookCamera : MonoBehaviour
{
    // public Transform followingTarget;

    public Transform cameraStartPoint;
    public BodyPhysics character;
    public AnimationCurve cameraLerpCurve;
    public float cameraFallingTime = 3f;
    public float lerpSpeed = 1f;
    bool finishFalling = true;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (EditorApplication.isPlaying) return;
        if (cameraStartPoint) transform.position = cameraStartPoint.position;
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
        if (cameraStartPoint)
        {
            StartCoroutine(CameraFallsDown());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 pos = followingTarget.position;
        // transform.position = pos;
        if (!finishFalling) return;
        transform.position = Vector3.Lerp(transform.position,
            (character.rightHandtargetTransform.position + character.leftHandtargetTransform.position) / 2,
            lerpSpeed * Time.deltaTime);
    }

    IEnumerator CameraFallsDown()
    {
        float timePassed = 0;
        finishFalling = false;
        Vector3 start = cameraStartPoint.position;
        Vector3 end = (character.rightHandtargetTransform.position + character.leftHandtargetTransform.position) / 2;
        while (timePassed < cameraFallingTime)
        {
            float t = cameraLerpCurve.Evaluate(timePassed / cameraFallingTime);
            transform.position = Vector3.Lerp(start, end, t);

            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        finishFalling = true;
    }
}