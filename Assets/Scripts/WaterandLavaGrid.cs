using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

public class WaterandLavaGrid : MonoBehaviour
{
    public Tilemap waterTilemap;
    public int damagePerSecond = 5; // Cantidad de da�o por segundo
    UIController uc;
    PlayerControl playc;
    public bool isPlayerOnBridge = false;
    private bool isPlayerInWater = false;
    public ParticleSystem particulas = null;

    void Start()
    {
        waterTilemap = GetComponent<Tilemap>();
    }

    public void SetPlayerOnBridge(bool value)
    {
        isPlayerOnBridge = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            uc = other.GetComponent<UIController>();
            playc = other.GetComponent<PlayerControl>();
            isPlayerInWater = true;
            StartCoroutine(DealDamage(other.name));
            if (PhotonNetwork.OfflineMode) Instantiate(particulas, other.transform.position, Quaternion.identity);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInWater = false; // Marca que el jugador ha salido del agua
        }
    }

    public IEnumerator DealDamage(string player)
    {
        float time = 0;
        float rate = 1 / 0.5f;
        float trueVelocity = playc.moveSpeed;
        while (time < 1 && isPlayerInWater)
        {
            playc.moveSpeed = isPlayerOnBridge ? trueVelocity : 1.5f; //Ajuste de velocidad 
            if (!isPlayerOnBridge) //Verifica si el jugador toca el agua
            {
                uc.TakeDamage(damagePerSecond, player);
            }

            time += Time.deltaTime * rate;
            yield return new WaitForSeconds(1.0f);
        }
        playc.moveSpeed = trueVelocity;

    }
}