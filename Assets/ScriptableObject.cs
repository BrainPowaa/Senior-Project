using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class SpawnManagerScriptableObject : ScriptableObject
{
    //This is the how we manage the ScriptableObjects that we create in unity. 


    public string prefabName;

    public int numberOfPrefabsToCreate;
    public Vector3[] spawnPoints;


}
