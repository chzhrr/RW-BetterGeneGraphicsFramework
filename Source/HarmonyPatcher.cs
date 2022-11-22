using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace BetterGeneGraphicsFramework
{
    [StaticConstructorOnStartup]
    [UsedImplicitly]
    public class HarmonyPatcher
    {
        public static GeneGraphicsHelper helper;

        static HarmonyPatcher()
        {
            var harmony = new Harmony("Telardo.BetterGeneGraphicsFramework");
            harmony.PatchAll();
            // hediff graphics functionality
            harmony.Patch(AccessTools.Method(typeof(Pawn), nameof(Pawn.PostApplyDamage)),
                postfix: new HarmonyMethod(typeof(HarmonyPatcher), nameof(PostApplyDamage)));
            harmony.Patch(AccessTools.Method(typeof(Hediff), nameof(Hediff.PostAdd)),
                postfix: new HarmonyMethod(typeof(HarmonyPatcher), nameof(RedrawWhenHediffPostAdd)));
            harmony.Patch(AccessTools.Method(typeof(Hediff), nameof(Hediff.PostRemoved)),
                postfix: new HarmonyMethod(typeof(HarmonyPatcher), nameof(RedrawWhenHediffPostRemove)));
            harmony.Patch(AccessTools.Method(typeof(Gene), nameof(Gene.PostAdd)),
                postfix: new HarmonyMethod(typeof(HarmonyPatcher), nameof(RegisterWhenGenePostAdd)));
            harmony.Patch(AccessTools.Method(typeof(Gene), nameof(Gene.PostRemove)),
                postfix: new HarmonyMethod(typeof(HarmonyPatcher), nameof(DeregisterWhenGenePostRemove)));
        }

        private static void RegisterWhenGenePostAdd(Gene __instance)
        {
            GraphicsWithAge graphicsWithAgeExt = __instance.def.GetModExtension<GraphicsWithAge>();
            if (graphicsWithAgeExt != null && !graphicsWithAgeExt.bodyPartExpressions.NullOrEmpty())
            {
                helper.AddPawn(__instance.pawn);
            }
        }

        private static void DeregisterWhenGenePostRemove(Gene __instance)
        {
            GraphicsWithAge graphicsWithAgeExt = __instance.def.GetModExtension<GraphicsWithAge>();
            if (graphicsWithAgeExt != null && !graphicsWithAgeExt.bodyPartExpressions.NullOrEmpty())
            {
                helper.RemovePawn(__instance.pawn);
            }
        }

        private static void PostApplyDamage(Pawn __instance)
        {
            RedrawWhenHediffChanged(__instance);
        }

        private static void RedrawWhenHediffPostAdd(Hediff __instance)
        {
            RedrawWhenHediffChanged(__instance?.pawn);
        }

        private static void RedrawWhenHediffPostRemove(Hediff __instance)
        {
            RedrawWhenHediffChanged(__instance?.pawn);
        }

        private static void RedrawWhenHediffChanged(Pawn pawn)
        {
            if (helper.ShouldRedrawWhenHediffChange(pawn))
            {
                pawn.Drawer.renderer.graphics.SetGeneGraphicsDirty();
            }
        }
    }
}