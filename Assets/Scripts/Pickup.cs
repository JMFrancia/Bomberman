using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class Pickup : MonoBehaviour
{
    [Serializable]
    public enum PickupType {
        BOMB,
        POWER,
        SPEED
    }

    public PickupType Type {
        get {
            return type;
        }
        set {
            SetType(value);
        }
    }

    //TODO : Refactor
    [SerializeField] PickupType type;
    [SerializeField] Renderer iconRenderer;
    [SerializeField] Material bombPickupMat;
    [SerializeField] Material powerPickupMat;
    [SerializeField] Material speedPickupMat;

    Dictionary<PickupType, Material> matDict;

    public void SetType(PickupType type) {
        if(matDict == null) {
            matDict = new Dictionary<PickupType, Material>()
            {
                {PickupType.BOMB, bombPickupMat},
                {PickupType.POWER, powerPickupMat},
                {PickupType.SPEED, speedPickupMat}
            };
        }

        this.type = type;
        iconRenderer.material = matDict[type];
    }

    private void OnValidate()
    {
        SetType(type);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == GlobalConstants.TagNames.BURST) {
            Destroy(gameObject);
        }
    }
} 
