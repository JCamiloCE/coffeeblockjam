using UnityEngine;

namespace CoffeeBlockJam.Grid
{
    public class GridBuilder
    {
        public void CreateGrid(int width, int height, float offsetX, float offsetY, Transform parent, Sprite floorA, Sprite floorB)
        {
            GameObject art = new GameObject("Art");
            art.transform.SetParent(parent);
            GenerateArtForGrid(art, width, height, offsetX, offsetY, floorA, floorB);
        }

        private void GenerateArtForGrid(GameObject art, int width, int height, float offsetX, float offsetY, Sprite floorA, Sprite floorB)
        {
            bool nextTextureIsA = true;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x <= width && y <= height)
                    {
                        GameObject tileArt = new GameObject($"Tile_{x}_{y}");
                        tileArt.transform.position = new Vector3(x * offsetX, -y * offsetY, 0f);
                        tileArt.transform.SetParent(art.transform);

                        SpriteRenderer renderer = tileArt.AddComponent<SpriteRenderer>();
                        renderer.sprite = nextTextureIsA ? floorA : floorB;
                        nextTextureIsA = !nextTextureIsA;
                    }
                }
                if (width % 2 == 0)
                {
                    nextTextureIsA = !nextTextureIsA;
                }
            }
        }
    }
}