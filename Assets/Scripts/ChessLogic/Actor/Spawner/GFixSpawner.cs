using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFixSpawner : GSpawner
{
    public GameObject prefabFixedSpawn;
    public GPlayerChess SpawnFixProtype()
    {
        return Spawn(prefabFixedSpawn);
    }
}
