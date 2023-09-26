using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgainstTheRopes : MonoBehaviour
{
    private float maxSize = 6.0f;
    private float growthRate = 3f;
    private float rotationSpeed = 30.0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        if(transform.localScale.x < maxSize){
            transform.localScale += new Vector3(growthRate, growthRate, 0) * Time.deltaTime;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy")){
            other.GetComponent<EnemyAi>().StartCoroutine(other.GetComponent<EnemyAi>().Freeze());
        }
        
    }
}
