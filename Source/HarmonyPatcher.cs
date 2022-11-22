using HarmonyLib;
using JetBrains.Annotations;
using System;
using Verse;

namespace BetterGeneGraphicsFramework
{
    [StaticConstructorOnStartup]
    [UsedImplicitly]
    public class HarmonyPatcher
    {
        static HarmonyPatcher()
        {
            var harmony = new Harmony("Telardo.BetterGeneGraphicsFramework");
            harmony.PatchAll();
            harmony.Patch(AccessTools.Method(typeof(Pawn), nameof(Pawn.PostApplyDamage)),
                postfix: new HarmonyMethod(typeof(HarmonyPatcher), nameof(PostApplyDamage)));
            harmony.Patch(AccessTools.Method(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.Notify_HediffChanged)),
                postfix: new HarmonyMethod(typeof(HarmonyPatcher), nameof(HediffChangedPostfix)));
        }

        
        private static void PostApplyDamage(Pawn __instance,DamageInfo dinfo, float totalDamageDealt)
        {
            
            if(__instance.RaceProps.Humanlike)
                __instance?.Drawer?.renderer?.graphics?.SetAllGraphicsDirty();
        }
        private static void HediffChangedPostfix(Hediff hediff)
        {
            Pawn pawn = hediff?.pawn;
            if(pawn?.RaceProps?.Humanlike??false)
                pawn?.Drawer?.renderer?.graphics?.SetAllGraphicsDirty();
        }
    }
}