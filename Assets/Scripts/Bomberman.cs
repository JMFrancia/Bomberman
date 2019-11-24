using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomberman : MonoBehaviour
{
    public float speed = 20f;

    [SerializeField] GameObject bombPrefab;

    Rigidbody rb;

    GameObject activeBomb;
    float bombDist = -1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
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
            CheckActiveBombs();
        }
    }

    void CheckActiveBombs() { 
        if(activeBomb != null && Vector3.Distance(activeBomb.transform.position, transform.position) > 3f) { 
            activeBomb.layer = LayerMask.NameToLayer("Default");
            activeBomb = null;
        }
    }

    void CheckBombDrop() { 
        if(Input.GetKeyDown(KeyCode.Space)) {
            Vector3 pos = StageManager.instance.GetClosestGridCenter(transform.position);
            activeBomb = Instantiate(bombPrefab, new Vector3(pos.x, 1.5f, pos.z), Quaternion.identity);
            if(bombDist < 0f) {
                bombDist = activeBomb.GetComponent<Collider>().bounds.size.magnitude / 2;
            }
        }
    }


    private void FixedUpdate()
    {
        CheckMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag) {
            case GlobalConstants.TagNames.BURST:
                Debug.Log("OUCH!");
                break;
            case GlobalConstants.TagNames.PICKUP:
                CollectPickup(other.GetComponent<Pickup>().Type);
                Destroy(other.gameObject);
                break;
            default:
                break;
        }
    }

    void CollectPickup(Pickup.PickupType type) { 
        switch(type) {
            case Pickup.PickupType.BOMB:
                Debug.Log("Got a bomb");
                break;
            case Pickup.PickupType.POWER:
                Debug.Log("Got power");
                break;
            case Pickup.PickupType.SPEED:
                Debug.Log("Got speed");
                break;
        }
    }
}
