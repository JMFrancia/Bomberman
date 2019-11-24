using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomberman : MonoBehaviour
{
    [SerializeField] int bombCapacity = 1;
    [SerializeField] int power = 1;
    [SerializeField] float speed = 10f;

    [SerializeField] float speedPickupIncrease = 5f;

    [SerializeField] GameObject bombPrefab;

    Rigidbody rb;

    HashSet<int> activeBombIDs = new HashSet<int>();

    GameObject lastBomb;
    float bombDist = -1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        EventManager.StartListening(EventName.BOMB_EXPLODED, OnBombExploded);
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
        if(lastBomb != null && Vector3.Distance(lastBomb.transform.position, transform.position) > 3f) { 
            lastBomb.layer = LayerMask.NameToLayer("Default");
            lastBomb = null;
        }
    }

    void CheckBombDrop() { 
        if(Input.GetKeyDown(KeyCode.Space)) {
            if (activeBombIDs.Count >= bombCapacity)
                return;

            Vector3 pos = StageManager.instance.GetClosestGridCenter(transform.position);
            lastBomb = Instantiate(bombPrefab, new Vector3(pos.x, 1.5f, pos.z), Quaternion.identity);
            lastBomb.GetComponent<Bomb>().explosionSpread = power;
            if(bombDist < 0f) {
                bombDist = lastBomb.GetComponent<Collider>().bounds.size.magnitude / 2;
            }
            activeBombIDs.Add(lastBomb.GetInstanceID());
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
                bombCapacity++;
                break;
            case Pickup.PickupType.POWER:
                Debug.Log("Got power");
                power++;
                break;
            case Pickup.PickupType.SPEED:
                Debug.Log("Got speed");
                speed += speedPickupIncrease;
                break;
        }
    }

    void OnBombExploded(int bombID) { 
        if(activeBombIDs.Contains(bombID)) {
            activeBombIDs.Remove(bombID);
        }
    }
}
