using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float shiverTime = 1f;
    [SerializeField] float shiverSpeed = 1f;
    [SerializeField] float shiverAmp = .2f;
    [SerializeField] float fuseTime = 2f;
    public int explosionSpread = 1;
    [SerializeField] float explosionSpreadSpeed = .2f;
    [SerializeField] Vector3 explosionStartAdjustment = new Vector3(0f, -1f, 0f);

    Coroutine fuseRoutine;

    bool shiver = false;

    WaitForSeconds shiverWait;
    WaitForSeconds fuseWait;
    Vector3 originalPos;

    private void Awake()
    {
        shiverWait = new WaitForSeconds(shiverTime);
        fuseWait = new WaitForSeconds(fuseTime);
    }

    private void OnEnable()
    {
        originalPos = transform.position;
        fuseRoutine = StartCoroutine(Set(fuseTime));
    }

    private void Update()
    {
        if(shiver) {
            float newX = Mathf.Sin(Time.time * shiverSpeed) * shiverAmp;
            transform.position = originalPos + new Vector3(newX, 0f, 0f);
        }
    }

    IEnumerator Set(float fuse) {
        yield return shiverWait;
        shiver = true;
        yield return fuseWait;
        Explode();
    }

    void Explode()
    {
        GameObject exp = Instantiate(explosionPrefab, originalPos + explosionStartAdjustment, Quaternion.identity);
        exp.GetComponent<Explosion>().Init(explosionSpread, explosionSpreadSpeed);
        EventManager.TriggerEvent(EventName.BOMB_EXPLODED, gameObject.GetInstanceID());
        shiver = false;
        DestroyIt.ObjectPool.Instance.PoolObject(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == GlobalConstants.TagNames.BURST) {
            StopCoroutine(fuseRoutine);
            Explode();
        }
    }
}
