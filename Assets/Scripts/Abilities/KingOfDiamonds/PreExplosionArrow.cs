using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// PROGRAMAR: si entra en contacto con un enemigo instancia una CrystallineExplosion y luego se destruye a sí misma
public class PreExplosionArrow : MonoBehaviour
{
    public CircleCollider2D explosion;

    [PunRPC]
    public void MoveArrow(Vector2 projDirection, float speed)
    {
        GetComponent<Rigidbody2D>().velocity = projDirection * speed;
        // Rotar la flecha para que apunte en la direcci�n correcta
        float angle = Mathf.Atan2(projDirection.y, projDirection.x) * Mathf.Rad2Deg;
        GetComponent<Rigidbody2D>().rotation = angle;
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Caja"))
        {
            PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
            //this.velocity = projDirection * speed;
        }
    }
    
    
}
