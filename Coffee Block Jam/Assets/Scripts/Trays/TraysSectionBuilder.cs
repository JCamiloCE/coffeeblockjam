using CoffeeBlockJam.Grid;
using Enums;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBlockJam.Trays
{
    public class TraysSectionBuilder 
    {
        private string _pathTrays = "Trays";
        private ITraySectionRules _traySectionRules = null;
        private Dictionary<ITraySection, GameObject> _traysPerType = null;
        private List<ITraySection> _finalTraySections = null;

        public List<ITraySection> CreateTraySections(GridDataJson loadedData, ITraySectionRules traySectionRules, Transform parent) 
        {
            _traySectionRules = traySectionRules;
            LoadAllPossibleTrays();
            CreateTraysInScene(loadedData, parent);
            return _finalTraySections;
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
            _finalTraySections = new List<ITraySection>();
            GameObject logic = new GameObject("Logic");
            logic.transform.SetParent(parent);
            foreach (CellDataJson cellData in loadedData.cellsData)
            {
                NeighborsCellData neighborsCellData = new (loadedData, cellData);
                (ETypeTray, float) tuple = _traySectionRules.GetTrayTypeWithRotation(neighborsCellData);
                ITraySection traySection = InstantiateTraysection(tuple.Item1, tuple.Item2, cellData.position, logic.transform, loadedData.offsetX, loadedData.offsetY);
                traySection.SetTraySectionData(cellData.color, cellData.id);
                _finalTraySections.Add(traySection);
            }
        }

        private ITraySection InstantiateTraysection(ETypeTray typeTray, float rotation, Vector2Int position, Transform parent, float offsetX, float offsetY) 
        {
            foreach (KeyValuePair<ITraySection, GameObject> tray in _traysPerType)
            {
                if (tray.Key.GetTypeTray() == typeTray) 
                {
                    GameObject instance = GameObject.Instantiate(tray.Value);
                    instance.transform.localPosition = new Vector3(position.x * offsetX, -position.y * offsetY, -0.2f);
                    instance.transform.SetParent(parent);
                    instance.transform.GetChild(0).localRotation = Quaternion.Euler(0,rotation,0);
                    return instance.GetComponent<ITraySection>();
                }
            }
            return null;
        }
    }
}