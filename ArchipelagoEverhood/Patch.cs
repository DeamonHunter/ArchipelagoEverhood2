using System;
using System.Reflection;
using ForeignGnomes.VisualScripting;
using Fungus;
using HarmonyLib;
using RocaVS;
using UnityEngine;
using SetOperator = Fungus.SetOperator;

namespace ArchipelagoEverhood
{
    [HarmonyPatch(typeof(DamageEnemy), "Run")]
    public static class LoadingSceneOnEnablePatch
    {
        private static void Postfix(DamageEnemy __instance)
        {
            return;

            var main_GameplayRoot = (Main_GameplayRoot)(typeof(DamageEnemy).GetField("main_GameplayRoot", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance));
            var _absorbedNotesCount = (int)(typeof(DamageEnemy).GetField("_absorbedNotesCount", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance));
            var _weaponAttack = (WeaponAttacks)(typeof(DamageEnemy).GetField("_weaponAttack", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance));
            var _colorAbsorbed = (ProjectileColor)(typeof(DamageEnemy).GetField("_colorAbsorbed", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance));
            var _serviceRoot = (ServicesRoot)(typeof(DamageEnemy).GetField("_serviceRoot", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance));
            var multiplier = (float)(typeof(DamageEnemy).GetField("multiplier", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance));

            GameplayEnemy gameplayEnemy = main_GameplayRoot.ActiveBattleRoot.GameplayEnemy;
            int num1 = gameplayEnemy.IsStrengthColor(_colorAbsorbed) ? 1 : 0;
            bool flag = gameplayEnemy.IsWeaknessColor(_colorAbsorbed);
            float num2 = 1f;
            if (num1 != 0)
                num2 -= _serviceRoot.WeaponSkills.StrengthModifier * gameplayEnemy.StrengthOffsetModifier;
            if (flag)
                num2 += _serviceRoot.WeaponSkills.WeaknessModifier * gameplayEnemy.WeaknessOffsetModifier;
            WeaponAttack weaponAttack = _serviceRoot.WeaponSkills.GetWeaponAttack(_weaponAttack);
            double num3 = (double)_serviceRoot.WeaponDamage.GetFinalDamage(weaponAttack, _absorbedNotesCount) * (double)num2 * (double)weaponAttack.DamageWeight * (double)multiplier;
            ArtifactModifier artifactModifier = _serviceRoot.Artifacts.GetArtifactModifier(_serviceRoot.GameData.GeneralData.equipedArtifact);
            int level = _serviceRoot.GameData.GeneralData.xpLevel_player.level;
            float num4 = InfinityEditorData.playerEnergyLevelModifier[level];
            GameplayPlayer activePlayer = main_GameplayRoot.ActivePlayer;
            int colorAbsorbed = (int)_colorAbsorbed;
            float colorBoost = artifactModifier.GetColorBoost(activePlayer, (ProjectileColor)colorAbsorbed);
            float colorBonusModifier = weaponAttack.GetColorBonusModifier(_colorAbsorbed);
            float num5 = 0.0f;
            WeaponLevel currentWeaponLevel = main_GameplayRoot.ActivePlayer.GameplayPlayerAttack.CurrentWeaponHolding.CurrentWeaponLevel;
            float num6 = currentWeaponLevel == null ? 0.0f : currentWeaponLevel.BaseDamageModifier;
            float num7 = currentWeaponLevel == null ? 0.0f : currentWeaponLevel.GetColorDamageModifier(_colorAbsorbed);
            double num8 = 1.0 + ((double)num4 + (double)colorBoost + (double)colorBonusModifier + (double)num5 + (double)num6 + (double)num7);
            float damage = (float)(num3 * num8);

            Globals.Logging.Msg(
                $"DAMAGE:\n NUM2 {num2} \n {weaponAttack.DamageWeight} \n {multiplier} \n NUM3 {num3}\n NUM4 {num4}\n NUM5 {num5} \n NUM6 {num6}\n NUM7 {num7}\n NUM8 {num8} \n DAMAGE {damage}");
        }
    }

    [HarmonyPatch(typeof(fvs_SetVariable), "DoSetOperation")]
    public static class fvs_SetVariablePatch
    {
        private static void Prefix(fvs_SetVariable __instance)
        {
            try
            {
                Globals.Logging.Msg("Set Operation Called. Context:" + __instance.Context());
            }
            catch (Exception e)
            {
                Globals.Logging.Error("DoSetOperation", e);
            }
        }
    }

    [HarmonyPatch(typeof(IntegerVariable), "Apply")]
    public static class IntegerVariablePatch
    {
        private static void Prefix(IntegerVariable __instance, SetOperator setOperator, int value)
        {
            try
            {
                if (VariableData.KnownVariable(__instance.Key))
                    return;

                Globals.Logging.Msg($"IntApply Key: {__instance.Key} Value: {__instance.Value}. Operation: {setOperator}, Addition: {value}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("IntegerVariable", e);
            }
        }

        private static void Postfix(IntegerVariable __instance)
        {
            try
            {
                if (VariableData.KnownVariable(__instance.Key))
                    return;

                Globals.Logging.Msg($"IntApply Outcome: {__instance.Value}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("IntegerVariable", e);
            }
        }
    }

    [HarmonyPatch(typeof(BooleanVariable), "Apply")]
    public static class BooleanVariablePatch
    {
        private static void Prefix(BooleanVariable __instance, SetOperator setOperator, bool value)
        {
            try
            {
                if (VariableData.KnownVariable(__instance.Key))
                    return;

                Globals.Logging.Msg($"BoolApply Key: {__instance.Key} Value: {__instance.Value}. Operation: {setOperator}, Addition: {value}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Apply", e);
            }
        }

        private static void Postfix(BooleanVariable __instance)
        {
            try
            {
                if (VariableData.KnownVariable(__instance.Key))
                    return;

                Globals.Logging.Msg($"BoolApply Outcome: {__instance.Value}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("BooleanVariable", e);
            }
        }
    }

    [HarmonyPatch(typeof(Vector2Variable), "Apply")]
    public static class Vector2VariablePatch
    {
        private static void Prefix(Vector2Variable __instance, SetOperator setOperator, Vector2 value)
        {
            try
            {
                if (VariableData.KnownVariable(__instance.Key))
                    return;

                Globals.Logging.Msg($"Vector2Apply Key: {__instance.Key} Value: {__instance.Value}. Operation: {setOperator}, Addition: {value}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Vector2Variable", e);
            }
        }

        private static void Postfix(Vector2Variable __instance)
        {
            try
            {
                if (VariableData.KnownVariable(__instance.Key))
                    return;
                Globals.Logging.Msg($"Vector2Apply Outcome: {__instance.Value}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Vector2Variable", e);
            }
        }
    }

    [HarmonyPatch(typeof(Vector3Variable), "Apply")]
    public static class Vector3VariablePatch
    {
        private static void Prefix(Vector3Variable __instance, SetOperator setOperator, Vector3 value)
        {
            try
            {
                if (VariableData.KnownVariable(__instance.Key))
                    return;

                Globals.Logging.Msg($"Vector3Apply Key: {__instance.Key} Value: {__instance.Value}. Operation: {setOperator}, Addition: {value}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Vector3Variable", e);
            }
        }

        private static void Postfix(Vector3Variable __instance)
        {
            try
            {
                if (VariableData.KnownVariable(__instance.Key))
                    return;

                Globals.Logging.Msg($"Vector3Apply Outcome: {__instance.Value}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Vector3Variable", e);
            }
        }
    }

    [HarmonyPatch(typeof(FloatVariable), "Apply")]
    public static class FloatVariablePatch
    {
        private static void Prefix(FloatVariable __instance, SetOperator setOperator, float value)
        {
            try
            {
                if (VariableData.KnownVariable(__instance.Key))
                    return;

                Globals.Logging.Msg($"FloatApply Key: {__instance.Key} Value: {__instance.Value}. Operation: {setOperator}, Addition: {value}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("FloatVariable", e);
            }
        }

        private static void Postfix(FloatVariable __instance)
        {
            try
            {
                if (VariableData.KnownVariable(__instance.Key))
                    return;

                Globals.Logging.Msg($"FloatApply Outcome: {__instance.Value}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("FloatVariable", e);
            }
        }
    }

    [HarmonyPatch(typeof(EverhoodVariableCondition), "EvaluateCondition")]
    public static class CheckPatch
    {
        private static void Prefix(EverhoodVariableCondition __instance, BooleanData ___booleanData, IntegerData ___integerData, StringData ___stringData)
        {
            try
            {
                if (__instance is IfGameplayPlayerEnergyAbsorbed)
                    return;

                var type = __instance.GetVariableType();
                if (type == typeof(int))
                    Globals.Logging.Msg($"Special If {__instance.GetType()}: {__instance.GetSummary()}\n{__instance.GetValue()} {__instance._CompareOperator} {___integerData.Value}");
                else if (type == typeof(bool))
                    Globals.Logging.Msg($"Special If {__instance.GetType()}: {__instance.GetSummary()}\n{__instance.GetValue()} {__instance._CompareOperator} {___booleanData.Value}");
                else if (type == typeof(string))
                    Globals.Logging.Msg($"Special If {__instance.GetType()}: {__instance.GetSummary()}\n{__instance.GetValue()} {__instance._CompareOperator} {___stringData.Value}");
                else
                    Globals.Logging.Msg($"Special If {__instance.GetType()}: Unknown type {type}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Failed during Variable Patch:", e);
            }
        }
    }

    [HarmonyPatch(typeof(Command), "OnEnter")]
    public static class CommandPatch
    {
        private static void Prefix(Command __instance)
        {
            try
            {
                Globals.Logging.Msg($"{__instance.GetSummary()}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Command", e);
            }
        }
    }
}