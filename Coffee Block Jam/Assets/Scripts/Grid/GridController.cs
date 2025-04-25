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
        private GridBuilder _gridBuilder = null;
        private TraysSectionBuilder _traySectionsBuilder = null;
        private TrayBuilder _trayBuilder = null;

        public void BuildGridAndTrays(GridDataJson dataInJson)
        {
            _gridWidth = dataInJson.width;
            _gridHeight = dataInJson.height;
            _gridBuilder = new ();
            _gridBuilder.CreateGrid(_gridWidth, _gridHeight, dataInJson.offsetX, dataInJson.offsetY, transform, _floorSpriteA, _floorSpriteB);
            _traySectionsBuilder = new ();
            List<ITraySection> traySections = _traySectionsBuilder.CreateTraySections(dataInJson, new TraySecionRulesImpl(), transform);
            _trayBuilder = new ();
            _trayBuilder.CreateTrays(dataInJson, traySections, transform);
        }
    }
}