using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class CharacterData : ScriptableObject
{
    public List<CharacterClass> characters;
}
