using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
public class EnemySpawn : MonoBehaviour
{
    public int maxEnemyOverNum=5;
    public int num = 2;
    public int spawnDistance=3;
    public GameObject[] prefabEnemy;
    [SerializeField] protected float[] probability;
    public void SpawnEnemy()
    {
        Vector2Int yRange = GridFunctionUtility.GetPlayerChessyRange();
        yRange.x = Mathf.Max(0, yRange.x - spawnDistance);
        yRange.y = Mathf.Min(GridManager.instance.size.y-1, yRange.y + spawnDistance);
        
        int cntNum = AIManager.instance.AIs.Count;
        foreach(CAICompoment t in AIManager.instance.AIs)
        {
            if (t.actor.elementComponent.state == ElementState.Frozen)
                cntNum--;
        }
        int maxEnemyNum = maxEnemyOverNum + GridManager.instance.GetPlayerActiveChesses().Count;
        int spawnnum = Mathf.Min(num, maxEnemyNum - cntNum);
        
        for (int i=0;i<spawnnum;i++)
        {
                      
            Vector2Int targetLocation = GetValidLocation(yRange);
            GridManager.instance.InstansiateChessAt(ChooseRandomEnemy(),targetLocation);
        }

    }
    GameObject ChooseRandomEnemy()
    {
        System.Random random = new System.Random();
        int index = 0;
        if (probability.Length == prefabEnemy.Length)
        {
            float rand = (float)random.NextDouble();
            float sum = 0;
            foreach (float x in probability)
            {
                sum += x;
            }
            rand *= sum;
            sum = 0;
            for (int i = 0; i < probability.Length; i++)
            {
                sum += probability[i];
                if (rand < sum)
                {
                    index = i;
                    break;
                }
            }
        }
        return prefabEnemy[index];
    }
    Vector2Int GetValidLocation(Vector2Int yRange)
    {        
        Vector2Int t = new Vector2Int();
        do
        {
            t = GridFunctionUtility.GetRandomLocation(yRange);
        } while (!GridManager.instance.CheckTransitability(t));
        return t;
    }
}
