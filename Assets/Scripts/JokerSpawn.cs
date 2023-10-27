/* Function: Determine which scenes are available for the Joker to spawn
   Author: Daniel Degollado Rodr�guez
   Modification date: 29/09/2023 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JokerSpawn : MonoBehaviour{
    private static JokerSpawn instance;
    public static JokerSpawn Instance { get { return instance; } }

    public List<string> availableScenes = new List<string>();
    public List<string> allScenePaths = new List<string>();
    private MusicSFXManager musicSFXManager;

    // Start is called before the first frame update
    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        }
        else{
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            musicSFXManager.PlaySFX(MusicSFXManager.Instance.Joker_Sound);
        }
    }

    void Start(){
        // Randomly select available scenes

        int numberOfScenesToEnable = 3;

        int totalScenes = SceneManager.sceneCountInBuildSettings;
        musicSFXManager = MusicSFXManager.Instance;


       
        for (int i = 0; i < totalScenes; i++){
            allScenePaths.Add(SceneUtility.GetScenePathByBuildIndex(i));
        }

        // Shuffle the list of scene paths
        System.Random rng = new System.Random();
        int n = allScenePaths.Count;
        while (n > 1){
            n--;
            int k = rng.Next(n + 1);
            string value = allScenePaths[k];
            allScenePaths[k] = allScenePaths[n];
            allScenePaths[n] = value;
        }
        // Adds available scenes to availableScenes list
        for (int i = 0; i < numberOfScenesToEnable; i++){
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(allScenePaths[i]);
            availableScenes.Add(sceneName);

            Debug.Log("Available scene " + sceneName);
        }
    }
    // Removes available scenes from avaulableScenes list. Takes the scene name as a parameter.
    public void RemoveScene(string sceneName){
        availableScenes.Remove(sceneName);
    }
}
