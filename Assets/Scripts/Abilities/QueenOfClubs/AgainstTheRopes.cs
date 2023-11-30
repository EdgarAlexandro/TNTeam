using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AgainstTheRopes : MonoBehaviourPunCallbacks
{
    private float maxSize = 1.0f;
    private float growthRate = 3f;
    private float rotationSpeed = 30.0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(growFieldC());

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy")){
            other.GetComponent<EnemyAi>().StartCoroutine(other.GetComponent<EnemyAi>().Freeze());
        }
        
    }

    [PunRPC]
    public void growField()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        if (transform.localScale.x < maxSize)
        {
            transform.localScale += new Vector3(growthRate, growthRate, 0) * Time.deltaTime;
        }
    }

    public IEnumerator growFieldC()
    {
        photonView.RPC("growField", RpcTarget.All);
        yield return new WaitForSeconds(0.01f);
    }
}
