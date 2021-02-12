using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnTurn", menuName = "Turn/EnemySpawnTurn")]
public class EnemySpawnTurn : Turn
{
    public int maxEnemyOverNum = 5;
    public int num = 2;
    public int spawnDistance = 3;
    public GameObject[] prefabEnemy;
    [SerializeField] protected float[] probability;

    async public override UniTask TurnBehaviourAsync()
    {
        Vector2Int yRange = GridExtensions.GetPlayerChessyRange();
        yRange.x = Mathf.Max(0, yRange.x - spawnDistance);
        yRange.y = Mathf.Min(GridManager.instance.size.y - 1, yRange.y + spawnDistance);

        int cntNum = AIManager.instance.AIs.Count;

        int maxEnemyNum = maxEnemyOverNum;
        int spawnnum = Mathf.Min(num, maxEnemyNum - cntNum);

        for (int i = 0; i < spawnnum; i++)
        {

            Vector2Int targetLocation = GetValidLocation(yRange);
            GridManager.instance.InstansiateChessAt(ChooseRandomEnemy(), targetLocation);
            GameObject t = UIManager.instance.CreateFloorHUD(targetLocation, Color.yellow);
            UIManager.instance.eRefreshFloorHUD.Invoke();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Destroy(t);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
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
            t = GridExtensions.GetRandomLocation(yRange);
        } while (!GridManager.instance.CheckTransitability(t));
        return t;
    }
}
