using Aki.Reflection.Patching;
using EFT;
using EFT.Ballistics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace VisceralRagdolls
{
    public class BodiesImpulsePatch : ModulePatch
    {
        private static Dictionary<String, Single> _dictionary =
            new Dictionary<String, Single> { { "12g", 150f }, { "762x51", 65f }, { "762x39", 45 }, { "9x39", 33 }, { "545x39", 35 }, { "9x18PM", 12 }, { "762x35", 60 }, { "556x45NATO", 30 }, { "127x55", 100 }, { "127x108", 1000 }, { "366TKM", 60 }, { "40x46", 200 }, { "26x75", 70 }, { "30x29", 350 }, { "762x54R", 95 }, { "86x70", 800 }, { "9x19PARA", 12 }, { "1143x23ACP", 12 }, { "Caliber9x21", 5 }, { "57x28", 40 }, { "23x75", 200 }, { "25x59mm", 180 }, { "12.7x99", 110 } };

        private static Dictionary<String, Single> _bonedictionary =
    new Dictionary<String, Single> { { "Base HumanSpine3", 0.8f }, { "Base HumanSpine2", 0.8f }, { "Base HumanSpine1", 0.8f }, { "Base HumanPelvis", 0.8f } };


        protected override MethodBase GetTargetMethod()
        {
            return typeof(BallisticsCalculator).GetMethod(nameof(BallisticsCalculator.Shoot),
                BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPostfix]
        private static void Postfix(Shot0 shot)
        {
            StaticManager.Instance.StartCoroutine(WatchShot(shot));
        }

        private static IEnumerator WatchShot(Shot0 shot)
        {
            while (!shot.IsShotFinished)
            {
                yield return null;
            }

            if (shot.HitCollider == null)
            {
                yield break;
            }

            if (!(shot.Ammo is BulletClass bulletClass))
            {
                yield break;
            }

            if (!_dictionary.TryGetValue(bulletClass.Caliber, out Single modifier))
            {
                yield break;
            }

            Rigidbody rb = shot.HitCollider.GetComponent<Rigidbody>();
            if (rb == null)
            {
                yield break;
            }

            modifier /= bulletClass.ProjectileCount > 0 ? bulletClass.ProjectileCount : 1;

            if (_bonedictionary.TryGetValue(shot.HitCollider.name, out Single bonemodifier) && bulletClass.Caliber != "12g")
            {
                modifier *= bonemodifier;
            }

            rb.AddForceAtPosition(shot.Direction * (modifier * 85f), shot.HitPoint);
        }

        private static IEnumerable<Transform> EnumerateHierarchyCore(Transform root)
        {
            Queue<Transform> transformQueue = new Queue<Transform>();
            transformQueue.Enqueue(root);

            while (transformQueue.Count > 0)
            {
                Transform parentTransform = transformQueue.Dequeue();

                if (!parentTransform)
                {
                    continue;
                }

                for (Int32 i = 0; i < parentTransform.childCount; i++)
                {
                    transformQueue.Enqueue(parentTransform.GetChild(i));
                }

                yield return parentTransform;
            }
        }
    }
}