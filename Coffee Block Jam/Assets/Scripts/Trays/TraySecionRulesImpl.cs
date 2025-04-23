using CoffeeBlockJam.Grid;
using Enums;

namespace CoffeeBlockJam.Trays
{
    public class TraySecionRulesImpl : ITraySectionRules
    {
        (ETypeTray, float) ITraySectionRules.GetTrayTypeWithRotation(NeighborsCellData neighborsCellData)
        {
            int numberOfWalls = GetNumberOfWalls(neighborsCellData);
            ETypeTray typeTray = GetTypeOfTrayByWalls(numberOfWalls);
            if (typeTray == ETypeTray.Invalid) 
            {
                typeTray = GetTypeOfTrayForTwoWalls(neighborsCellData, numberOfWalls);
            }
            return (typeTray, GetRotation(typeTray, neighborsCellData));
        }

        private int GetNumberOfWalls(NeighborsCellData neighborsCellData) 
        {
            int counterWalls = 0;
            counterWalls = counterWalls + (neighborsCellData.up ? 0 : 1);
            counterWalls = counterWalls + (neighborsCellData.down ? 0 : 1);
            counterWalls = counterWalls + (neighborsCellData.left ? 0 : 1);
            counterWalls = counterWalls + (neighborsCellData.right ? 0 : 1);
            return counterWalls;
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
                if(neighborsCellData.up == true && neighborsCellData.down == true) return ETypeTray.OppositeWall;
                if(neighborsCellData.left == true && neighborsCellData.right == true) return ETypeTray.OppositeWall;
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
            if (!neighborsCellData.left) return 90f;
            if (!neighborsCellData.up) return 180f;
            if (!neighborsCellData.right) return 270f;
            return 0f;
        }

        private float GetRotationForTrayTwoWall(NeighborsCellData neighborsCellData)
        {
            if (!neighborsCellData.left && !neighborsCellData.up) return 90f;
            if (!neighborsCellData.up && !neighborsCellData.right) return -180f;
            if (!neighborsCellData.right && !neighborsCellData.down) return 270f;
            return 0f;
        }

        private float GetRotationForTrayThreeWall(NeighborsCellData neighborsCellData)
        {
            if (neighborsCellData.right) return 90f;
            if (neighborsCellData.down) return 180f;
            if (neighborsCellData.left ) return 270f;
            return 0f;
        }

        private float GetRotationForOppositeWall(NeighborsCellData neighborsCellData)
        {
            if (neighborsCellData.up && neighborsCellData.down) return 90f;
            return 0f;
        }
    }
}