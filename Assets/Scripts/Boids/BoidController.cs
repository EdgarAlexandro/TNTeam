/* Function: allows the enemies to move around while player is not at sight (using boids algorithm)
   Author: Edgar Alexandro Castillo Palacios & Carlos Morales
   Modification date: 27/11/2023 */

using UnityEngine;
using System.Collections.Generic;

public class BoidController : MonoBehaviour
{
    //Boids algorith variables
    public float maxSpeed = 2f;
    public float neighborRadius = 5f;
    public float separationDistance = 4f;
    private List<Boid> boids;

    //Finds all objects that use boids algorithm and updates them
    void Update()
    {
        boids = new List<Boid>(FindObjectsOfType<Boid>());
        foreach (var boid in boids)
        {
            boid.UpdateBoid(boids, maxSpeed, neighborRadius, separationDistance);
        }
    }
}
