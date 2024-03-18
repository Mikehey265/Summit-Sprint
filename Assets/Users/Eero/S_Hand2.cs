using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class S_Hand2 : MonoBehaviour
{
    public GameObject Body;
    public bool IsRight;

    private bool _wasHolding;
    private Vector3 _relativePosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool IsHolding()
    {
        KeyCode key = IsRight ? KeyCode.JoystickButton7 : KeyCode.JoystickButton6;
        return Input.GetKey(key);
    }

    public Vector3 GetAxes()
    {
        float x = Input.GetAxis(IsRight ? "Horizontal2" : "Horizontal");
        float y = Input.GetAxis(IsRight ? "Vertical2" : "Vertical");
        
        float length = Mathf.Sqrt(x * x + y * y);
        if (length > 1)
        {
            x /= length;
            y /= length;
            length = 1;
            x *= length;
            y *= length;
        }

        return new Vector3(x, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // we can use gamepads to test! left and right sticks.
        // let's do one-hand controls for now.

        Rigidbody rigidbody = Body.GetComponent<Rigidbody>();

        bool isHolding = IsHolding();
        if (isHolding)
        {
            if (!_wasHolding)
            {
                Vector2 axes = GetAxes();
                transform.position = Body.transform.position + new Vector3(axes.x, axes.y, 0f);
            }

            // constraint force
            Vector3 delta = transform.position - Body.transform.position;
            float force_scale = 10f * Mathf.Max(delta.magnitude - 1f, 0f);

            if (force_scale > 0.00001f)
            {
                //Vector3 target_velocity = rigidbody.velocity - delta.normalized * Mathf.Max(Vector3.Dot(rigidbody.velocity, delta.normalized), 0f);
                //rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, target_velocity, 0.1f);
                rigidbody.AddForce(delta.normalized * force_scale);
            }
                
            _relativePosition = transform.position - Body.transform.position;
        }
        else
        {
            transform.position = Body.transform.position;
        }

        _wasHolding = isHolding;
        // if holding both hands
    }
}
