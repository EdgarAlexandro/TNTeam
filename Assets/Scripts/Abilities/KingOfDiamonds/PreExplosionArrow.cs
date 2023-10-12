using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// PROGRAMAR: si entra en contacto con un enemigo instancia una CrystallineExplosion y luego se destruye a sí misma
public class PreExplosionArrow : MonoBehaviour
{
    public CircleCollider2D explosion;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Caja"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //this.velocity = projDirection * speed;
        }
    }
    
    
}
