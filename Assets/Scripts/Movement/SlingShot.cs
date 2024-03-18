using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class SlingShot : MonoBehaviour
{
    public GameObject SlingVisualizePoint;
    public Image ArrowImage;

    public bool hasJumped = false;
    public bool playerIsTouched;
    public bool hasPlayerJumpedThisRun;
    public int jumpCount;
    public GameObject player;
    private PlayerStamina stamina;
    private BodyPhysics bodyPhysics;


    public BodyPhysicsParameters Parameters;
    //[SerializeField] private float _jumpForce = 10f;

    //public MeshCollider meshCollider;
    private bool _wasTouching = false;
    private bool _shouldCancel = false;

    public bool hasJumpedWithNoChalk = false;
    // public float forwardForce = .7f;
    // public float backwardForce = 1f;
    
    public Vector3 lastJumpDirection = Vector3.zero;
    public Vector3 lastJumpDirectionLocal = Vector3.zero;

    private bool initTouched = false;
    private Vector2 initTouch;
    public Vector3 inputDelta = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        stamina = GetComponent<PlayerStamina>();
        bodyPhysics = GetComponent<BodyPhysics>();
        hasPlayerJumpedThisRun = false;
        jumpCount = 0;
    }

    private void Launch()
    {
        // launch!
        bodyPhysics.IsGrabbing[0] = false;
        bodyPhysics.IsGrabbing[1] = false;
        bodyPhysics.HasTouchedWallAfterJump = false;
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        Vector3 jumpVector = (transform.position - SlingVisualizePoint.transform.position);

        rigidbody.AddForce(Parameters.JumpForce * jumpVector, ForceMode.Impulse);

        transform.position += transform.forward * (-0.02f);
        rigidbody.AddForce(-transform.forward * Parameters.JumpBackwardForce, ForceMode.Impulse);

        lastJumpDirection = jumpVector.normalized;
        lastJumpDirectionLocal = transform.InverseTransformDirection(lastJumpDirection); // make it from world to local
        // bodyPhysics._handLerpTargetPositions[0] = transform.position + jumpDirection.normalized * Parameters.MaxArmLength;
        // bodyPhysics._handLerpTargetPositions[1] = transform.position + jumpDirection.normalized * Parameters.MaxArmLength;

        stamina.DepleteChalkOnSlingShot();
        //Debug.DrawLine(jumpDirection, transform.position, Color.blue);
        hasJumped = true;
        hasPlayerJumpedThisRun = true;
        jumpCount++;

        inputDelta = Vector3.zero;

        // if (stamina.currentChalk <= 0)
        // {
        //     hasJumpedWithNoChalk = true;
        // }
    }

    public void UpdateSlingShot()
    {
        //Debug.Log("jumping = " + hasJumped);

        if (bodyPhysics.isClimbing == false && bodyPhysics.InputIsEnabled == true && stamina.currentChalk > 0)
        {
            bool isTouching = Input.touchCount > 0;
            if (isTouching)
            {
                Touch touch = Input.GetTouch(0);

                inputDelta = Vector3.zero;

                Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y));

                float t = 0f;
                if (Physics.Raycast(ray, out RaycastHit hitt, Mathf.Infinity, 1))
                {
                    t = hitt.distance;
                    Debug.DrawLine(ray.origin, ray.GetPoint(t), Color.red);
                    Vector3 hitPoint = ray.GetPoint(t);

                    Vector3 hitPointToPos = transform.position - hitPoint;
                    bool isFlying = !bodyPhysics.IsGrabbing[0] && !bodyPhysics.IsGrabbing[1];

                    if (hitPointToPos.magnitude < Parameters.SlingshotInputRadius && !isFlying)
                    {
                        playerIsTouched = true;
                    }

                    float radius = 1.3f;     
                    _shouldCancel = hitPointToPos.magnitude < radius * 0.5f;

                    // clamp to correct range
                    hitPoint = transform.position - hitPointToPos.normalized * Mathf.Min(hitPointToPos.magnitude, radius);
                    if (_shouldCancel) hitPoint = transform.position;

                    
                    //jump direction visualizer
                    Vector3 direction = hitPoint - transform.position;
                    SlingVisualizePoint.transform.position = hitPoint;
                    SlingVisualizePoint.transform.up = direction.normalized;

                    if (playerIsTouched)
                    {
                        Vector3 localDir = transform.InverseTransformVector(direction);
                        inputDelta = new Vector3(localDir.x, localDir.y, 0);

                        Vector3 delta = new Vector3(Vector3.Dot(direction, transform.right), direction.y, 0);
                        float angle = Mathf.Atan2(delta.y, delta.x) + Mathf.PI*0.5f;

                        float aspectRatio = (float)Screen.height / (float)Screen.width;
                        float arrowX = 0.5f - 0.2f * delta.x * aspectRatio;
                        float arrowY = 0.5f - 0.2f * delta.y;
                        ArrowImage.transform.position = new Vector3(arrowX * (float)Screen.width, arrowY * (float)Screen.height, 0f);
                        ArrowImage.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, new Vector3(0, 0, 1));

                        float scale = delta.magnitude;
                        ArrowImage.transform.localScale = new Vector3(scale, scale, scale);
                    }
                    else
                    {
                        inputDelta = Vector3.zero;
                    }
                }
            }
            else if (_wasTouching && playerIsTouched && !_shouldCancel)
            {
                Launch();
            }

            _wasTouching = isTouching;
        }

        if (Input.touchCount == 0)
        {
            playerIsTouched = false;
        }

        ArrowImage.gameObject.SetActive(playerIsTouched);
        SlingVisualizePoint.SetActive(playerIsTouched);
    }
}