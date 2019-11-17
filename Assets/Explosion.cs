using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] GameObject burstPrefab;

    bool initiated = false;

    Vector3[] dirs = new Vector3[4] { 
        new Vector3(1f, 0f, 0f),
        new Vector3(0f, 0f, 1f),
        new Vector3(-1f, 0f, 0f),
        new Vector3(0f, 0f, -1f),
    };

    public void Init(int spread, float speed) {
        if (!initiated)
        {
            StartCoroutine(Explode(spread, speed));
            initiated = true;
        }
    }

    IEnumerator Explode(int spread, float speed) {
        Instantiate(burstPrefab, transform.position, Quaternion.identity);

        for(int n = 0; n < spread; n++) {
            yield return new WaitForSeconds(speed);
            for(int k = 0; k < dirs.Length; k++) {
                Vector3 pos = transform.position + dirs[k] * (n + 1) * StageManager.GRID_UNIT;
                Instantiate(burstPrefab, pos, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }
}
