using Aki.Reflection.Patching;
using Comfort.Common;
using EFT;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace VisceralRagdolls
{
    public class GameStartedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(GameWorld).GetMethod(nameof(GameWorld.OnGameStarted));
        }

        [PatchPostfix]

        private static void Postfix(GameWorld __instance)
        {
            var tarkovApplication = (TarkovApplication)Singleton<ClientApplication<ISession>>.Instance;
            var currentRaidSettings = (RaidSettings)typeof(TarkovApplication).GetField("_raidSettings", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tarkovApplication);
            var GrassLayers = UnityEngine.GameObject.FindObjectsOfType<GameObject>().Where(go => go.layer == LayerMask.NameToLayer("Grass"));
            var FoliageLayers = UnityEngine.GameObject.FindObjectsOfType<GameObject>().Where(go => go.layer == LayerMask.NameToLayer("Foliage"));
            var TerrainAIObject = GameObject.Find("TerrainsAI");


            if (GrassLayers != null)
            {

                if (currentRaidSettings.SelectedLocation.Name == "Streets of Tarkov")
                {
                    foreach (GameObject gameObject in GrassLayers)
                    {
                        gameObject.layer = LayerMask.NameToLayer("PlayerSpiritAura");
                        //gameObject.SetActive(false);
                    }
                }
                else
                {
                    foreach (GameObject gameObject in GrassLayers)
                    {
                        //gameObject.layer = LayerMask.NameToLayer("PlayerSpiritAura");
                        gameObject.SetActive(false);
                    }
                }

            }

            if (FoliageLayers != null)
            {
                foreach (GameObject gameObject in FoliageLayers)
                {
                    gameObject.layer = LayerMask.NameToLayer("PlayerSpiritAura");
                    //gameObject.SetActive(false);
                }
            }

            if (TerrainAIObject != null)
            {
                if (TerrainAIObject.activeSelf)
                {
                    TerrainAIObject.SetActive(false);
                }
            }
        }
    }
}
