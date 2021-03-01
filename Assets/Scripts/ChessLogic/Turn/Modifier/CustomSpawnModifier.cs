using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSpawnModifier : MonoBehaviour
{
    public bool enable=false;
    [SerializeField]
    public List<RoundSpawnData> roundSpawnDatas;
}
[Serializable]
public class RoundSpawnData
{
    public List<SpawnData> spawnDatas;
    public int Count { get { return spawnDatas.Count; } }
    public SpawnData this[int index]
    {
        get { return spawnDatas[index]; }
        set { spawnDatas[index]=value; }
    }
}
[Serializable]
public class SpawnData
{
    public GameObject spawnChess;
    public bool speceficLocation = false;
    public Vector2Int spawnLocation;
}
