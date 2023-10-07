using Aki.Reflection.Patching;
using EFT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace VisceralRagdolls
{
    public class BodyPatch : ModulePatch
    {
        private static String[] TargetBones = { "calf", "foot", "toe", "spine2", "spine3", "forearm", "neck" };

        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player).GetMethod("CreateCorpse", BindingFlags.Public | BindingFlags.Instance, null, Array.Empty<Type>(), null);
        }

        [PatchPostfix]
        private static void Postfix(Player __instance)
        {
            if (VisceralEntry.Instance.BodyCollision.Value)
            {
                if (__instance.IsYourPlayer)
                {
                    return;
                }
                foreach (Transform child in EnumerateHierarchyCore(__instance.Transform.Original).Where(t => TargetBones.Any(u => t.name.ToLower().Contains(u))))
                {
                    child.gameObject.layer = 6;
                }
            }
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