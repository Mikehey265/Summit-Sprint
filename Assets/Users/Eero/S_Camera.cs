using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Camera : MonoBehaviour
{

    public GameObject Body;

    // Start is called before the first frame update
    void Start()
    {
        //my_object.Data
    }

    // Update is called once per frame
    void Update()
    {
        //if (Body.transform.position.y > transform.position.y)
        //{
        //}
        transform.position = new Vector3(transform.position.x, Body.transform.position.y, transform.position.z);
    }
}
