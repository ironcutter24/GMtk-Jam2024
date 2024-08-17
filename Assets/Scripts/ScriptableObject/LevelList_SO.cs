using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "ScriptableObjects/LevelList", order = 1)]
public class LevelList_SO : ScriptableObject
{
    [SerializeField] string[] sceneList;

    public string GetLevelAt(int index)
    {
        return sceneList[index];
    }

    public int GetCount()
    {
        return sceneList.Length;
    }
}
