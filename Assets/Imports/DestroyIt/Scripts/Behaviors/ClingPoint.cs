﻿using UnityEngine;

namespace DestroyIt
{
    public class ClingPoint : MonoBehaviour
    {
        public int chanceToCling = 75;          // 0 = 0% chance, 1 = 100% chance.
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position - (this.transform.forward * 0.025f), .01f);
            Gizmos.DrawRay(this.transform.position - (this.transform.forward * 0.025f), this.transform.forward * 0.075f);
        }
    }
}