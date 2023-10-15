using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Espada : MonoBehaviourPunCallbacks
{
    public int damage = 1;
    public float knockback = 5f;
    float probabilidadFuncionA;
    float randomValue;
    private DestructionManager dm;
    private JokerSpawn jk;

    void Start()
    {
        dm = DestructionManager.Instance;
        jk = JokerSpawn.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
      
        // Check if the object the sword hit is a box
        if (other.CompareTag("Caja") && this.GetComponentInParent<Animator>().GetBool("isAttacking") && photonView.IsMine)
        {
            List<string> availableScenes = jk.availableScenes;
            string boxIdentifier = other.gameObject.name;
         
            // Check if the current scene was selected as one of the scenes available for Joker spawning
            if(availableScenes.Contains(currentSceneName))
            { 
                probabilidadFuncionA = 0.8f;
                randomValue = Mathf.Round(Random.Range(0f, 1f) * 10f) / 10f;
               
                // If the random value is less than the defined probability value, it spawns an object
                if (randomValue < probabilidadFuncionA)
                {
                    other.GetComponent<CajaRotaSpawn>().SpawnObject();
                }
                // If it is more than the defined probability value, it spawns the Joker
                else
                {
                    other.GetComponent<CajaRotaSpawn>().SpawnJoker();
                    jk.RemoveScene(currentSceneName);
                }
            }
            // If it's not available, it spawns an object
            else
            {
                other.GetComponent<CajaRotaSpawn>().SpawnObject();
            }

            // Marks the box as destroyed
            dm.MarkAsDestroyed(boxIdentifier);
            //PhotonNetwork.Destroy(other.gameObject);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }
            else
            {
                other.GetComponent<CajaRotaSpawn>().photonView.RPC("DestroyBox", RpcTarget.MasterClient);
            }
            
        }

        // Check if the object the sword hit is an enemy
        if (other.gameObject.tag == "Enemy" && this.GetComponentInParent<Animator>().GetBool("isAttacking"))
        {
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
            other.gameObject.TryGetComponent<EnemyAi>(out EnemyAi enemyComponent);
            if (transform.parent.TryGetComponent<ChargeAttack>(out ChargeAttack chargeA))
            {
                if (chargeA.isChargeAttacking)
                {
                    enemyComponent.OnHit(chargeA.chargeDmg, knockbackDirection, knockback * 2);
                    //Debug.Log("Damage dealt: " + chargeA.chargeDmg.ToString());
                    //Debug.Log(chargeA.msg);
                }
            }
            else
            {
                // Deals damage to the enemy and applys a knockback to it
                enemyComponent.OnHit(damage, knockbackDirection, knockback);
            }
        }

        // Check if the object the sword hit is a spawner
        if (other.gameObject.tag == "Spawner" && this.GetComponentInParent<Animator>().GetBool("isAttacking") && photonView.IsMine)
        {
            other.gameObject.TryGetComponent<SpawnerScript>(out SpawnerScript spawnerComponent);
            // Deals damage to the spawner
            spawnerComponent.OnHit(damage);
        }
    }
}