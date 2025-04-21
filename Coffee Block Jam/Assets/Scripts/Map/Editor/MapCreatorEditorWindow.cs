using UnityEditor;
using UnityEngine;

namespace CoffeeBlockJam 
{
    public class MapCreatorEditorWindow : EditorWindow
    {
        private int _gridWidth = 10;
        private int _gridHeight = 10;
        private Sprite _floorSprite;
        private int _spriteCellSize = 1;
        private float _sizeForPreview = 100f;

        [MenuItem("Tools/MapCreatorEditorWindow")]
        public static void OpenMapCreatorWin()
        {
            GetWindow<MapCreatorEditorWindow>("Map Creator");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();

            GUILayout.Label("Map Creator", EditorStyles.boldLabel);
            GUILayout.Label("Floor Sprite", EditorStyles.boldLabel);
            _floorSprite = (Sprite)EditorGUILayout.ObjectField("Sprite", _floorSprite, typeof(Sprite), false);

            if (_floorSprite == null) 
            {
                GUILayout.Label("Floor Sprite should be assign", EditorStyles.boldLabel);
                return;
            }

            EditorGUILayout.Space();

            GUILayout.Label("Configuration grid", EditorStyles.boldLabel);
            _spriteCellSize = EditorGUILayout.IntField("Size per sprite", _spriteCellSize);
            _gridWidth = EditorGUILayout.IntField("Width", _gridWidth);
            _gridHeight = EditorGUILayout.IntField("Height", _gridHeight);

            if (AreGridConditionCompleted())
            {
                GUILayout.Label("Error in grid width, height and sprite cell size", EditorStyles.boldLabel);
                return;
            }

            EditorGUILayout.Space();

            GUILayout.Label("Preview", EditorStyles.boldLabel);
            _sizeForPreview = EditorGUILayout.FloatField("Size for preview", _sizeForPreview);

            EditorGUILayout.Space();

            Rect previewRect = GUILayoutUtility.GetRect(_gridWidth * _sizeForPreview, _gridHeight * _sizeForPreview);
            HandleMouseInput(previewRect);
            DrawGridPreview(previewRect);
        }

        private bool AreGridConditionCompleted() 
        {
            return _gridWidth % _spriteCellSize != 0 ||
                _gridHeight % _spriteCellSize != 0 ||
                _gridWidth < _spriteCellSize ||
                _gridHeight < _spriteCellSize ||
                _gridWidth != _gridHeight;
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
                    Debug.Log("Grilla tocada mouseX " + mouseX + " mouseY " + mouseY);
                }
            }
        }

        private void DrawGridPreview(Rect rect)
        {
            Handles.BeginGUI();

            DrawFloorSprites(rect);
            DrawGrid(rect);

            Handles.EndGUI();
        }

        private void DrawGrid(Rect rect)
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

        private void DrawFloorSprites(Rect rect)
        {
            if (_floorSprite != null)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(_floorSprite);
                if (texture != null)
                {
                    for (int y = 0; y < _gridHeight; y += _spriteCellSize)
                    {
                        for (int x = 0; x < _gridWidth; x += _spriteCellSize)
                        {
                            if (x + _spriteCellSize <= _gridWidth && y + _spriteCellSize <= _gridHeight)
                            {
                                Rect spriteRect = new Rect(
                                    rect.x + x * _sizeForPreview,
                                    rect.y + y * _sizeForPreview,
                                    _spriteCellSize * _sizeForPreview,
                                    _spriteCellSize * _sizeForPreview);

                                GUI.DrawTexture(spriteRect, texture, ScaleMode.StretchToFill, true);
                            }
                        }
                    }
                }
            }
        }
    }
}

