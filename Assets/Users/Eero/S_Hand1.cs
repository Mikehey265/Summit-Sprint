using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_Hand1 : MonoBehaviour
{
    public GameObject Body;

    private Vector3 _relativePosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // we can use gamepads to test! left and right sticks.
        //if (Input.GetKey(KeyCode.I))
        //{
        //
        //}

        // let's do one-hand controls for now.

        Rigidbody rigidbody = Body.GetComponent<Rigidbody>();

        float targetX = 2f * ((Input.mousePosition.x / (float)Screen.width) * 2f - 1f);
        float targetY = 2f * ((Input.mousePosition.y / (float)Screen.height) * 2f - 1f);
        //targetX = Mathf.Clamp(2f*targetX, -1f, 1f);
        //targetY = Mathf.Clamp(2f*targetY, -1f, 1f);

        float x1 = Input.GetAxis("Horizontal");
        float y1 = Input.GetAxis("Vertical");
        float x2 = Input.GetAxis("Horizontal2");
        float y2 = Input.GetAxis("Vertical2");
        //Debug.Log($"{x1} {y1}   {x2} {y2}");

        if (Input.GetMouseButtonDown(1))
        {
            transform.position = Body.transform.position + new Vector3(targetX, targetY, 0f);
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 target = transform.position - new Vector3(targetX, targetY, 0f);

            // force should be bigger the lower the body is
            float force_amount = 1f;// Mathf.Clamp(0.5f*(transform.position.y - Body.transform.position.y), 0f, 1f);
            
            Vector3 bodyToHands = Body.transform.position - transform.position;
            float targetRotDeg = Mathf.Rad2Deg * Mathf.Atan2(bodyToHands.y, bodyToHands.x) + 90f;
            //float rotDeg =

            Body.transform.rotation = Quaternion.AngleAxis(targetRotDeg, new Vector3(0, 0, 1));

            //Body.transform.LookAt(transform.position, new Vector3(0, 1, 0));
            rigidbody.AddForce(10f * force_amount * (target - Body.transform.position));
            //rigidbody.position = Vector3.Lerp(rigidbody.position, target, 0.01f);

            rigidbody.drag = 10f * force_amount;
        }
        else
        {
            rigidbody.drag = 0.1f;
            transform.position = Body.transform.position + _relativePosition;
        }

        if (Input.GetMouseButton(1))
        {
            _relativePosition = transform.position - Body.transform.position;
        }

        rigidbody.AddForce(new Vector3(0, -1f, 0));
        // add force to the body to make sure it sticks
    }
}
