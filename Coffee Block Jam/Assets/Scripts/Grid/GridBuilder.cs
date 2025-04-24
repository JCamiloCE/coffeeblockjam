using UnityEngine;

namespace CoffeeBlockJam.Grid
{
    public class GridBuilder
    {
        private float _sizeSmall = 0.3f;

        public void CreateGrid(int width, int height, float offsetX, float offsetY, Transform parent, Sprite floorA, Sprite floorB)
        {
            GameObject art = new GameObject("Art");
            art.transform.SetParent(parent);
            GenerateArtForGrid(art, width, height, offsetX, offsetY, floorA, floorB);

            GameObject walls = new GameObject("Walls");
            walls.transform.SetParent(parent);
            GenerateWalls(walls.transform, (width * offsetX), (height * offsetY), offsetX, offsetY);
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

        private void GenerateWalls(Transform parent, float totalSizeX, float totalSizeY, float offsetX, float offsetY) 
        {
            SetUpWall(totalSizeX, offsetX, offsetY).transform.SetParent(parent);
            SetDownWall(totalSizeX, totalSizeY, offsetX, offsetY).transform.SetParent(parent);
            SetLeftWall(totalSizeY, offsetX, offsetY).transform.SetParent(parent);
            SetRigthtWall(totalSizeY, totalSizeX, offsetX, offsetY).transform.SetParent(parent);
        }


        private GameObject SetUpWall(float size, float offsetX, float offsetY) 
        {
            GameObject wall = CreateBaseWall(size, "Up");
            wall.transform.localPosition = new Vector3(size/2 - (offsetX/2), (offsetY / 2) + (_sizeSmall/2), - 0.2f);
            return wall;
        }

        private GameObject SetDownWall(float size, float totalSizeY, float offsetX, float offsetY) 
        {
            GameObject wall = CreateBaseWall(size, "Down");
            wall.transform.localPosition = new Vector3(size/2 - (offsetX/2), -totalSizeY - (_sizeSmall / 2) + (offsetY / 2), -0.2f);
            return wall;
        }

        private GameObject SetLeftWall(float size, float offsetX, float offsetY) 
        {
            GameObject wall = CreateBaseWall(size, "Left");
            wall.transform.localPosition = new Vector3(-(offsetX/2) - (_sizeSmall / 2), -size / 2 + offsetY/2 , -0.2f);
            wall.transform.localRotation = Quaternion.Euler(0f, 0f, 90);
            return wall;
        }

        private GameObject SetRigthtWall(float size, float totalSizeY, float offsetX, float offsetY) 
        {
            GameObject wall = CreateBaseWall(size, "Right");
            wall.transform.localPosition = new Vector3(size - (offsetX / 2) + (_sizeSmall / 2), -size / 2 + offsetY / 2, -0.2f);
            wall.transform.localRotation = Quaternion.Euler(0f, 0f, 90);
            return wall;
        }

        private GameObject CreateBaseWall(float xSize, string name) 
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.layer = 2; //Ignore Raycast
            wall.transform.localScale = new Vector3(xSize, _sizeSmall, _sizeSmall);
            wall.AddComponent<BoxCollider>();
            var rigid = wall.AddComponent<Rigidbody>();
            rigid.isKinematic = true;
            rigid.useGravity = false;
            rigid.constraints = RigidbodyConstraints.FreezeAll;
            return wall;
        }
    }
}