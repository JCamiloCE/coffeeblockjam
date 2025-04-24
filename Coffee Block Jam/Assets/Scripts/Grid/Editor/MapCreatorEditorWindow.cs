using Enums;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CoffeeBlockJam.Grid.Editor 
{
    public class MapCreatorEditorWindow : EditorWindow
    {
        private int _gridWidth = 10;
        private int _gridHeight = 10;
        private Sprite _floorSpriteA;
        private Sprite _floorSpriteB;
        private float _sizeForPreview = 100f;
        private float _offsetForX = 1f;
        private float _offsetForY = 1f;
        private bool _showingPreview = false;
        private List<CellDataJson> _marks = new List<CellDataJson>();
        private EColorTray _currentMarkColor = EColorTray.None;
        private int _currentMarkID = 0;
        private bool _isValidGrid = false;
        private string _currentErrorInValidation = string.Empty;
        private MapCreadorValidator _validator = new ();

        [MenuItem("Tools/MapCreatorEditorWindow")]
        public static void OpenMapCreatorWin()
        {
            GetWindow<MapCreatorEditorWindow>("Map Creator");
        }

        private void OnGUI()
        {
            ShowBasicGridConfiguration();

            if (_showingPreview)
            {
                ShowPreview();
                if (_isValidGrid)
                {
                    ShowToGenerateInScene();
                }
                else
                {
                    ShowValidGridSection();
                }
            }
        }

        private void ShowBasicGridConfiguration() 
        {
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            GUILayout.Label("Map Creator", EditorStyles.boldLabel);
            GUILayout.Label("Floor Sprite", EditorStyles.boldLabel);
            _floorSpriteA = (Sprite)EditorGUILayout.ObjectField("Sprite", _floorSpriteA, typeof(Sprite), false);
            _floorSpriteB = (Sprite)EditorGUILayout.ObjectField("Sprite", _floorSpriteB, typeof(Sprite), false);

            if (_floorSpriteA == null || _floorSpriteB == null)
            {
                GUILayout.Label("Floor Sprite should be assign", EditorStyles.boldLabel);
                return;
            }

            EditorGUILayout.Space();

            GUILayout.Label("Configuration grid", EditorStyles.boldLabel);
            _gridWidth = EditorGUILayout.IntField("Width", _gridWidth);
            _gridHeight = EditorGUILayout.IntField("Height", _gridHeight);
            if (EditorGUI.EndChangeCheck()) 
            {
                ResetByChangeBaseConfiguration();
            }

            if (!_showingPreview && GUILayout.Button("Show Preview"))
            {
                _showingPreview = true;
            }
        }

        private void ShowPreview()
        {
            EditorGUILayout.Space();

            GUILayout.Label("Preview", EditorStyles.boldLabel);
            _sizeForPreview = EditorGUILayout.FloatField("Size for preview", _sizeForPreview);

            EditorGUILayout.Space();

            GUILayout.Label("Mark Tool", EditorStyles.boldLabel);
            _currentMarkColor = (EColorTray)EditorGUILayout.EnumPopup("Mark Color", _currentMarkColor);
            _currentMarkID = EditorGUILayout.IntField("Mark ID", _currentMarkID);

            EditorGUILayout.Space();

            Rect previewRect = GUILayoutUtility.GetRect(_gridWidth * _sizeForPreview, _gridHeight * _sizeForPreview);
            HandleMouseInput(previewRect);
            DrawGridPreview(previewRect);
        }

        private void HandleMouseInput(Rect rect)
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown && rect.Contains(currentEvent.mousePosition))
            {
                int mouseX = Mathf.FloorToInt((currentEvent.mousePosition.x - rect.x) / _sizeForPreview);
                int mouseY = Mathf.FloorToInt((currentEvent.mousePosition.y - rect.y) / _sizeForPreview);

                if (mouseX >= 0 && mouseX < _gridWidth && mouseY >= 0 && mouseY < _gridHeight)
                {
                    Vector2Int gridPos = new Vector2Int(mouseX, mouseY);

                    CellDataJson existing = _marks.Find(mark => mark.position == gridPos);
                    if (existing != null)
                    {
                        _marks.Remove(existing);
                    }
                    if (_currentMarkColor != EColorTray.None)
                    {
                        _marks.Add(new CellDataJson(gridPos, _currentMarkColor, _currentMarkID));
                    }
                    Repaint();
                    _isValidGrid = false;
                }
            }
        }

        private void DrawGridPreview(Rect rect)
        {
            Handles.BeginGUI();

            DrawFloorSprites(rect);
            DrawGridBorders(rect);
            DrawMarks(rect);

            Handles.EndGUI();
        }

        private void DrawFloorSprites(Rect rect)
        {
            Texture2D textureA = AssetPreview.GetAssetPreview(_floorSpriteA);
            Texture2D textureB = AssetPreview.GetAssetPreview(_floorSpriteB);
            Texture2D currentTexture = textureA;
            bool nextTextureIsA = true;
            if (textureA != null && textureB != null)
            {
                for (int y = 0; y < _gridHeight; y ++)
                {
                    for (int x = 0; x < _gridWidth; x ++)
                    {
                        if (x <= _gridWidth && y <= _gridHeight)
                        {
                            Rect spriteRect = new Rect(
                                rect.x + x * _sizeForPreview,
                                rect.y + y * _sizeForPreview,
                                _sizeForPreview,
                                _sizeForPreview);

                            currentTexture = nextTextureIsA ? textureA : textureB;
                            nextTextureIsA = !nextTextureIsA;
                            GUI.DrawTexture(spriteRect, currentTexture, ScaleMode.StretchToFill, true);
                        }
                    }
                    if (_gridWidth % 2 == 0)
                    { 
                        nextTextureIsA = !nextTextureIsA; 
                    }
                }
            }
        }

        private void DrawGridBorders(Rect rect)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    Rect cellRect = new Rect(
                        rect.x + x * _sizeForPreview,
                        rect.y + y * _sizeForPreview,
                        _sizeForPreview, _sizeForPreview);

                    Handles.DrawSolidRectangleWithOutline(cellRect, new Color(0, 0, 0, 0), Color.gray);
                }
            }
        }

        private void DrawMarks(Rect rect)
        {
            foreach (var mark in _marks)
            {
                Vector2 cellPos = new Vector2(
                    rect.x + mark.position.x * _sizeForPreview,
                    rect.y + mark.position.y * _sizeForPreview
                );

                Rect circleRect = new Rect(cellPos.x + 5, cellPos.y + 5, _sizeForPreview - 10, _sizeForPreview - 10);

                Handles.color = mark.color;
                Handles.DrawSolidDisc(cellPos + new Vector2(_sizeForPreview / 2, _sizeForPreview / 2), Vector3.forward, _sizeForPreview * 0.3f);

                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    normal = new GUIStyleState { textColor = Color.white },
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold
                };

                GUI.Label(circleRect, mark.id.ToString(), style);
            }
        }

        private void ShowToGenerateInScene()
        {
            EditorGUILayout.Space();

            GUILayout.Label("In Scene", EditorStyles.boldLabel);

            _offsetForX = EditorGUILayout.FloatField("Off set for X in scene", _offsetForX);
            _offsetForY = EditorGUILayout.FloatField("Off set for Y in scene", _offsetForY);

            EditorGUILayout.Space();

            if (GUILayout.Button("Generate In Scene"))
            {
                GenerateGridInScene();
            }
        }

        private void GenerateGridInScene()
        {
            GridDataJson data = new GridDataJson
            {
                width = _gridWidth,
                height = _gridHeight,
                offsetX = _offsetForX,
                offsetY = _offsetForY,
                cellsData = _marks
            };

            string json = JsonUtility.ToJson(data);
            GridController gridController = FindObjectOfType<GridController>();
            gridController.BuildGridAndTrays(data);
            System.IO.File.WriteAllText("Assets/GridsJson/gridData.json", json);
        }

        private void ShowValidGridSection() 
        {
            EditorGUILayout.Space();

            if (GUILayout.Button("Validate Grid"))
            {
                _currentErrorInValidation = string.Empty;
                _isValidGrid =_validator.AreMarksValid(_marks, out _currentErrorInValidation);
            }
            if (!string.IsNullOrEmpty(_currentErrorInValidation))
            {
                GUI.contentColor = Color.red;
                GUILayout.Label("Error: " + _currentErrorInValidation);
            }
        }

        

        private void ResetByChangeBaseConfiguration() 
        {
            _isValidGrid = false;
            _showingPreview = false;
            _marks = new List<CellDataJson>();
            _currentMarkColor = EColorTray.None;
            _currentMarkID = 0;
        }
    }
}

