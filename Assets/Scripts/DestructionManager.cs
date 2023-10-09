using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionManager : MonoBehaviour
{
    private static DestructionManager instance;
    public static DestructionManager Instance { get { return instance; } }

    public HashSet<string> destroyedElements = new HashSet<string>();

    // Singleton
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Mark an element as destroyed
    public void MarkAsDestroyed(string destructionIdentifier)
    {
        destroyedElements.Add(destructionIdentifier);
    }

    // Check if an element is destroyed
    public bool IsDestroyed(string destructionIdentifier)
    {
        return destroyedElements.Contains(destructionIdentifier);
    }

    // Remove an element from the destroyed list
    public void RemoveFromDestroyed()
    {
        destroyedElements.Clear();
    }

}
