/* Function: Manage the destruction of boxes
   Author: Daniel Degollado Rodríguez
   Modification date: 13/10/2023 */
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DestructionManager : MonoBehaviour{
    private static DestructionManager instance;
    public static DestructionManager Instance { get { return instance; } }
    public HashSet<string> destroyedElements = new();

    // Singleton
    private void Awake(){
        if (instance != null && instance != this){
            Destroy(gameObject);
            return;
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Mark an element as destroyed. Takes the name of the box as a parameter
    public void MarkAsDestroyed(string destructionIdentifier){
        destroyedElements.Add(destructionIdentifier);
    }

    // Check if an element is destroyed. Takes the name of the box as a parameter
    public bool IsDestroyed(string destructionIdentifier){
        return destroyedElements.Contains(destructionIdentifier);
    }

    // Remove an element from the destroyed list
    public void RemoveFromDestroyed(){
        destroyedElements.Clear();
    }
}
