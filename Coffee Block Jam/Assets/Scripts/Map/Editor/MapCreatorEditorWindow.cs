using UnityEditor;
using UnityEngine;

namespace CoffeeBlockJam 
{
    public class MapCreatorEditorWindow : EditorWindow
    {
        private int _gridWidth = 10;
        private int _gridHeight = 10;
        private Sprite _floorSpriteA;
        private Sprite _floorSpriteB;
        private float _sizeForPreview = 100f;
        private float _offsetForX = 2.5f;
        private float _offsetForY = 2.5f;

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

            

            EditorGUILayout.Space();

            GUILayout.Label("Preview", EditorStyles.boldLabel);
            _sizeForPreview = EditorGUILayout.FloatField("Size for preview", _sizeForPreview);

            EditorGUILayout.Space();

            Rect previewRect = GUILayoutUtility.GetRect(_gridWidth * _sizeForPreview, _gridHeight * _sizeForPreview);
            HandleMouseInput(previewRect);
            DrawGridPreview(previewRect);

            EditorGUILayout.Space();

            _offsetForX = EditorGUILayout.FloatField("Off set for X in scene", _offsetForX);
            _offsetForY = EditorGUILayout.FloatField("Off set for Y in scene", _offsetForY);
            if (GUILayout.Button("Generate In Scene"))
            {
                GenerateGridInScene();
            }
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

        private void GenerateGridInScene()
        {
            GameObject root = new GameObject("Generated_Grid");
            GameObject art = new GameObject("ArtGrid");
            GameObject logic = new GameObject("LogicGrid");
            art.transform.SetParent(root.transform);
            logic.transform.SetParent(root.transform);
            GenerateArtForGrid(art);
            GenerateLogicSpots(logic);
        }

        private void GenerateArtForGrid(GameObject art) 
        {
            bool nextTextureIsA = true;
            for (int y = 0; y < _gridHeight; y ++)
            {
                for (int x = 0; x < _gridWidth; x ++)
                {
                    if (x <= _gridWidth && y <= _gridHeight)
                    {
                        GameObject tileArt = new GameObject($"Tile_{x}_{y}");
                        tileArt.transform.position = new Vector3(x * _offsetForX, -y * _offsetForY, 0f);
                        tileArt.transform.SetParent(art.transform);

                        SpriteRenderer renderer = tileArt.AddComponent<SpriteRenderer>();
                        renderer.sprite = nextTextureIsA ? _floorSpriteA : _floorSpriteB;
                        nextTextureIsA = !nextTextureIsA;
                    }
                }
                if (_gridWidth % 2 == 0)
                {
                    nextTextureIsA = !nextTextureIsA;
                }
            }
        }

        private void GenerateLogicSpots(GameObject logic) 
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                for (int x = 0; x < _gridWidth; x++)
                {
                    GameObject tileArt = new GameObject($"Tile_{x}_{y}");
                    tileArt.transform.position = new Vector3(x * _offsetForX, -y * _offsetForY, 0f);
                    tileArt.transform.SetParent(logic.transform);
                }
            }
        }
    }
}

