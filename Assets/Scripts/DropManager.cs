using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DropData class that contains the drop's position, the scene it belongs to and it's tag
[System.Serializable]
public class DropData
{
    public Vector2 position;
    public string sceneName;
    public string tag;

    // Override Equals method
    public override bool Equals(object obj)
    {
        //Check if obj is null or if it is a different type than DropData
        if (obj == null || !(obj is DropData))
        {
            return false;
        }

        DropData other = (DropData)obj;

        // Equality logic
        return position == other.position && sceneName == other.sceneName && tag == other.tag;
    }

    // Create hash code
    public override int GetHashCode()
    {
        return position.GetHashCode() ^ sceneName.GetHashCode();
    }
}

public class DropManager : MonoBehaviour
{
    private static DropManager instance;
    public static DropManager Instance { get { return instance; } }

    public List<GameObject> objectsPrefabs = new List<GameObject>();
    private List<DropData> dropPositions = new List<DropData>();

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

    // Add drop position to the drop positions list
    public void AddDropPosition(Vector2 dropPosition, string sceneName, string tag)
    {
        bool positionExists = dropPositions.Exists(data => data.position == dropPosition && data.sceneName == sceneName && data.tag == tag);

        // Check if the position waiting to be saved is not already in the list
        if (!positionExists)
        {
            DropData dropData = new DropData { position = dropPosition, sceneName = sceneName, tag = tag };
            dropPositions.Add(dropData);
        }
    }

    // Verify if there are saved drop positions
    public bool HasSavedDropPositions()
    {
        return dropPositions.Count > 0;
    }

    // Restore drops in the positions they belong to
    public void RestoreDropPositions(string currentScene)
    {
        for(int i = 0; i < objectsPrefabs.Count; i++)
        {
            string tag = objectsPrefabs[i].gameObject.tag;
            List<DropData> savedDropPositions = GetSavedPositions(currentScene, tag);
            foreach (DropData data in savedDropPositions)
            {
                Instantiate(objectsPrefabs[i], data.position, Quaternion.identity);
            }
        }
    }

    // Return the saved positions based on the given parameters
    private List<DropData> GetSavedPositions(string currentScene, string tag)
    {
        return dropPositions.FindAll(dropData => dropData.sceneName == currentScene && dropData.tag == tag);
    }
    
    // Remove a specific drop position from the drop positions list
    public void RemoveDropPosition(Vector2 dropPosition, string sceneName, string tag)
    {
        DropData dataToRemove = new DropData { position = dropPosition, sceneName = sceneName, tag = tag };

        dropPositions.RemoveAll(data => data.Equals(dataToRemove)); 
    }

    // Remove all drop positions from the drop positions list
    public void RemoveAllDropPositions()
    {
        dropPositions.Clear();
    }
}
