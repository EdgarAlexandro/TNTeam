/* Function: interface to describe the methods the game scripts need
   Author: Edgar Alexandro Castillo Palacios
   Modification date: 02/11/2023 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData data);

    void SaveData(ref GameData data);
}
