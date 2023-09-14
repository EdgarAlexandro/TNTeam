using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionSceneChange : MonoBehaviour
{

	//public string level = "testingscene2";

	// Use this for initialization
	void OnCollisionEnter2D(Collision2D Colider)
	{
		//if (Colider.gameObject.tag == "Player") ;
		SceneManager.LoadScene("TestingScene");
	}
	// Start is called before the first frame update

}
