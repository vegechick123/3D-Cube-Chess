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
    private CustomSpawnModifier spawnModifier;
    public override void Init(TurnManager turnManager)
    {
        base.Init(turnManager);
        spawnModifier = turnManager.GetComponent<CustomSpawnModifier>();
    }
    async public override UniTask TurnBehaviourAsync()
    {
        int cntNum = AIManager.instance.AIs.Count;

        int maxEnemyNum = maxEnemyOverNum;
        int spawnNum = Mathf.Min(num, maxEnemyNum - cntNum);
        if (spawnModifier?.enable == true)
        {
            if (turnManager.currentRound < spawnModifier.roundSpawnDatas.Count)
                spawnNum = spawnModifier.roundSpawnDatas[turnManager.currentRound].Count;
            else
                spawnNum = 0;
        }
        for (int i = 0; i < spawnNum; i++)
        {

            Vector2Int targetLocation = GetValidLocation(turnManager.currentRound, i);
            GridManager.instance.InstansiateChessAt(ChooseRandomEnemy(turnManager.currentRound, i), targetLocation);
            GameObject t = UIManager.instance.CreateFloorHUD(targetLocation, Color.yellow);
            UIManager.instance.eRefreshFloorHUD.Invoke();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Destroy(t);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    GameObject ChooseRandomEnemy(int round, int id)
    {
        if (spawnModifier?.enable == true)
        {
            return spawnModifier.roundSpawnDatas[round][id].spawnChess;
        }
        else
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
    }
    Vector2Int GetValidLocation(int round, int id)
    {
        if (spawnModifier?.enable == true && 
            spawnModifier.roundSpawnDatas[round][id].speceficLocation&& 
            id < spawnModifier.roundSpawnDatas[round].Count)
        {
            return spawnModifier.roundSpawnDatas[round][id].spawnLocation;
        }
        else
        {          
            Vector2Int t = new Vector2Int();
            do
            {
                t = GridExtensions.GetRandomLocation(new Vector2Int(0,0),GridManager.instance.size);
            } while (!GridManager.instance.CheckTransitability(t));
            return t;
        }
    }
}
