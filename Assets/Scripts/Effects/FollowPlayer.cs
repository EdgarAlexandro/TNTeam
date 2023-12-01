/* Function: allows the particle effects to follow the player
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 28/11/2023 */

using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;

    void Awake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player.transform.position == transform.position)
            {
                target = player.transform;
                break;
            }
        }
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            if(target.GetComponent<PlayerControl>().moveSpeed >= 3) Destroy(gameObject);
        }
    }
}