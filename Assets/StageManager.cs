using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public const float GRID_UNIT = 3f;

    public static StageManager instance;
    float width;
    float length;

    private void Awake()
    {
        instance = this;

        Bounds b = transform.GetChild(0).GetComponent<Renderer>().bounds;
        width = b.size.x;
        length = b.size.z;
    }

    public Vector3 GetClosestGridCenter(Vector3 input) {
        //Debug.Log(input + " --> " + new Vector3(RoundToGridUnit(input.x) + GRID_UNIT / 2, 0f, RoundToGridUnit(input.z) + GRID_UNIT / 2));
        return instance.transform.position - new Vector3(width / 2, 0f, length / 2) + new Vector3(RoundToGridUnit(input.x) + GRID_UNIT / 2, input.y, RoundToGridUnit(input.z) + GRID_UNIT / 2);
    }

    static float RoundToGridUnit(float input)
    {
        return Mathf.Round(input / GRID_UNIT) * GRID_UNIT;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int x = 0; x < 15; x++) { 
            for(int z = 0; z < 11; z++) {
                Vector3 pos = GetClosestGridCenter(new Vector3(x * GRID_UNIT, GRID_UNIT / 2, z * GRID_UNIT));
                Gizmos.DrawSphere(pos, GRID_UNIT / 2);
            }
        }
    }
}
