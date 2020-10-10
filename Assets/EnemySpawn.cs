using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
public class EnemySpawn : MonoBehaviour
{
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
            minY = Mathf.Min(minY, t.location.y-spawnDistance);
            maxY = Mathf.Max(minY, t.location.y+spawnDistance);
        }
        if(maxY<minY)
        {
            return;
        }
        Vector2Int leftButtom=new Vector2Int(0,minY),rightTop=new Vector2Int(GridManager.instance.size.x, maxY);
        for (int i=0;i<num;i++)
        {
            Vector2Int targetLocation = GetValidLocation(leftButtom, rightTop);
            GridManager.instance.InstansiateChessAt(prefabEnemy,targetLocation);
        }

    }
    Vector2Int GetValidLocation(Vector2Int leftButtom, Vector2Int rightTop)
    {
        System.Random random = new System.Random();
        Vector2Int t = new Vector2Int();
        do
        {
            t.y = random.Next(leftButtom.y, rightTop.y);
            t.x = random.Next(leftButtom.x, rightTop.x);
        } while (!GridManager.instance.CheckTransitability(t));
        return t;
    }
    
}
