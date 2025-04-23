using CoffeeBlockJam.Grid;
using Enums;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public class TraysBuilder 
    {
        private string _pathTrays = "Trays";
        private ITraySectionRules _traySectionRules = null;
        private Dictionary<ITraySection, GameObject> _traysPerType = null;

        public void CreateTrays(GridDataJson loadedData, ITraySectionRules traySectionRules, Transform parent) 
        {
            _traySectionRules = traySectionRules;
            LoadAllPossibleTrays();
            CreateTraysInScene(loadedData, parent);
        }

        private void LoadAllPossibleTrays() 
        {
            _traysPerType = new Dictionary<ITraySection, GameObject>();
            GameObject[] allTrays = Resources.LoadAll<GameObject>(_pathTrays);

            foreach (GameObject tray in allTrays)
            {
                ITraySection traySection = tray.GetComponent<ITraySection>();
                _traysPerType[traySection] = tray;
            }
        }

        private void CreateTraysInScene(GridDataJson loadedData, Transform parent) 
        {
            GameObject logic = new GameObject("Logic");
            logic.transform.SetParent(parent);
            foreach (CellDataJson cellData in loadedData.cellsData)
            {
                NeighborsCellData neighborsCellData = new (loadedData, cellData.position);
                (ETypeTray, float) tuple = _traySectionRules.GetTrayTypeWithRotation(neighborsCellData);
                InstantiateTraysection(tuple.Item1, tuple.Item2, cellData.position, logic.transform, loadedData.offsetX, loadedData.offsetY);
            }
        }

        private void InstantiateTraysection(ETypeTray typeTray, float rotation, Vector2Int position, Transform parent, float offsetX, float offsetY) 
        {
            foreach (KeyValuePair<ITraySection, GameObject> tray in _traysPerType)
            {
                if (tray.Key.GetTypeTray() == typeTray) 
                {
                    GameObject instance = GameObject.Instantiate(tray.Value);
                    instance.transform.localPosition = new Vector3(position.x * offsetX, -position.y * offsetY, -0.2f);
                    instance.transform.SetParent(parent);
                    instance.transform.GetChild(0).localRotation = Quaternion.Euler(0,rotation,0);
                    return;
                }
            }
        }
    }
}