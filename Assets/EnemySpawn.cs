using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
public class EnemySpawn : MonoBehaviour
{
    public int maxEnemyNum=5;
    public int num = 2;
    public int spawnDistance=3;
    public GameObject prefabEnemy;
    public void SpawnEnemy()
    {
        int minY = 100,maxY=0;
        GChess[] targets = GridManager.instance.GetChesses(1);
        foreach(var t in targets)
        {
            if (t.elementComponent.state == ElementState.Frozen)
                continue;
            minY = Mathf.Min(minY, t.location.y);
            maxY = Mathf.Max(minY, t.location.y);
        }
        if(maxY<minY)
        {
            return;
        }
        Vector2Int leftButtom=new Vector2Int(0,minY-spawnDistance),rightTop=new Vector2Int(GridManager.instance.size.x, maxY+spawnDistance);
        int cntNum = AIManager.instance.AIs.Count;
        int spawnnum = Mathf.Min(num,maxEnemyNum-cntNum);
        for (int i=0;i<spawnnum;i++)
        {
            Vector2Int targetLocation = GetValidLocation(leftButtom, rightTop);
            GridManager.instance.InstansiateChessAt(prefabEnemy,targetLocation);
        }

    }
    Vector2Int GetValidLocation(Vector2Int leftButtom, Vector2Int rightTop)
    {        
        Vector2Int t = new Vector2Int();
        do
        {
            t = GridFunctionUtility.GetRandomLocation(leftButtom,rightTop);
        } while (!GridManager.instance.CheckTransitability(t));
        return t;
    }
    
}
