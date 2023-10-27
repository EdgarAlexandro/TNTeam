/* Function: controls the behaviour of the item: bomba (used)
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 14/10/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Bomba : MonoBehaviourPunCallbacks
{
    private SpriteRenderer spriteRenderer = null;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Explode());
    }

    //Remote procedure call to destroy the bomb after explosion
    [PunRPC]
    public void DestroyBomb(string bombGameObjectName)
    {
        GameObject bomb = GameObject.Find(bombGameObjectName);
        if (bomb.GetPhotonView().IsMine)
        {
            PhotonNetwork.Destroy(bomb);
        }
    }

    IEnumerator Explode()
    {
        // Color changing effect
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }

        // Gets all the colliders in a radius and deals damage to the enemies or destroys boxes ????
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius / 2);
        foreach (Collider2D col in colliders)
        {
            EnemyAi saludEnemigo;
            if (col.TryGetComponent(out saludEnemigo))
            {
                saludEnemigo.OnHit(1, new Vector2(2.0f, 3.0f), 2.0f);
            }
            CajaRotaSpawn cajaDestroy;
            if (col.TryGetComponent(out cajaDestroy))
            {
                cajaDestroy.boxDestructionAux(SceneManager.GetActiveScene().name);
            }
        }
        // RPC for owner to destroy de gameobject
        photonView.RPC("DestroyBomb", RpcTarget.All, gameObject.name);
    }
}
