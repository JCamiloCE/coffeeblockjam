using Unity.VisualScripting;
using UnityEngine;

namespace CoffeeBlockJam.Grid
{
    public class NeighborsCellData 
    {
        private bool _upNeighbor;
        private bool _downNeighbor;
        private bool _leftNeighbor;
        private bool _rightNeighbor;
        private int _walls;

        public bool GetUpNeighbor => _upNeighbor;
        public bool GetDownNeighbor => _downNeighbor;
        public bool GetLeftNeighbor => _leftNeighbor;
        public bool GetRightNeighbor => _rightNeighbor;
        public int GetNumberOfWalls => _walls;

        public NeighborsCellData (GridDataJson loadedData, CellDataJson currentCell)
        {
            FillNeighborsData(loadedData, currentCell);
            SetWalls();
        }

        private void FillNeighborsData(GridDataJson loadedData, CellDataJson currentCell) 
        {
            Vector2Int neighborPos = Vector2Int.zero;
            Vector2Int currentPosition = currentCell.position;
            int currentId = currentCell.id;
            Color currentColor = currentCell.color;//This can be improve to use the Enum
            //check _upNeighbor
            neighborPos = new Vector2Int(currentPosition.x, currentPosition.y - 1);
            _upNeighbor = CheckNeighbor(loadedData, neighborPos, currentId, currentColor);
            //check _downNeighbor
            neighborPos = new Vector2Int(currentPosition.x, currentPosition.y + 1);
            _downNeighbor = CheckNeighbor(loadedData, neighborPos, currentId, currentColor);
            //check _leftNeighbor
            neighborPos = new Vector2Int(currentPosition.x - 1, currentPosition.y);
            _leftNeighbor = CheckNeighbor(loadedData, neighborPos, currentId, currentColor);
            //check _rightNeighbor
            neighborPos = new Vector2Int(currentPosition.x + 1, currentPosition.y);
            _rightNeighbor = CheckNeighbor(loadedData, neighborPos, currentId, currentColor);
        }

        private bool CheckNeighbor(GridDataJson loadedData, Vector2Int neighborPos, int currentId, Color currentColor)
        {
            CellDataJson neighborData = loadedData.cellsData.Find(x => x.position == neighborPos && 
                                                                  x.id == currentId &&
                                                                  x.color == currentColor);
            return neighborData != null;
        }

        private void SetWalls()
        {
            _walls = 0;
            _walls = _walls + (_upNeighbor ? 0 : 1);
            _walls = _walls + (_downNeighbor ? 0 : 1);
            _walls = _walls + (_leftNeighbor ? 0 : 1);
            _walls = _walls + (_rightNeighbor ? 0 : 1);
        }
    }
}