using CoffeeBlockJam.Trays;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBlockJam.Grid
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private Sprite _floorSpriteA = null;
        [SerializeField] private Sprite _floorSpriteB = null;
        private int _gridWidth = 1;
        private int _gridHeight = 1;
        private List<ITray> _trays = null;
        private GridBuilder _gridBuilder = null;

        public void BuildGridAndTrays(string dataInJson)
        {
            GridDataJson loadedData = JsonUtility.FromJson<GridDataJson>(dataInJson);

            _gridWidth = loadedData.width;
            _gridHeight = loadedData.height;
            _gridBuilder = new GridBuilder();
            _gridBuilder.CreateGrid(_gridWidth, _gridHeight, loadedData.offsetX, loadedData.offsetY, transform, _floorSpriteA, _floorSpriteB);
        }
    }
}