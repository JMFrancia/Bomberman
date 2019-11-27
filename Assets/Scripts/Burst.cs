using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour
{
    [SerializeField] ParticleSystem shockwavePS;

    ParticleSystem ps;
    BoxCollider bCollider;
    AudioSource aSource;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        bCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(ps.particleCount == 0) {
            bCollider.enabled = false;
        }
            
        if (!ps.IsAlive()) {
            //Destroy(gameObject);
        }
    }
}
