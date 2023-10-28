using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SaveLoadSystem
{
    public class SaveGameController : MonoBehaviour
    {
        public void SaveGame()
        {
            SaveGameManager.Save();
        }

        public void LoadGame()
        {

        }
    }
}

