using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnerScript : MonoBehaviourPun
{
    public string enemyPrefabString;
    public string keyPrefabString;
    public GameObject[] spawnPoints;
    private float timer;
    private int spawnIndex = 0;
    public int spawnHealth;
    public int spawnMaxHealth;
    private SpriteRenderer sprite;
    public Transform spawnPoint;

    public void SpawnEnemy()
    {
        //PhotonNetwork.Instantiate(enemyPrefabString, spawnPoints[spawnIndex % 3].transform.position, Quaternion.identity);
        PhotonNetwork.Instantiate(enemyPrefabString, spawnPoints[spawnIndex].transform.position, Quaternion.identity);
        if (spawnIndex == 0) spawnIndex++;
        else spawnIndex--;
    }

    [PunRPC]
    public void DestroySpawner()
    {
        string spawnerIdentifier = gameObject.name;
        DestructionManager.Instance.MarkAsDestroyed(spawnerIdentifier);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = gameObject.transform;
        /*
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            PhotonNetwork.Instantiate(enemyPrefabString, spawnPoints[i].transform.position, Quaternion.identity);
        }*/

        //timer = Time.time + 7.0f;
        spawnHealth = spawnMaxHealth;
        sprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (timer < Time.time)
            {
                timer = Time.time + 3.0f;
                GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
                if (objectsWithTag.Length < 6)
                {
                    SpawnEnemy();
                }
               
                //spawnIndex++;
            }
        }
    }

    public void OnHit(int damageAmount)
    {
        spawnHealth -= damageAmount;

        if (spawnHealth <= 0)
        {
            SpawnKey();
        }
        else
        {
            sprite.color = new Color(255, 0, 0, 255);
            StartCoroutine(ReturnToNormalColor());
        }
    }

    public void SpawnKey()
    {
        PhotonNetwork.Instantiate(keyPrefabString, spawnPoint.position, Quaternion.identity);
        photonView.RPC("DestroySpawner", RpcTarget.All);
    }

    private IEnumerator ReturnToNormalColor()
    {
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(55, 212, 205, 255);
    }
}
