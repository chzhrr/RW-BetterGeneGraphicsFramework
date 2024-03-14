using Verse;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnassignedField.Global

namespace BetterGeneGraphicsFramework
{
    public class WithBodyTypeOffset : DefModExtension
    {
        public bool disableBodyTypeScaleForAdult;
        public bool disableBodyTypeScaleForChildren;
        public BodyTypeOffset bodyTypeOffsetNorth;
        public BodyTypeOffset bodyTypeOffsetSouth;
        public BodyTypeOffset bodyTypeOffsetEast;
    }
}