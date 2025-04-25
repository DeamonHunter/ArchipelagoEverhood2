using ArchipelagoEverhood;
using ArchipelagoEverhood.Archipelago;
using ArchipelagoEverhood.Util;
using MelonLoader;
using TMPro;
using UnityEngine;

[assembly: MelonInfo(typeof(ArchipelagoEverhoodMod), "Archipelago Everhood 2", "0.1.0", "DeamonHunter")]
[assembly: MelonGame("Foreign Gnomes", "Everhood 2")]
[assembly: MelonPriority(100)]

namespace ArchipelagoEverhood
{
    public class ArchipelagoEverhoodMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();

            AssetHelpers.Assembly = MelonAssembly;
            Globals.Logging = new ArchLogger();
            Globals.LoginHandler = new ArchipelagoLogin(Info.Version, Info.Version);


            //Sets up the textures for ap assets
            TMP_Settings.defaultSpriteAsset.spriteSheet = AssetHelpers.OverwriteTextureWithTexture("ArchipelagoEverhood.Assets.APIcons.png",
                (Texture2D)TMP_Settings.defaultSpriteAsset.spriteSheet, 128, 0, 64, 16);

            TMP_Settings.defaultSpriteAsset.material.mainTexture = TMP_Settings.defaultSpriteAsset.spriteSheet;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Input.GetKey(KeyCode.J))
            {
                var camera = Object.FindObjectOfType<Main_TopdownRoot>().PixelPerfectCamera;
                camera.assetsPPU += 1;
            }
            else if (Input.GetKey(KeyCode.K))
            {
                var camera = Object.FindObjectOfType<Main_TopdownRoot>().PixelPerfectCamera;
                camera.assetsPPU -= 1;
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            //This game loads scenes from other mods. Don't want to conflict with one named "MenuRoot" for some reason.
            if (buildIndex == 1)
                Globals.LoginHandler.MainMenuLoaded(buildIndex);
        }
    }
}