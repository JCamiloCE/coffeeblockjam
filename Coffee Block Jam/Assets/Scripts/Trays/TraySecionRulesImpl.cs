using CoffeeBlockJam.Grid;
using Enums;

namespace CoffeeBlockJam.Trays
{
    public class TraySecionRulesImpl : ITraySectionRules
    {
        (ETypeTray, float) ITraySectionRules.GetTrayTypeWithRotation(NeighborsCellData neighborsCellData)
        {
            int numberOfWalls = neighborsCellData.GetNumberOfWalls;
            ETypeTray typeTray = GetTypeOfTrayByWalls(numberOfWalls);
            if (typeTray == ETypeTray.Invalid) 
            {
                typeTray = GetTypeOfTrayForTwoWalls(neighborsCellData, numberOfWalls);
            }
            return (typeTray, GetRotation(typeTray, neighborsCellData));
        }

        

        private ETypeTray GetTypeOfTrayByWalls(int numberOfWalls) 
        {
            if (numberOfWalls == 0) return ETypeTray.NoWalls;
            if (numberOfWalls == 1) return ETypeTray.OneWall;
            if (numberOfWalls == 3) return ETypeTray.ThreeWalls;
            if (numberOfWalls == 4) return ETypeTray.FourWall;

            return ETypeTray.Invalid;
        }

        private ETypeTray GetTypeOfTrayForTwoWalls(NeighborsCellData neighborsCellData, int numberOfWalls) 
        {
            if (numberOfWalls == 2) 
            {
                if(neighborsCellData.GetUpNeighbor == true && neighborsCellData.GetDownNeighbor == true) return ETypeTray.OppositeWall;
                if(neighborsCellData.GetLeftNeighbor == true && neighborsCellData.GetRightNeighbor == true) return ETypeTray.OppositeWall;
                return ETypeTray.TwoWalls;
            }

            return ETypeTray.Invalid;
        }

        private float GetRotation(ETypeTray typeTray, NeighborsCellData neighborsCellData) 
        {
            if (typeTray == ETypeTray.OneWall) return GetRotationForTrayOneWall(neighborsCellData);
            if (typeTray == ETypeTray.TwoWalls) return GetRotationForTrayTwoWall(neighborsCellData);
            if (typeTray == ETypeTray.ThreeWalls) return GetRotationForTrayThreeWall(neighborsCellData);
            if (typeTray == ETypeTray.OppositeWall) return GetRotationForOppositeWall(neighborsCellData);
            return 0f;
        }

        private float GetRotationForTrayOneWall(NeighborsCellData neighborsCellData) 
        {
            if (!neighborsCellData.GetLeftNeighbor) return 90f;
            if (!neighborsCellData.GetUpNeighbor) return 180f;
            if (!neighborsCellData.GetRightNeighbor) return 270f;
            return 0f;
        }

        private float GetRotationForTrayTwoWall(NeighborsCellData neighborsCellData)
        {
            if (!neighborsCellData.GetLeftNeighbor && !neighborsCellData.GetUpNeighbor) return 90f;
            if (!neighborsCellData.GetUpNeighbor && !neighborsCellData.GetRightNeighbor) return -180f;
            if (!neighborsCellData.GetRightNeighbor && !neighborsCellData.GetDownNeighbor) return 270f;
            return 0f;
        }

        private float GetRotationForTrayThreeWall(NeighborsCellData neighborsCellData)
        {
            if (neighborsCellData.GetRightNeighbor) return 90f;
            if (neighborsCellData.GetDownNeighbor) return 180f;
            if (neighborsCellData.GetLeftNeighbor) return 270f;
            return 0f;
        }

        private float GetRotationForOppositeWall(NeighborsCellData neighborsCellData)
        {
            if (neighborsCellData.GetUpNeighbor && neighborsCellData.GetDownNeighbor) return 90f;
            return 0f;
        }
    }
}