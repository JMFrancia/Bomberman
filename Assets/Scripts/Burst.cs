using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour
{
    [SerializeField] ParticleSystem shockwavePS;

    static int activeBurstSounds = 0;
    static Queue<AudioSource> audioSources = new Queue<AudioSource>();

    ParticleSystem ps;
    BoxCollider bCollider;
    AudioSource aSource;

    int maxBurstSounds = 2;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        bCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        LimitBurstSFX();
        bCollider.enabled = true;
    }

    void LimitBurstSFX() {
        audioSources.Enqueue(GetComponent<AudioSource>());
        activeBurstSounds++;

        while (activeBurstSounds > maxBurstSounds)
        {
            AudioSource AS = audioSources.Dequeue();
            if (AS != null)
            {
                AS.Stop();
                activeBurstSounds = Mathf.Max(activeBurstSounds - 1, 0);
            }
        }
    }

    private void Update()
    {
        if(ps.particleCount == 0) {
            bCollider.enabled = false;
        }
            
        if (!ps.IsAlive()) {
            DestroyIt.ObjectPool.Instance.PoolObject(gameObject);
        }
    }

    private void OnDisable()
    {
        activeBurstSounds = Mathf.Max(activeBurstSounds - 1, 0);
    }
}
