using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomberman : MonoBehaviour
{
    public float speed = 20f;

    [SerializeField] GameObject bombPrefab;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //CheckMovement();
        CheckBombDrop();
    }

    void CheckMovement() {
        float zAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");

        Vector3 delta = new Vector3(xAxis, 0f, zAxis) * speed;
        rb.velocity = delta;
        if(Mathf.Approximately(Vector3.Magnitude(delta), 0f)) {
            rb.angularVelocity = Vector3.zero;
        } else {
            transform.LookAt(transform.position + delta);
        }
    }

    void CheckBombDrop() { 
        if(Input.GetKeyDown(KeyCode.Space)) {
            Instantiate(bombPrefab, new Vector3(transform.position.x, 1.5f, transform.position.z), Quaternion.identity);
        }
    }


    private void FixedUpdate()
    {
        CheckMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag) {
            case "Burst":
                Debug.Log("OUCH!");
                break;
        }
    }
}
