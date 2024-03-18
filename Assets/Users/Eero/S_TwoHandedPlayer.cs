using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_TwoHandedPlayer : MonoBehaviour
{
    public S_Hand2 HandL;
    public S_Hand2 HandR;

    private bool _wasHolding = false;
    private Vector3 _holdBeginHandInputAvg = Vector3.zero;
    private Vector3 _holdBeginPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Rigidbody rigidbody = GetComponent<Rigidbody>();

        Debug.Log($"HandL.IsHolding() {HandL.IsHolding()}");

        bool isHolding = HandL.IsHolding() && HandR.IsHolding();
        if (isHolding)
        {
            Vector3 axes_l = HandL.GetAxes();
            Vector3 axes_r = HandR.GetAxes();

            Vector3 handInputAvg = 0.5f * (axes_l + axes_r);

            if (!_wasHolding)
            {
                _holdBeginHandInputAvg = handInputAvg;
                _holdBeginPosition = transform.position;
            }

            Vector3 bodyTargetPosition = _holdBeginPosition - (handInputAvg - _holdBeginHandInputAvg);
            Vector3 force = 10f * (bodyTargetPosition - transform.position);
            rigidbody.AddForce(force);
            //transform.position = 

            rigidbody.drag = 10f;
            // axes_l

        }
        else {
            rigidbody.drag = 0f;
        }

        _wasHolding = isHolding;

        rigidbody.AddForce(new Vector3(0, -0.5f, 0));
    }
}
