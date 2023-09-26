using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject enemyPrefab;
    private GameObject instantiatedPrefab;
    public GameObject[] spawnPoints;
    private float timer;
    private int spawnIndex = 0;
    public int spawnHealth;
    public int spawnMaxHealth;
    private SpriteRenderer sprite;

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoints[spawnIndex % 3].transform.position, Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            Instantiate(enemyPrefab, spawnPoints[i].transform.position, Quaternion.identity);
        }

        timer = Time.time + 7.0f;
        spawnHealth = spawnMaxHealth;
        sprite = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if(timer < Time.time)
        {
            SpawnEnemy();
            timer = Time.time + 4.0f;
            spawnIndex++;
        }
    }

    public void OnHit(int damageAmount)
    {
        spawnHealth -= damageAmount;

        if (spawnHealth <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            sprite.color = new Color(255, 0, 0, 255);
            StartCoroutine(ReturnToNormalColor());
        }
    }

    private IEnumerator ReturnToNormalColor()
    {
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(55, 212, 205, 255);
    }
}
