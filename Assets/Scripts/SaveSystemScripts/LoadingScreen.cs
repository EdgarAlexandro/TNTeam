/* Function: waits for scene gameobjects to be destroyed, then loads main scene
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 17/11/2023 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        yield return new WaitForSeconds(2);
        
        SceneManager.LoadScene("StartMenu");
    }
}
