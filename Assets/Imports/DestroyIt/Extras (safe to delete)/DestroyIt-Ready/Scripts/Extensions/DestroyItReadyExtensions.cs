using DestroyItReady;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace DestroyIt
{
    public static class DestroyItReadyExtensions
    {
        public static void DestructiblesToStubs(this IList<GameObject> objs)
        {
            if (objs == null || objs.Count == 0) return;

            foreach (GameObject obj in objs)
            {
                if (obj == null) continue;

                if (PrefabUtility.IsPartOfPrefabAsset(obj))
                {
                    string path = AssetDatabase.GetAssetPath(obj);
                    GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);
                    try
                    {
                        // STEP 1 - DESTRUCTIBLE
                        prefabRoot.RemoveExistingComponents<Destructible, DestructibleStub>();
                        prefabRoot.ConvertFrom<Destructible>();
                        prefabRoot.RemoveConvertedComponents<Destructible>();

                        // STEP 2 - TAGIT
                        prefabRoot.RemoveExistingComponents<TagIt, TagItStub>();
                        prefabRoot.ConvertFrom<TagIt>();
                        prefabRoot.RemoveConvertedComponents<TagIt>();

                        // STEP 3 - HIT EFFECTS
                        prefabRoot.RemoveExistingComponents<HitEffects, HitEffectsStub>();
                        prefabRoot.ConvertFrom<HitEffects>();
                        prefabRoot.RemoveConvertedComponents<HitEffects>();

                        PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
                    }
                    finally
                    {
                        PrefabUtility.UnloadPrefabContents(prefabRoot);
                    }
                }
                else
                {
                    // STEP 1 - DESTRUCTIBLE
                    obj.RemoveExistingComponents<Destructible, DestructibleStub>();
                    obj.ConvertFrom<Destructible>();
                    obj.RemoveConvertedComponents<Destructible>();

                    // STEP 2 - TAGIT
                    obj.RemoveExistingComponents<TagIt, TagItStub>();
                    obj.ConvertFrom<TagIt>();
                    obj.RemoveConvertedComponents<TagIt>();

                    // STEP 3 - HIT EFFECTS
                    obj.RemoveExistingComponents<HitEffects, HitEffectsStub>();
                    obj.ConvertFrom<HitEffects>();
                    obj.RemoveConvertedComponents<HitEffects>();
                }
            }
        }

        public static void StubsToDestructibles(this IList<GameObject> objs)
        {
            if (objs == null || objs.Count == 0) return;

            foreach (GameObject obj in objs)
            {
                if (obj == null) continue;

                if (PrefabUtility.IsPartOfPrefabAsset(obj))
                {
                    string path = AssetDatabase.GetAssetPath(obj);
                    GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);
                    try
                    {
                        // STEP 1 - DESTRUCTIBLE
                        prefabRoot.RemoveExistingComponents<DestructibleStub, Destructible>();
                        prefabRoot.ConvertFrom<DestructibleStub>();
                        prefabRoot.RemoveConvertedComponents<DestructibleStub>();

                        // STEP 2 - TAGIT
                        prefabRoot.RemoveExistingComponents<TagItStub, TagIt>();
                        prefabRoot.ConvertFrom<TagItStub>();
                        prefabRoot.RemoveConvertedComponents<TagItStub>();

                        // STEP 3 - HIT EFFECTS
                        prefabRoot.RemoveExistingComponents<HitEffectsStub, HitEffects>();
                        prefabRoot.ConvertFrom<HitEffectsStub>();
                        prefabRoot.RemoveConvertedComponents<HitEffectsStub>();

                        PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
                    }
                    finally
                    {
                        PrefabUtility.UnloadPrefabContents(prefabRoot);
                    }
                }
                else
                {
                    // STEP 1 - DESTRUCTIBLE
                    obj.RemoveExistingComponents<DestructibleStub, Destructible>();
                    obj.ConvertFrom<DestructibleStub>();
                    obj.RemoveConvertedComponents<DestructibleStub>();

                    // STEP 2 - TAGIT
                    obj.RemoveExistingComponents<TagItStub, TagIt>();
                    obj.ConvertFrom<TagItStub>();
                    obj.RemoveConvertedComponents<TagItStub>();

                    // STEP 3 - HIT EFFECTS
                    obj.RemoveExistingComponents<HitEffectsStub, HitEffects>();
                    obj.ConvertFrom<HitEffectsStub>();
                    obj.RemoveConvertedComponents<HitEffectsStub>();
                }
            }
        }

        private static List<DestroyItReady.DamageEffect> ToDestroyItReady(this List<DamageEffect> damageEffects)
        {
            if (damageEffects == null || damageEffects.Count <= 0) return null;

            var retVal = new List<DestroyItReady.DamageEffect>();
            foreach (var source in damageEffects)
            {
                var dest = new DestroyItReady.DamageEffect
                {
                    TriggeredAt = source.TriggeredAt,
                    Offset = source.Offset,
                    Rotation = source.Rotation,
                    Effect = source.Prefab,
                    HasStarted = source.HasStarted,
                    UseDependency = source.HasTagDependency,
                    TagDependency = (DestroyItReady.Tag) source.TagDependency,
                    UnparentOnDestroy = source.UnparentOnDestroy,
                    StopEmittingOnDestroy = source.StopEmittingOnDestroy
                };
                retVal.Add(dest);
            }
            return retVal;
        }

        private static List<DestroyItReady.MaterialMapping> ToDestroyItReady(this List<MaterialMapping> replaceMaterials)
        {
            if (replaceMaterials == null || replaceMaterials.Count <= 0) return null;

            var retVal = new List<DestroyItReady.MaterialMapping>();
            foreach (var source in replaceMaterials)
            {
                var dest = new DestroyItReady.MaterialMapping()
                {
                    SourceMaterial = source.SourceMaterial,
                    ReplacementMaterial = source.ReplacementMaterial
                };
                retVal.Add(dest);
            }
            return retVal;
        }

        private static List<DamageEffect> ToDestructible(this List<DestroyItReady.DamageEffect> damageEffects)
        {
            if (damageEffects == null || damageEffects.Count <= 0) return null;

            var retVal = new List<DamageEffect>();
            foreach (var source in damageEffects)
            {
                var dest = new DamageEffect
                {
                    TriggeredAt = source.TriggeredAt,
                    Offset = source.Offset,
                    Rotation = source.Rotation,
                    Prefab = source.Effect,
                    HasStarted = source.HasStarted,
                    HasTagDependency = source.UseDependency,
                    TagDependency = (DestroyIt.Tag)source.TagDependency,
                    UnparentOnDestroy = source.UnparentOnDestroy,
                    StopEmittingOnDestroy = source.StopEmittingOnDestroy
                };
                retVal.Add(dest);
            }
            return retVal;
        }

        private static List<MaterialMapping> ToDestructible(this List<DestroyItReady.MaterialMapping> replaceMaterials)
        {
            if (replaceMaterials == null || replaceMaterials.Count <= 0) return null;

            var retVal = new List<MaterialMapping>();
            foreach (var source in replaceMaterials)
            {
                var dest = new MaterialMapping()
                {
                    SourceMaterial = source.SourceMaterial,
                    ReplacementMaterial = source.ReplacementMaterial
                };
                retVal.Add(dest);
            }
            return retVal;
        }

        private static List<DestroyItReady.DamageLevel> ToDestroyItReady(this List<DamageLevel> damageLevels)
        {
            if (damageLevels == null || damageLevels.Count <= 0) return null;

            var retVal = new List<DestroyItReady.DamageLevel>();
            foreach (var source in damageLevels)
            {
                var dest = new DestroyItReady.DamageLevel
                {
                    maxHitPoints = source.maxHitPoints,
                    minHitPoints = source.minHitPoints,
                    healthPercent = source.healthPercent,
                    hasError = source.hasError,
                    visibleDamageLevel = source.visibleDamageLevel
                };
                retVal.Add(dest);
            }
            return retVal;
        }

        private static List<DamageLevel> ToDestructible(this List<DestroyItReady.DamageLevel> damageLevels)
        {
            if (damageLevels == null || damageLevels.Count <= 0) return null;

            var retVal = new List<DamageLevel>();
            foreach (var source in damageLevels)
            {
                var dest = new DamageLevel
                {
                    maxHitPoints = source.maxHitPoints,
                    minHitPoints = source.minHitPoints,
                    healthPercent = source.healthPercent,
                    hasError = source.hasError,
                    visibleDamageLevel = source.visibleDamageLevel
                };
                retVal.Add(dest);
            }
            return retVal;
        }

        private static List<DestroyItReady.HitEffect> ToDestroyItReady(this List<HitEffect> hitEffects)
        {
            if (hitEffects == null || hitEffects.Count <= 0) return null;

            var retVal = new List<DestroyItReady.HitEffect>();
            foreach (var source in hitEffects)
            {
                var dest = new DestroyItReady.HitEffect()
                {
                    hitBy = (DestroyItReady.HitBy)source.hitBy,
                    effect = source.effect
                };
                retVal.Add(dest);
            }
            return retVal;
        }

        private static List<HitEffect> ToHitEffects(this List<DestroyItReady.HitEffect> hitEffects)
        {
            if (hitEffects == null || hitEffects.Count <= 0) return null;

            var retVal = new List<HitEffect>();
            foreach (var source in hitEffects)
            {
                var dest = new HitEffect()
                {
                    hitBy = (HitBy)source.hitBy,
                    effect = source.effect
                };
                retVal.Add(dest);
            }
            return retVal;
        }

        /// <summary>For the specified gameobject, search for components of type TSource and remove any existing component of type TDestination at the same level.
        /// This is in preparation for adding the TDestination component later.</summary>
        private static void RemoveExistingComponents<TSource, TDestination>(this GameObject obj) where TSource : MonoBehaviour where TDestination : MonoBehaviour
        {
            // Find all source components
            TSource[] sourceComponents = obj.GetComponentsInChildren<TSource>();

            if (sourceComponents == null || sourceComponents.Length == 0) return;

            for (int i = 0; i < sourceComponents.Length; i++)
            {
                // Find any existing destination component at the same level
                TDestination destComponent = sourceComponents[i].gameObject.GetComponent<TDestination>();
                if (destComponent != null)
                    Object.DestroyImmediate(destComponent, true);
            }
        }

        private static void RemoveConvertedComponents<TComponent>(this GameObject obj) where TComponent : MonoBehaviour
        {
            TComponent[] components = obj.GetComponentsInChildren<TComponent>();

            if (components == null || components.Length == 0) return;

            for (int i = 0; i < components.Length; i++)
                Object.DestroyImmediate(components[i], true);
        }

        private static void ConvertFrom<TSource>(this GameObject obj) where TSource : MonoBehaviour
        {
            TSource[] sourceComponents = obj.GetComponentsInChildren<TSource>();

            if (sourceComponents == null || sourceComponents.Length == 0) return;

            for (int i = 0; i < sourceComponents.Length; i++)
            {
                if (sourceComponents[i] == null) continue;

                // DestructibleStub => Destructible
                if (sourceComponents[i].GetType() == typeof(DestructibleStub))
                {
                    DestructibleStub stub = sourceComponents[i] as DestructibleStub;
                    stub.ToScript();
                    continue;
                }
                // Destructible => DestructibleStub
                if (sourceComponents[i].GetType() == typeof(Destructible))
                {
                    Destructible script = sourceComponents[i] as Destructible;
                    script.ToStub();
                    continue;
                }

                // TagItStub => TagIt
                if (sourceComponents[i].GetType() == typeof(TagItStub))
                {
                    TagItStub stub = sourceComponents[i] as TagItStub;
                    stub.ToScript();
                    continue;
                }
                // TagIt => TagItStub
                if (sourceComponents[i].GetType() == typeof(TagIt))
                {
                    TagIt script = sourceComponents[i] as TagIt;
                    script.ToStub();
                    continue;
                }

                // HitEffectsStub => HitEffects
                if (sourceComponents[i].GetType() == typeof(HitEffectsStub))
                {
                    HitEffectsStub stub = sourceComponents[i] as HitEffectsStub;
                    stub.ToScript();
                    continue;
                }
                // HitEffects => HitEffectsStub
                if (sourceComponents[i].GetType() == typeof(HitEffects))
                {
                    HitEffects script = sourceComponents[i] as HitEffects;
                    script.ToStub();
                    continue;
                }

                //Object.DestroyImmediate(sourceComponents[i], true);
            }
        }

        /// <summary>Converts a DestructibleStub script to a Destructible script.</summary>
        private static void ToScript(this DestructibleStub stub)
        {
            Destructible dest = stub.gameObject.AddComponent<Destructible>();

            // Assign values from the Destructible script to the stub.
            dest.totalHitPoints = stub.totalHitPoints;
            dest.currentHitPoints = stub.currentHitPoints;
            dest.damageLevels = stub.damageLevels.ToDestructible();
            dest.velocityReduction = stub.velocityReduction;
            dest.ignoreCollisionsUnder = stub.ignoreCollisionsUnder;
            dest.destroyedPrefab = stub.destroyedPrefab;
            dest.destroyedPrefabParent = stub.destroyedPrefabParent;
            dest.fallbackParticle = stub.fallbackParticle;
            dest.useFallbackParticle = stub.useFallbackParticle;
            dest.fallbackParticleMaterial = stub.fallbackParticleMaterial;
            dest.damageEffects = stub.damageEffects.ToDestructible();
            dest.unparentOnDestroy = stub.unparentOnDestroy;
            dest.disableKinematicOnUparentedChildren = stub.disableKinematicOnUparentedChildren;
            dest.replaceMaterials = stub.replaceMaterials.ToDestructible();
            dest.canBeDestroyed = stub.canBeDestroyed;
            dest.canBeRepaired = stub.canBeRepaired;
            dest.canBeObliterated = stub.canBeObliterated;
            dest.debrisToReParentByName = stub.debrisToReParentByName;
            dest.debrisToReParentIsKinematic = stub.debrisToReParentIsKinematic;
            dest.childrenToReParentByName = stub.childrenToReParentByName;
            dest.destructibleGroupId = stub.destructibleGroupId;
            dest.isDebrisChipAway = stub.isDebrisChipAway;
            dest.chipAwayDebrisAngularDrag = stub.chipAwayDebrisAngularDrag;
            dest.chipAwayDebrisDrag = stub.chipAwayDebrisDrag;
            dest.chipAwayDebrisMass = stub.chipAwayDebrisMass;
            dest.autoPoolDestroyedPrefab = stub.autoPoolDestroyedPrefab;
            dest.centerPointOverride = stub.centerPointOverride;
            dest.rotationOverride = stub.rotationOverride;
            dest.sinkWhenDestroyed = stub.sinkWhenDestroyed;
        }

        private static void ToStub(this Destructible destObj)
        {
            DestructibleStub stub = destObj.gameObject.AddComponent<DestructibleStub>();

            // Assign values from the Destructible script to the stub.
            stub.totalHitPoints = destObj.totalHitPoints;
            stub.currentHitPoints = destObj.currentHitPoints;
            stub.damageLevels = destObj.damageLevels.ToDestroyItReady();
            stub.velocityReduction = destObj.velocityReduction;
            stub.ignoreCollisionsUnder = destObj.ignoreCollisionsUnder;
            stub.destroyedPrefab = destObj.destroyedPrefab;
            stub.destroyedPrefabParent = destObj.destroyedPrefabParent;
            stub.fallbackParticle = destObj.fallbackParticle;
            stub.useFallbackParticle = destObj.useFallbackParticle;
            stub.fallbackParticleMaterial = destObj.fallbackParticleMaterial;
            stub.damageEffects = destObj.damageEffects.ToDestroyItReady();
            stub.unparentOnDestroy = destObj.unparentOnDestroy;
            stub.disableKinematicOnUparentedChildren = destObj.disableKinematicOnUparentedChildren;
            stub.replaceMaterials = destObj.replaceMaterials.ToDestroyItReady();
            stub.canBeDestroyed = destObj.canBeDestroyed;
            stub.canBeRepaired = destObj.canBeRepaired;
            stub.canBeObliterated = destObj.canBeObliterated;
            stub.debrisToReParentByName = destObj.debrisToReParentByName;
            stub.debrisToReParentIsKinematic = destObj.debrisToReParentIsKinematic;
            stub.childrenToReParentByName = destObj.childrenToReParentByName;
            stub.destructibleGroupId = destObj.destructibleGroupId;
            stub.isDebrisChipAway = destObj.isDebrisChipAway;
            stub.chipAwayDebrisAngularDrag = destObj.chipAwayDebrisAngularDrag;
            stub.chipAwayDebrisDrag = destObj.chipAwayDebrisDrag;
            stub.chipAwayDebrisMass = destObj.chipAwayDebrisMass;
            stub.autoPoolDestroyedPrefab = destObj.autoPoolDestroyedPrefab;
            stub.centerPointOverride = destObj.centerPointOverride;
            stub.centerPointOverride = destObj.rotationOverride;
            stub.sinkWhenDestroyed = destObj.sinkWhenDestroyed;
        }

        private static void ToScript(this TagItStub stub)
        {
            TagIt tagIt = stub.gameObject.AddComponent<TagIt>();

            // Assign values from the TagIt script to the stub.
            tagIt.tags = stub.tags.Select(x => (Tag)x).ToList();
        }

        private static void ToStub(this TagIt tagIt)
        {
            TagItStub stub = tagIt.gameObject.AddComponent<TagItStub>();

            // Assign values from the TagIt script to the stub.
            stub.tags = tagIt.tags.Select(x => (DestroyItReady.Tag)x).ToList();
        }

        private static void ToScript(this HitEffectsStub stub)
        {
            HitEffects hitEffects = stub.gameObject.AddComponent<HitEffects>();

            // Assign values from the HitEffects script to the stub.
            hitEffects.effects = stub.effects.ToHitEffects();
        }

        private static void ToStub(this HitEffects obj)
        {
            HitEffectsStub stub = obj.gameObject.AddComponent<HitEffectsStub>();

            // Assign values from the HitEffects script to the stub.
            stub.effects = obj.effects.ToDestroyItReady();
        }
    }
}
