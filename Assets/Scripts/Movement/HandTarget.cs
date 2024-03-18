using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandTarget : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform selectedHandPos;

    public BodyPhysics bodyPhysics;
    public BodyPhysicsParameters Parameters;
    public List<Grabbable> grabbablesInRange;
    public Grabbable nearest;
    public Transform snapTarget;

    public bool isSnap = false;
    public float lerpSpeed = 1f;

    private void Awake()
    {
        grabbablesInRange = new List<Grabbable>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (!selectedHandPos) return;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            int _ = 0;
        }
        snapTarget = transform;

        if (nearest)
        {
            nearest.SetOutlineEnabled(false);
        }
        nearest = null;

        if (selectedHandPos)
        {
            float minDist = float.MaxValue;
            foreach (var grabbable in grabbablesInRange)
            {
                grabbable.currentState = Grabbable.GrabbableState.InRange;
                float dist = Vector3.Distance(selectedHandPos.position, grabbable.transform.position);
                bool canReach = Vector3.Distance(grabbable.transform.position, transform.position) < Parameters.MaxArmLength + Parameters.GrabbableExtraReachDistance;

                if (dist < minDist && canReach)
                {
                    minDist = dist;
                    nearest = grabbable;
                }
            }
        }

        if (nearest)
        {
            //Debug.Log($"bodyPhysics.IsGrabbing[0] {bodyPhysics.IsGrabbing[0]} bodyPhysics.IsGrabbing[1] {bodyPhysics.IsGrabbing[1]}");
            //
            nearest.SetOutlineEnabled(!bodyPhysics.IsGrabbing[0] || !bodyPhysics.IsGrabbing[1]);
            //nearest.SetOutlineEnabled(true);

            nearest.currentState = Grabbable.GrabbableState.Selected;
        }
        /*
        selectedHandPos.transform.position = Vector3.Lerp(
            selectedHandPos.transform.position,
            snapTarget.position,
            lerpSpeed * Time.deltaTime
        );*/
    }

        private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Grabbable>())
        {
            isSnap = true;

            Grabbable newGrabbable = other.GetComponent<Grabbable>();
            newGrabbable.currentState = Grabbable.GrabbableState.InRange;
            grabbablesInRange.Add(newGrabbable);

            snapTarget = other.transform;
            print("Snap to " + other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            isSnap = false;

            Grabbable exitingGrabbable = other.GetComponent<Grabbable>();
            exitingGrabbable.currentState = Grabbable.GrabbableState.OutOfRange;
            //exitingGrabbable.SetOutlineEnabled(false);
            grabbablesInRange.Remove(exitingGrabbable);

            print("Leave " + other.gameObject.name);
        }
    }

    //public void SelectFollowingHand(Transform handPos)
    //{
    //    selectedHandPos = handPos;
    //    transform.position = handPos.position;
    //}
}