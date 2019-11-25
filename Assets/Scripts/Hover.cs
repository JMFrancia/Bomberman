using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField] float amplitude = 1f;
    [SerializeField] float speed = 1f;

    Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.position;
    }

    void Update()
    {
        transform.position = originalPos + new Vector3(0f, Mathf.Sin(Time.time * speed) * amplitude, 0f);
    }
}
