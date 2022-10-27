using HarmonyLib;
using JetBrains.Annotations;
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
        }
    }
}