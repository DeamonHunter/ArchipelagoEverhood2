using System;
using System.Collections.Generic;
using System.Reflection;
using ArchipelagoEverhood.Data;
using ArchipelagoEverhood.Util;
using HarmonyLib;
using UnityEngine;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(nameof(ShootProjectileFromNoteEventCommand), "Awake")]
    public static class ShootProjectileCommandAwakePatch
    {
#region ColorDefinitions

        private static Dictionary<ProjectileColor, Color> _colors = new()
        {
            { ProjectileColor.Blue, new Color(0.3632f, 0.6425f, 1, 1) },
            { ProjectileColor.Red, new Color(1f, 0.3647f, 0.3647f, 1) },
            { ProjectileColor.Green, new Color(0.4706f, 1, 0.3647f, 1) },
            { ProjectileColor.Yellow, new Color(0.9576f, 1, 0.3647f, 1) },
            { ProjectileColor.Brown, new Color(0.7358f, 0.2272f, 0f, 1) },
            { ProjectileColor.Purple, new Color(0.9047f, 0.3647f, 1, 1) },
            { ProjectileColor.Orange, new Color(1f, 0.64f, 0.3647f, 1) },
        };

        private static Dictionary<ProjectileColor, Color[]> _energyColors = new()
        {
            { ProjectileColor.Blue, new[] { new Color(0f, 0.7049f, 1, 1), new Color(0f, 0.3077011f, 1, 1) } },
            { ProjectileColor.Red, new[] { new Color(1f, 0f, 0.05809641f, 1), new Color(1f, 0f, 0.1321745f, 1) } },
            { ProjectileColor.Green, new[] { new Color(0.006026864f, 1, 0f, 1), new Color(0f, 1, 0.01352072f, 1) } },
            { ProjectileColor.Yellow, new[] { new Color(1f, 0.9118239f, 0, 1) } },
            { ProjectileColor.Brown, new[] { new Color(0.4339623f, 0.1009927f, 0, 1) } },
            { ProjectileColor.Purple, new[] { new Color(1f, 0f, 0.9142747f, 1) } },
            { ProjectileColor.Orange, new[] { new Color(1f, 0.5039347f, 0f, 1) } },
        };

        private static Dictionary<ProjectileColor, Color> _lightColors = new()
        {
            { ProjectileColor.Blue, new Color(0f, 0.610426f, 0.972549f, 1) },
            { ProjectileColor.Red, new Color(0.972549f, 0f, 0.06522933f, 1) },
            { ProjectileColor.Green, new Color(0f, 0.972549f, 0.008270937f, 1) },
            { ProjectileColor.Yellow, new Color(0.972549f, 0.8453369f, 0f, 1) },
            { ProjectileColor.Brown, new Color(0.7358f, 0.2272f, 0f, 1) },
            { ProjectileColor.Purple, new Color(0.972549f, 0f, 0.9034228f, 1) },
            { ProjectileColor.Orange, new Color(1f, 0.4745701f, 0f, 1) },
        };

        private static Dictionary<ProjectileColor, Color> _iconColors = new()
        {
            { ProjectileColor.Blue, new Color(0f, 0.7283535f, 1f, 0.8f) },
            { ProjectileColor.Red, new Color(1f, 0.3702772f, 0.3443396f, 0.7372549f) },
            { ProjectileColor.Green, new Color(0f, 1, 0.005622149f, 0.8f) },
            { ProjectileColor.Yellow, new Color(1f, 0.9910696f, 0.3443396f, 0.8f) },
            { ProjectileColor.Brown, new Color(0.7924528f, 0.2770341f, 0.08597367f, 0.8f) },
            { ProjectileColor.Purple, new Color(1f, 0.2783019f, 0.9971699f, 0.8f) },
            { ProjectileColor.Orange, new Color(1f, 0.6212669f, 0.004716992f, 0.8f) },
        };

        private static Dictionary<ProjectileColor, Color[]> _mirrorColors = new()
        {
            { ProjectileColor.Blue, new[] { new Color(0f, 0.6163464f, 1, 1), new Color(0f, 0.4591708f, 1, 1) } },
            { ProjectileColor.Red, new[] { new Color(1f, 0f, 0.01863098f, 1), new Color(1f, 0f, 0.119318f, 1) } },
            { ProjectileColor.Green, new[] { new Color(0f, 1f, 0.003575325f, 1), new Color(0f, 1f, 0.01039314f, 1) } },
            { ProjectileColor.Yellow, new[] { new Color(1f, 0.7873625f, 0f, 1) } },
            { ProjectileColor.Brown, new[] { new Color(1f, 0.2788269f, 0f, 1) } },
            { ProjectileColor.Purple, new[] { new Color(0.9873223f, 0f, 1f, 1) } },
            { ProjectileColor.Orange, new[] { new Color(1f, 0.405365f, 0f, 1) } },
        };

        private static Dictionary<ProjectileColor, Color> _wallColors = new()
        {
            { ProjectileColor.Blue, new Color(0f, 0.6075771f, 1f, 1f) },
            { ProjectileColor.Red, new Color(1f, 0f, 0.01919127f, 1f) },
            { ProjectileColor.Green, new Color(0.04850841f, 1, 0f, 1f) },
            { ProjectileColor.Yellow, new Color(1f, 0.7334804f, 0f, 1f) },
            { ProjectileColor.Brown, new Color(0.735849f, 0.2272279f, 0f, 1f) },
            { ProjectileColor.Purple, new Color(0.972549f, 0f, 0.9034228f, 1f) },
            { ProjectileColor.Orange, new Color(0.972549f, 0.3844193f, 0f, 1f) },
        };

#endregion

        public static void Prefix(ShootProjectileFromNoteEventCommand __instance, ref GameObject ___projectilPrefab, ref GameObject ___projectilPrefabBlue,
            ref GameObject ___projectilPrefabGreen, ref GameObject ___projectilPrefabRed, out GameObject[]? __state)
        {
            __state = null;
            if (!Globals.SessionHandler.LoggedIn || Globals.EverhoodOverrides.Settings == null || !Globals.EverhoodOverrides.Settings.ColorSanity)
                return;
            try
            {
                //Prevent permanent damage 
                __state = new[] { ___projectilPrefab, ___projectilPrefabBlue, ___projectilPrefabGreen, ___projectilPrefabRed };

                if (___projectilPrefab)
                {
                    ___projectilPrefab = GameObject.Instantiate(___projectilPrefab);
                    ___projectilPrefab.SetActive(false);
                }

                if (___projectilPrefabBlue)
                {
                    ___projectilPrefabBlue = GameObject.Instantiate(___projectilPrefabBlue);
                    ___projectilPrefabBlue.SetActive(false);
                }

                if (___projectilPrefabGreen)
                {
                    ___projectilPrefabGreen = GameObject.Instantiate(___projectilPrefabGreen);
                    ___projectilPrefabGreen.SetActive(false);
                }

                if (___projectilPrefabRed)
                {
                    ___projectilPrefabRed = GameObject.Instantiate(___projectilPrefabRed);
                    ___projectilPrefabRed.SetActive(false);
                }


                if (BattleOverrides(__instance, ___projectilPrefab, ___projectilPrefabBlue, ___projectilPrefabGreen, ___projectilPrefabRed))
                    return;

                AdjustPrefab(___projectilPrefab);
                AdjustPrefab(___projectilPrefabBlue);
                AdjustPrefab(___projectilPrefabGreen);
                AdjustPrefab(___projectilPrefabRed);
            }
            catch (Exception e)
            {
                Globals.Logging.Error("ColorSanity", e);
            }
        }

        public static void PostFix(ref GameObject ___projectilPrefab, ref GameObject ___projectilPrefabBlue,
            ref GameObject ___projectilPrefabGreen, ref GameObject ___projectilPrefabRed, GameObject[]? __state)
        {
            if (__state == null)
                return;

            ___projectilPrefab = __state[0];
            ___projectilPrefabBlue = __state[1];
            ___projectilPrefabGreen = __state[2];
            ___projectilPrefabRed = __state[3];
        }

        private static bool BattleOverrides(ShootProjectileFromNoteEventCommand __instance, GameObject projectilePrefab, GameObject projectilePrefabBlue,
            GameObject projectilePrefabGreen, GameObject projectilePrefabRed)
        {
            switch (__instance.gameObject.scene.name)
            {
                case "Tutorial2-Battle":
                    typeof(ShootProjectileFromNoteEventCommand).GetField("projectilPrefabBlue", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, projectilePrefabRed);
                    typeof(ShootProjectileFromNoteEventCommand).GetField("projectilPrefabRed", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, projectilePrefabGreen);
                    typeof(ShootProjectileFromNoteEventCommand).GetField("projectilPrefabGreen", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, projectilePrefabBlue);
                    break;
                case "Capsicum-Battle":
                    if (Globals.EverhoodOverrides.Settings!.SoulColor == SoulColor.Red && !projectilePrefab.name.Contains("Power"))
                    {
                        AdjustToColor(projectilePrefab, ProjectileColor.Red);
                        return true;
                    }

                    break;
                case "JuiceMaster-Battle":
                    if (Globals.EverhoodOverrides.Settings!.SoulColor == SoulColor.Red)
                    {
                        var comp = projectilePrefab.GetComponent<NoteProjectileColorData>();
                        if (comp && comp.projectileColor == ProjectileColor.Blue)
                        {
                            AdjustToColor(projectilePrefab, ProjectileColor.Red);
                            return true;
                        }
                    }

                    break;
            }

            return false;
        }

        private static void AdjustPrefab(GameObject prefab)
        {
            if (!prefab)
                return;

            var projectile = prefab.GetComponent<NoteProjectileColorData>();
            if (!projectile || projectile.projectileColor == ProjectileColor.Any || projectile.projectileColor == ProjectileColor.Black)
                return;

            var flag = 1 << (int)(projectile.projectileColor);
            if ((Globals.EverhoodOverrides.ColorSanityMask & flag) != 0)
                return;

            var originalColor = projectile.projectileColor;
            projectile.projectileColor = ProjectileColor.Black;
            if (EverhoodHelpers.TryGetChildWithName("Sabre", prefab, out var saber))
            {
                var particleSystem = saber.GetComponent<ParticleSystem>();
                var main = particleSystem.main;
                main.startColor = new ParticleSystem.MinMaxGradient(_colors[originalColor]);

                var renderer = saber.GetComponent<ParticleSystemRenderer>();
                renderer.material.shader = Shader.Find("Mobile/Particles/Alpha Blended");
                renderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                renderer.material.mainTexture = Globals.BlackHalfMoonTexture;
            }

            if (EverhoodHelpers.TryGetChildWithName("Wall", prefab, out var wall))
            {
                var particleSystem = wall.GetComponent<ParticleSystem>();
                var main = particleSystem.main;
                main.startColor = new ParticleSystem.MinMaxGradient(Color.black);

                var renderer = wall.GetComponent<ParticleSystemRenderer>();
                renderer.material.shader = Shader.Find("Mobile/Particles/Alpha Blended");
                renderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            }

            if (EverhoodHelpers.TryGetChildWithName("Skull", prefab, out var skull))
            {
                var particleSystem = skull.GetComponent<ParticleSystem>();
                var colorOverLifetime = particleSystem.colorOverLifetime;
                var color = colorOverLifetime.color;
                var keys = color.gradient;
                for (var i = 0; i < keys.colorKeys.Length; i++)
                    keys.colorKeys[i] = new GradientColorKey(new Color(0, 0, 0), keys.colorKeys[i].time);
                color.gradient = keys;
                colorOverLifetime.color = color;

                var renderer = skull.GetComponent<ParticleSystemRenderer>();
                renderer.material.shader = Shader.Find("Mobile/Particles/Alpha Blended");
                renderer.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            }
        }

        private static void AdjustToColor(GameObject prefab, ProjectileColor setColor)
        {
            if (!prefab)
                return;

            var projectile = prefab.GetComponent<NoteProjectileColorData>();
            if (!projectile || projectile.projectileColor == ProjectileColor.Any || projectile.projectileColor == ProjectileColor.Black)
                return;

            projectile.projectileColor = setColor;
            if (EverhoodHelpers.TryGetChildWithName("Energy", prefab, out var energy) && energy.gameObject.activeSelf)
            {
                var particleSystem = energy.GetComponent<ParticleSystem>();
                var colorOverLifetime = particleSystem.colorOverLifetime;
                var color = colorOverLifetime.color;
                var keys = color.gradient;
                if (keys.colorKeys.Length >= 2)
                {
                    var colors = _energyColors[setColor];
                    var copy = new GradientColorKey[colors.Length + 1];
                    copy[0] = keys.colorKeys[0];

                    copy[1] = new GradientColorKey(colors[0], keys.colorKeys[1].time);
                    if (colors.Length > 1)
                        copy[2] = new GradientColorKey(colors[1], 1);
                    keys.colorKeys = copy;
                    color.gradient = keys;
                }

                colorOverLifetime.color = color;
            }

            if (EverhoodHelpers.TryGetChildWithName("Light", prefab, out var light) && light.gameObject.activeSelf)
            {
                var particleSystem = light.GetComponent<ParticleSystem>();
                var main = particleSystem.main;
                main.startColor = new ParticleSystem.MinMaxGradient(_lightColors[setColor]);
            }

            if (EverhoodHelpers.TryGetChildWithName("SabreMirror", prefab, out var saberMirror) && saberMirror.gameObject.activeSelf)
            {
                var particleSystem = saberMirror.GetComponent<ParticleSystem>();
                var colorOverLifetime = particleSystem.colorOverLifetime;
                var color = colorOverLifetime.color;
                var keys = color.gradient;
                if (keys.colorKeys.Length >= 2)
                {
                    var colors = _mirrorColors[setColor];
                    var copy = new GradientColorKey[colors.Length + 1];
                    copy[0] = keys.colorKeys[0];

                    copy[1] = new GradientColorKey(colors[0], keys.colorKeys[1].time);
                    if (colors.Length > 1)
                        copy[2] = new GradientColorKey(colors[1], 1);
                    keys.colorKeys = copy;
                    color.gradient = keys;
                }

                colorOverLifetime.color = color;
            }

            if (EverhoodHelpers.TryGetActiveChildWithName("Energy-Icon", prefab, out var icon) && icon.gameObject.activeSelf)
            {
                var particleSystem = icon.GetComponent<ParticleSystem>();
                var main = particleSystem.main;
                main.startColor = new ParticleSystem.MinMaxGradient(_iconColors[setColor]);

                var sheet = particleSystem.textureSheetAnimation;
                for (var i = sheet.spriteCount - 1; i >= 0; i--)
                    sheet.RemoveSprite(0);

                //Todo: If we support non-red colors this will need to change
                var sprite = Sprite.Create(AssetHelpers.LoadTexture("ArchipelagoEverhood.Assets.RedMarker.png"), Rect.MinMaxRect(0, 0, 22, 28), new Vector2(0.5f, 0.5f));
                sheet.AddSprite(sprite);
            }

            if (EverhoodHelpers.TryGetChildWithName("WallShine", prefab, out var wall) && wall.gameObject.activeSelf)
            {
                var particleSystem = wall.GetComponent<ParticleSystem>();
                var main = particleSystem.main;
                main.startColor = _wallColors[setColor];
            }
        }
    }

    public class ColorSanityPatches
    {
        //GameplayPlayerAttack.DoAbsorb - Prevent the notes being taken

        //ShootProjectileFromNoteEventCommand.Awake - Adjust the coloring and data
    }
}