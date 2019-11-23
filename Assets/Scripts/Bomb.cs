using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float fuseTime = 2f;
    [SerializeField] int explosionSpread = 1;
    [SerializeField] float explosionSpreadSpeed = .2f;
    [SerializeField] Vector3 explosionStartAdjustment = new Vector3(0f, -1f, 0f);

    Coroutine fuseRoutine;

    private void Start()
    {
        fuseRoutine = StartCoroutine(Set(fuseTime));
    }

    IEnumerator Set(float fuse) {
        yield return new WaitForSeconds(fuse);
        Explode();
    }

    void Explode()
    {
        GameObject exp = Instantiate(explosionPrefab, transform.position + explosionStartAdjustment, Quaternion.identity);
        exp.GetComponent<Explosion>().Init(explosionSpread, explosionSpreadSpeed);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == GlobalConstants.TagNames.BURST) {
            StopCoroutine(fuseRoutine);
            Explode();
        }
    }
}
