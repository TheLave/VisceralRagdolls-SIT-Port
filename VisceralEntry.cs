using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using System;
using System.Linq;
using UnityEngine;

namespace VisceralRagdolls
{
    [BepInPlugin("com.servph.visceralbodies", "Visceral Bodies", "1.2.0")]
    public class VisceralEntry : BaseUnityPlugin
    {
        public Player LocalPlayer { get; private set; }
        public Player HideoutPlayer { get; private set; }
        public ConfigEntry<Boolean> BodyCollision { get; set; }
        public bool IsSoT { get; set; }

        public static VisceralEntry Instance { get; private set; }

        public void Awake()
        {
            EFTHardSettings.Instance.DEBUG_CORPSE_PHYSICS = true;
            Instance = this;
            new BodyPatch().Enable();
            new BodiesImpulsePatch().Enable();
            new GrenadeDeadBodiesPatch().Enable();
            new GameStartedPatch().Enable();
            this.BodyCollision = this.Config.Bind("", "Player Body Collision", true);
        }



        public void Update()
        {
            if (!Singleton<GameWorld>.Instantiated)
            {
                this.LocalPlayer = null;
                return;
            }

            EFTHardSettings.Instance.DEBUG_CORPSE_PHYSICS = true;

            if (EFTHardSettings.Instance.DEBUG_CORPSE_PHYSICS == false)
            {
                EFTHardSettings.Set("DEBUG_CORPSE_PHYSICS", "true");
            }

            GameWorld gameWorld = Singleton<GameWorld>.Instance;

            if (this.LocalPlayer == null && gameWorld.RegisteredPlayers.Count > 0)
            {
                this.LocalPlayer = (Player)gameWorld.RegisteredPlayers[0];
                return;
            }

            if (this.HideoutPlayer == null && gameWorld.RegisteredPlayers.Count > 0)
            {
                this.HideoutPlayer = (Player)gameWorld.RegisteredPlayers[0];
                return;
            }
        }
    }
}