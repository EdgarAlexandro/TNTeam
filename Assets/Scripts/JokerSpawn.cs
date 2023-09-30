using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JokerSpawn : MonoBehaviour
{
    private static JokerSpawn instance;
    public static JokerSpawn Instance { get { return instance; } }

    public List<string> availableScenes = new List<string>();
    public List<string> allScenePaths = new List<string>();
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        // Randomly select available scenes

        // Add scene names to the availableScenes list based on your selection criteria
        // For example:
        // availableScenes.Add("Scene1");
        // availableScenes.Add("Scene2");

        int numberOfScenesToEnable = 3;

        /*
        for (int i = 0; i < numberOfScenesToEnable; i++)
        {
            int randomSceneIndex = Random.Range(0, SceneManager.sceneCountInBuildSettings);
            string sceneName = SceneUtility.GetScenePathByBuildIndex(randomSceneIndex);
            availableScenes.Add(sceneName);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(sceneName);
            Debug.Log("Available scene " + sceneNameFromPath);
        }
        */

        int totalScenes = SceneManager.sceneCountInBuildSettings;

       
        for (int i = 0; i < totalScenes; i++)
        {
            allScenePaths.Add(SceneUtility.GetScenePathByBuildIndex(i));
        }

        // Shuffle the list of scene paths
        System.Random rng = new System.Random();
        int n = allScenePaths.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = allScenePaths[k];
            allScenePaths[k] = allScenePaths[n];
            allScenePaths[n] = value;
        }

        for (int i = 0; i < numberOfScenesToEnable; i++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(allScenePaths[i]);
            availableScenes.Add(sceneName);

            Debug.Log("Available scene " + sceneName);
        }

        //Debug.Log(string.Join(", ", availableScenes));
    }

    public void RemoveScene(string sceneName)
    {
        availableScenes.Remove(sceneName);
    }
}
