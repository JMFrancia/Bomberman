﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomberman : MonoBehaviour
{
    [Serializable]
    public enum PlayerInput { 
        PLAYER1,
        PLAYER2
    }

    [SerializeField] PlayerInput inputSource;
    [SerializeField] int bombCapacity = 1;
    [SerializeField] int power = 1;
    [SerializeField] float speed = 10f;

    [SerializeField] float speedPickupIncrease = 1f;
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] int maxPower = 8;

    [SerializeField] GameObject bombPrefab;

    [SerializeField] AudioClip bombDropSFX;

    AudioClip pickupSFX;
    AudioSource audioSource;
    Animator animator;
    Rigidbody rb;
    GameObject lastBomb;

    static HashSet<int> activeBombIDs = new HashSet<int>();
    float bombDist = -1f;
    bool dead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        EventManager.StartListening(EventName.BOMB_EXPLODED, OnBombExploded);
        pickupSFX = Resources.Load<AudioClip>("SFX/Pickup SFX");
    }

    private void Update()
    {
        CheckBombDrop();
    }

    void CheckMovement() {
        if (dead)
            return;

        float zAxis = Input.GetAxis(inputSource == PlayerInput.PLAYER1 ? GlobalConstants.InputAxisNames.P1_VERTICAL : GlobalConstants.InputAxisNames.P2_VERTICAL);
        float xAxis = Input.GetAxis(inputSource == PlayerInput.PLAYER1 ? GlobalConstants.InputAxisNames.P1_HORIZONTAL : GlobalConstants.InputAxisNames.P2_HORIZONTAL);

        Vector3 delta = new Vector3(xAxis, 0f, zAxis) * speed;
        rb.velocity = delta;

        animator.SetFloat(Animator.StringToHash("Speed_f"), Mathf.InverseLerp(0f, speed, delta.magnitude));

        if(Mathf.Approximately(Vector3.Magnitude(delta), 0f)) {
            rb.angularVelocity = Vector3.zero;
        } else {
            transform.LookAt(transform.position + delta);
            CheckActiveBombs();
        }
    }

    void CheckActiveBombs() { 
        if(lastBomb != null && lastBomb.activeInHierarchy && Vector3.Distance(lastBomb.transform.position, transform.position) > 3f) { 
            lastBomb.layer = LayerMask.NameToLayer("Default");
            lastBomb = null;
        }
    }

    void CheckBombDrop() { 
        if(!dead && Input.GetKeyDown(inputSource == PlayerInput.PLAYER1 ? KeyCode.RightShift : KeyCode.Space)) {
            if (activeBombIDs.Count >= bombCapacity)
                return;

            audioSource.PlayOneShot(bombDropSFX);
            Vector3 pos = StageManager.instance.GetClosestGridCenter(transform.position);
            lastBomb = DestroyIt.ObjectPool.Instance.Spawn(bombPrefab, new Vector3(pos.x, 1.5f, pos.z), Quaternion.identity);
            lastBomb.layer = LayerMask.NameToLayer("TransBomb");
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
                Die();
                break;
            case GlobalConstants.TagNames.PICKUP:
                audioSource.PlayOneShot(pickupSFX);
                CollectPickup(other.GetComponent<Pickup>().Type);
                Destroy(other.gameObject);
                break;
            default:
                break;
        }
    }

    void Die() {
        if (dead)
            return;
        EventManager.TriggerEvent(EventName.PLAYER_DIED, gameObject.name);
        dead = true;
        GetComponentInChildren<DestroyIt.Destructible>().currentHitPoints = 0;
    }

    void CollectPickup(Pickup.PickupType type) { 
        switch(type) {
            case Pickup.PickupType.BOMB:
                bombCapacity++;
                break;
            case Pickup.PickupType.POWER:
                power = Mathf.Min(power + 1, maxPower);
                break;
            case Pickup.PickupType.SPEED:
                speed = Mathf.Min(speed + speedPickupIncrease, maxSpeed);
                break;
        }
    }

    void OnBombExploded(int bombID) { 
        if(activeBombIDs.Contains(bombID)) {
            activeBombIDs.Remove(bombID);
        }
    }
}
