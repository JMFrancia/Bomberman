using System;
using UnityEngine;

namespace DestroyIt
{
    public static class Check
    {
        public static bool HasNonTriggerCollider(GameObject go)
        {
            if (DestructionManager.Instance == null) return false;

            Collider[] colls = go.GetComponentsInChildren<Collider>();
            bool colliderFound = false;
            foreach (Collider coll in colls)
            {
                if (!coll.isTrigger)
                    colliderFound = true;
            }
            if (!colliderFound)
            {
                Debug.LogWarning("[" + go.name + "]: No collider found. Each destructible object should have at least one non-trigger collider, whether on it or one of its children.");
                return false;
            }
            return true;
        }

        public static bool IsDefaultLargeParticleAssigned()
        {
            if (DestructionManager.Instance == null) return false;

            if (DestructionManager.Instance.defaultLargeParticle == null)
            {
                Debug.LogError("DestructionManager: Default Large Particle is not assigned. You should assign a default large particle for simple destructible objects OVER 1m in size.");
                return false;
            }
            return true;
        }

        public static bool IsDefaultSmallParticleAssigned()
        {
            if (DestructionManager.Instance == null) return false;

            if (DestructionManager.Instance.defaultSmallParticle == null)
            {
                Debug.LogError("[DestructionManager] Default Small Particle is not assigned. You should assign a default small particle for simple destructible objects UNDER 1m in size.");
                return false;
            }
            return true;
        }

        public static bool LayerExists(string layerName, bool logMessage)
        {
            if (DestructionManager.Instance == null) return false;

            int layer = LayerMask.NameToLayer(layerName);
            if (layer == -1)
            {
                if (logMessage)
                    Debug.LogWarning(String.Format("[DestroyIt Core] Layer \"{0}\" does not exist. Please add a layer named \"{0}\" to your project.", layerName));
                return false;
            }
            return true;
        }
    }
}