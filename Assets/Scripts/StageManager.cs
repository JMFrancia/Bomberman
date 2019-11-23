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
        if(instance != null) {
            Destroy(gameObject);
        }

        instance = this;

        Bounds b = transform.GetChild(0).GetComponent<Renderer>().bounds;
        width = b.size.x;
        length = b.size.z;
    }

    public Vector3 GetClosestGridCenter(Vector3 input) {
        return new Vector3(RoundToGridUnit(input.x), input.y, RoundToGridUnit(input.z));
    }

    float RoundToFloat(float input, float target) {
        float quotient = input / target;
        if(quotient - Mathf.Floor(quotient) < .5) {
            return Mathf.Floor(quotient) * target;
        } else {
            return Mathf.Ceil(quotient) * target;
        }
    }

    float RoundToGridUnit(float input) {
        return RoundToFloat(input, GRID_UNIT);
    }
}
