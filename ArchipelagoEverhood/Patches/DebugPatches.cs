#if DEBUG
using System;
using ForeignGnomes.VisualScripting;
using Fungus;
using HarmonyLib;
using UnityEngine;
using SetOperator = Fungus.SetOperator;

namespace ArchipelagoEverhood
{
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

    [HarmonyPatch(typeof(Command), "Execute")]
    public static class CommandPatch
    {
        private static void Prefix(Command __instance)
        {
            try
            {
                Globals.Logging.Log($"Command {__instance.GetType()}", $"{__instance.GetSummary()}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error($"Command {__instance.GetType()}", e);
            }
        }
    }
}
#endif