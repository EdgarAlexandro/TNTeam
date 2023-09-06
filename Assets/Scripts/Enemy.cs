using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float range;
    public Transform target;
    private float minDistance = 5.0f;
    private bool targetCollision = false;
    private float speed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        range = Vector2.Distance(transform.position, target.position)
 ;
        if (range < minDistance)
        {
            if (!targetCollision)
            {
                transform.LookAt(target.position);

                transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }
        transform.rotation = Quaternion.identity;
    }
}
