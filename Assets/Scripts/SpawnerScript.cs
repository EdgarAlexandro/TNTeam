using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] spawnPoints;
    private float timer;
    private int spawnIndex = 0;
    public int health = 5;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(enemyPrefab, spawnPoints[0].transform.position, Quaternion.identity);
        Instantiate(enemyPrefab, spawnPoints[1].transform.position, Quaternion.identity);
        timer = Time.time + 7.0f; 
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < Time.time)
        {
            Instantiate(enemyPrefab, spawnPoints[spawnIndex % 2].transform.position, Quaternion.identity);
            timer = Time.time + 7.0f;
            spawnIndex++;
        }
    }
}
