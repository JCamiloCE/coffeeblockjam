using CoffeeBlockJam.Grid;
using Enums;

namespace CoffeeBlockJam.Trays
{
    public interface ITraySectionRules
    {
        public (ETypeTray, float) GetTrayTypeWithRotation(NeighborsCellData neighborsCellData);
    }
}