using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Jobs;
using UnityEngine;

public class SlingShotMechanic : MonoBehaviour
{
    public GameObject SlingVisualizePoint;

    private bool _wasTouching = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {

        bool isTouching = Input.touchCount > 0;
        if (isTouching)
        {
            Touch touch = Input.GetTouch(0);
            float x = touch.position.x / (float)Screen.width;
            float y = touch.position.y / (float)Screen.height;
            
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y));
            
            Plane xy_plane = new Plane(new Vector3(0, 0, 1), 0);
            float t = 0f;
            if (xy_plane.Raycast(ray, out t))
            {
                Vector3 hitPoint = ray.GetPoint(t);

                Vector3 hitPointToPos = transform.position - hitPoint;
                if (!_wasTouching && hitPointToPos.magnitude > 1f)
                {
                    isTouching = false;
                }

                // clamp to correct range
                hitPoint = transform.position - hitPointToPos.normalized * Mathf.Min(hitPointToPos.magnitude, 3f);
                SlingVisualizePoint.transform.position = hitPoint;
            }

        }
        else if (_wasTouching)
        {
            // launch!

            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.AddForce(4f*(transform.position - SlingVisualizePoint.transform.position), ForceMode.Impulse);
        }
        
        
        SlingVisualizePoint.SetActive(isTouching);

        _wasTouching = isTouching;
    }

}
