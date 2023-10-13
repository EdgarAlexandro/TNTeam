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
    //private int spawnIndex = 0;
    public int spawnHealth;
    public int spawnMaxHealth;
    private SpriteRenderer sprite;
    public Transform spawnPoint;

    [PunRPC]
    public void SpawnEnemy()
    {
        //PhotonNetwork.Instantiate(enemyPrefabString, spawnPoints[spawnIndex % 3].transform.position, Quaternion.identity);
        PhotonNetwork.Instantiate(enemyPrefabString, spawnPoints[Random.Range(0, 1)].transform.position, Quaternion.identity);
    }
    [PunRPC]
    public void DestroySpawner()
    {
        PhotonNetwork.Destroy(gameObject);
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
        if (timer < Time.time)
        {
            //photonView.RPC("SpawnEnemy", RpcTarget.All);
            timer = Time.time + 10.0f;
            //spawnIndex++;
        }
    }

    public void OnHit(int damageAmount)
    {
        spawnHealth -= damageAmount;

        if (spawnHealth <= 0)
        {
            string spawnerIdentifier = gameObject.name;
            DestructionManager.Instance.MarkAsDestroyed(spawnerIdentifier);
            //PhotonNetwork.Destroy(gameObject);
            
            photonView.RPC("SpawnKey", RpcTarget.All);
            
        }
        else
        {
            sprite.color = new Color(255, 0, 0, 255);
            StartCoroutine(ReturnToNormalColor());
        }
    }

    [PunRPC]
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
