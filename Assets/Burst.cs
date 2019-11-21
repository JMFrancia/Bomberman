using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour
{
    [SerializeField] ParticleSystem shockwavePS;

    ParticleSystem ps;
    BoxCollider collider;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        collider = GetComponent<BoxCollider>();
    }

    public void Init(Vector3 dir, int spread, float speed) { 
        
    }

    private void Update()
    {
        if(!shockwavePS.IsAlive()) {
            collider.enabled = false;
        }
            
        if (!ps.IsAlive()) {
            Destroy(gameObject);
        }
    }
}
