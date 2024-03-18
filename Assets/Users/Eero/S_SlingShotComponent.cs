using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Jobs;
using UnityEngine;

public class S_SlingShotComponent : MonoBehaviour
{
    public GameObject SlingVisualizePoint;
    private bool hasJumped = false;
    public bool playerIsTouched;
    public GameObject player;
    

    private bool _wasTouching = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    
    void Update()
    {
        Debug.Log("jumping = " + hasJumped);
            SlingShot();
            StopMovement();
            
            if (Input.touchCount == 0)
            {
                // Set playerIsTouched to false
                playerIsTouched = false;
            }
    }

    private void SlingShot()
{
    bool isTouching = Input.touchCount > 0;
    if (isTouching)
    {
        Touch touch = Input.GetTouch(0);
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y));

        Plane xy_plane = new Plane(new Vector3(0, 0, 1), 0);
        float t = 0f;
        if (xy_plane.Raycast(ray, out t))
        {
            Vector3 hitPoint = ray.GetPoint(t);

            // Check if the hit point is on the player
            int layerMask = 1 << 8; // 8 is the layer number for the player
            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("Raycast hit: " + hit.transform.name); // Log the name of the hit object

                if (hit.transform.CompareTag("Player")) // if the hit object is the player
                {
                    playerIsTouched = true;
                    Debug.Log("Touched the player");
                }
            }
            

            Vector3 hitPointToPos = transform.position - hitPoint;
            if (!_wasTouching && hitPointToPos.magnitude > 1f)
            {
                isTouching = false;
            }

            // clamp to correct range
            hitPoint = transform.position - hitPointToPos.normalized * Mathf.Min(hitPointToPos.magnitude, 3f);
            SlingVisualizePoint.transform.position = hitPoint;

            //jump direction visualizer
            Vector3 direction = hitPoint - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            SlingVisualizePoint.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
    else if (_wasTouching)
    {
        // launch!
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.AddForce(4f*(transform.position - SlingVisualizePoint.transform.position), ForceMode.Impulse);
        hasJumped = true;
    }
    SlingVisualizePoint.SetActive(isTouching);
    _wasTouching = isTouching;
}
private void StopMovement()
{
    bool isTouching = Input.touchCount > 0;
    if(hasJumped && isTouching)
    {
        Touch touch = Input.GetTouch(0);
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                Rigidbody rigidbody = GetComponent<Rigidbody>();
                rigidbody.velocity = Vector3.zero;
                rigidbody.isKinematic = true;//grab on to something here, remove isKinematic
                hasJumped = false;
            }
        }
    }
}///////
}
