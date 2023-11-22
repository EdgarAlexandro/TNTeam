/* Function: manages the player data that is going to be saved
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 02/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string password;
    public string scene;
    public List<int> playersHealth;
    public List<int> playersMagic;
    public List<Vector3> playerPosition;
    public List<string> charactersSelected;
    public int obtainedKeys;
    public int obtainedLevers;
    public List<DropData> dropDatas;
    public List<string> destroyedElements;
    public List<string> playerOneInventory;
    public List<string> playerTwoInventory;
    public List<string> playerOneCardInventory;
    public List<string> playerTwoCardInventory;

    //Constructor where we define the default values (when there's no data to load)
    public GameData()
    {
        this.scene = "Main 1";
        this.playersHealth = new List<int> { 20 };
        this.playersMagic = new List<int> { 0 };
        this.playerPosition = new();
        this.charactersSelected = new List<string> { "", "" };
        this.obtainedKeys = 0;
        this.obtainedLevers = 0;
        this.dropDatas = new();
        this.destroyedElements = new();
        this.playerOneInventory = new List<string> { };
        this.playerTwoInventory = new List<string> { };
        this.playerOneCardInventory = new List<string> { };
        this.playerTwoCardInventory = new List<string> { };
    }
}
