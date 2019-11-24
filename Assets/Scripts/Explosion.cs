using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] GameObject burstPrefab;
    [SerializeField] GameObject pickupPrefab;

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
        bool[] activeDirections = new bool[] {
            true,
            true,
            true,
            true
        };

        Instantiate(burstPrefab, transform.position, Quaternion.identity);

        for (int n = 0; n < spread; n++) {
            yield return new WaitForSeconds(speed);
            for(int k = 0; k < dirs.Length; k++) {
                if (!activeDirections[k])
                    continue;
                Vector3 pos = transform.position + dirs[k] * (n + 1) * StageManager.GRID_UNIT;
                if (Physics.CheckSphere(pos, .5f, LayerMask.GetMask("Wall")))
                {
                    activeDirections[k] = false;
                }
                else if (Physics.CheckSphere(pos, .5f, LayerMask.GetMask("DestructibleWall"))) {
                    activeDirections[k] = false;
                    Physics.OverlapSphere(pos, .5f, LayerMask.GetMask("DestructibleWall"))[0].GetComponent<DestroyIt.Destructible>().currentHitPoints = 0;
                    GeneratePickup(pos);
                }
                else
                {
                    Instantiate(burstPrefab, pos, Quaternion.identity);
                }
            }
        }

        Destroy(gameObject);
    }

    void GeneratePickup(Vector3 pos) {
        if(Random.value < .5f) {
            return;
        }

        Pickup pickup = Instantiate(pickupPrefab, pos, Quaternion.identity).GetComponent<Pickup>();
        switch(Random.Range(0, 3)) {
            case 0:
                pickup.Type = Pickup.PickupType.BOMB;
                break;
            case 1:
                pickup.Type = Pickup.PickupType.POWER;
                break;
            case 2:
                pickup.Type = Pickup.PickupType.SPEED;
                break;
        }

    }
}
