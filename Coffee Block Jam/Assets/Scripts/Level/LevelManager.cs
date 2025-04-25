using CoffeeBlockJam.Grid;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoffeeBlockJam.Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GridController _gridBuilder;

        private void Awake()
        {
            TextAsset[] jsonText = Resources.LoadAll<TextAsset>("Jsons");
            TextAsset randomJson = jsonText[Random.Range(0, jsonText.Length)];
            GridDataJson gridData = JsonUtility.FromJson<GridDataJson>(randomJson.text);
            _gridBuilder.BuildGridAndTrays(gridData);

            Transform camera = Camera.main.transform;
            camera.position = new Vector3(gridData.width/2, -gridData.height/2, -10f);
        }

        public void ResetLevel() 
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
