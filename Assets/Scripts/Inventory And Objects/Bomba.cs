using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius/2);
        foreach (Collider2D col in colliders)
        {
            EnemyAi saludEnemigo = col.GetComponent<EnemyAi>();
            if (saludEnemigo != null) saludEnemigo.OnHit(1, new Vector2(2.0f, 3.0f), 2.0f);
        }
        Destroy(gameObject);
    }
}
