using HarmonyLib;
using RimWorld;
using Verse;

namespace BetterGeneGraphicsFramework
{
    /// <summary>
    /// Support mod "FemaleBB BodyType Support".
    /// </summary>
    public static class FemaleBBBodyTypeSupport
    {
        private static bool _checked;

        private static BodyTypeDef femaleBodyTypeDef;

        public static BodyTypeDef FemaleBodyTypeDef
        {
            get
            {
                if (!_checked)
                {
                    if (AccessTools.TypeByName("BBBodySupport.BBBodyTypeSupportDefof") != null)
                        femaleBodyTypeDef = DefDatabase<BodyTypeDef>.GetNamed("FemaleBB");
                    _checked = true;
                }
                return femaleBodyTypeDef;
            }
        }
    }
}