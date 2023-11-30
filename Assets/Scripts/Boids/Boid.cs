/* Function: allows the enemies to move around while player is not at sight (using boids algorithm)
   Author: Edgar Alexandro Castillo Palacios & Carlos Morales
   Modification date: 27/11/2023 */

using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 velocity;
    private Camera mainCamera;
    private Animator animator;
    private float timeSinceLastInversion = 0f;
    private float randomTimeVelocityChange = 0f;

    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        animator = GetComponent<Animator>();
        System.Random random = new System.Random();
        double randomValue = 4 + random.NextDouble() * 3;
        randomTimeVelocityChange = (float)randomValue;
    }

    //If the random time defined on awake passed, the direction is changed
    private void Update()
    {
        timeSinceLastInversion += Time.deltaTime;
        if (timeSinceLastInversion > randomTimeVelocityChange)
        {
            velocity = -velocity;
            timeSinceLastInversion = 0f;
        }
    }

    //Boid implementation
    public void UpdateBoid(List<Boid> boids, float maxSpeed, float neighborRadius, float separationDistance)
    {
        if (GetComponent<EnemyAi>().currentTarget == null)
        {
            Vector3 separation = Vector3.zero;
            Vector3 alignment = Vector3.zero;
            Vector3 cohesion = Vector3.zero;
            int neighborCount = 0;

            foreach (var boid in boids)
            {
                if (boid != this)
                {
                    float distance = Vector3.Distance(transform.position, boid.transform.position);
                    if (distance < neighborRadius)
                    {
                        // Separación
                        if (distance < separationDistance)
                        {
                            Vector3 difference = transform.position - boid.transform.position;
                            separation += difference.normalized / distance;
                        }

                        // Alineación y Cohesión
                        alignment += boid.velocity;
                        cohesion += boid.transform.position;
                        neighborCount++;
                    }
                }
            }

            if (neighborCount > 0)
            {
                alignment /= neighborCount;
                alignment = alignment.normalized * maxSpeed;
                cohesion /= neighborCount;
                cohesion = (cohesion - transform.position).normalized * maxSpeed;
                Vector3 direction = (separation * 5 + alignment + cohesion * 0.5f).normalized * maxSpeed;
                velocity = Vector3.Lerp(velocity, direction, Time.deltaTime);
            }

            if (EstáFueraDeLosLímitesDeLaCámara())
            {
                Vector3 directionToCenter = mainCamera.transform.position - transform.position;
                velocity += directionToCenter.normalized * maxSpeed * Time.deltaTime;
            }
            UpdateAnimation(velocity);

            // Update the position (keeping Z value)
            Vector3 newPosition = transform.position + velocity * Time.deltaTime;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
    }

    private bool EstáFueraDeLosLímitesDeLaCámara()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        bool fueraDeLimites = screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
        return fueraDeLimites;
    }

    private void UpdateAnimation(Vector3 velocity)
    {
        bool movingUp = velocity.y > 0;
        bool movingDown = velocity.y < 0;
        bool movingRight = velocity.x > 0;
        bool movingLeft = velocity.x < 0;
        animator.SetBool("isWalkingUp", movingUp);
        animator.SetBool("isWalkingDown", movingDown);
        animator.SetBool("isWalkingRight", movingRight);
        animator.SetBool("isWalkingLeft", movingLeft);
    }
}