using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public GameObject Character;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
#if false
        BodyPhysics bodyPhysics = Character.GetComponent<BodyPhysics>();

        Vector3 targetPos = Character.transform.position;

        SlingShot slingShot = Character.GetComponent<SlingShot>();
        if (slingShot != null)
        {
            if (slingShot.playerIsTouched)
            {
                Vector3 slingForce = slingShot.transform.position - slingShot.SlingVisualizePoint.transform.position;
                targetPos += slingForce;
            }
        }

        bool isFlying = !bodyPhysics.IsGrabbing[0] && !bodyPhysics.IsGrabbing[1];

        if (!isFlying)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Character.transform.rotation, 0.1f);
        }
#endif
#if true
        BodyPhysics bodyPhysics = Character.GetComponent<BodyPhysics>();

        Vector3 targetPos = Character.transform.position;

        SlingShot slingShot = Character.GetComponent<SlingShot>();
        if (slingShot != null)
        {
            if (slingShot.playerIsTouched)
            {
                Vector3 slingForce = slingShot.transform.position - slingShot.SlingVisualizePoint.transform.position;
                targetPos += 0.3f*slingForce;
            }
        }

        bool isFlying = !bodyPhysics.IsGrabbing[0] && !bodyPhysics.IsGrabbing[1];

        transform.position = Vector3.Lerp(transform.position, targetPos, 0.3f);

        float rotation = bodyPhysics.GetRotationAtPoint(transform.position);
        transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * rotation, new Vector3(0, 1, 0));

        //transform.rotation = Quaternion.Slerp(transform.rotation, Character.transform.rotation, 0.3f);
#endif
    }
}
