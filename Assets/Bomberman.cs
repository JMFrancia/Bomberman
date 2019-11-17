using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomberman : MonoBehaviour
{
    public float speed = 20f;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckInput();
    }

    void CheckInput() {
        float zAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");


        Vector3 delta = new Vector3(xAxis, 0f, zAxis) * speed;

        if(delta == Vector3.zero) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        } else {
            rb.AddForce(delta, ForceMode.VelocityChange);
        }

        rb.velocity = delta;
        //rb.AddForce(delta, ForceMode.VelocityChange);

        //transform.position += delta;

        //if(delta == Vector3.zero)
    }

}
