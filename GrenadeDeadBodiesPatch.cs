using Aki.Reflection.Patching;
using EFT;
using System.Reflection;
using UnityEngine;

namespace VisceralRagdolls
{
    public class GrenadeDeadBodiesPatch : ModulePatch
    {
        static float _force = 190f;


        protected override MethodBase GetTargetMethod()
        {
            return typeof(Grenade).GetMethod(nameof(Grenade.Explosion), BindingFlags.Public | BindingFlags.Static);
        }

        [PatchPostfix]
        static void Postfix(IExplosiveItem grenadeItem, Vector3 grenadePosition)
        {
            var radius = UnityEngine.Random.Range(grenadeItem.MinExplosionDistance, grenadeItem.MaxExplosionDistance);
            var hits = Physics.SphereCastAll(new Ray(grenadePosition, Vector3.up), radius, grenadeItem.MaxExplosionDistance, GClass2781.HitMask);
            foreach (var hit in hits)
            {
                var rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(grenadeItem.GetStrength * 0.5f * _force, grenadePosition, radius);
                }
            }
        }
    }
}