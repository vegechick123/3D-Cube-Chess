using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Turn/FireHandleTurn")]
public class FireHandleTurn : Turn
{
    async public override UniTask TurnBehaviourAsync()
    {
        List<GFloor> igniteFloor = new List<GFloor>();
        List<GFloor> burningUpFloor = new List<GFloor>();
        List<GFloor> allFloors = GridManager.instance.GetAllFloors();
        foreach (GFloor floor in allFloors)
        {
            if (floor.inflammable && !floor.state.IsWet())
            {
                foreach (GFloor adjacentFloor in GridManager.instance.GetAdjacentFloor(floor.location))
                {
                    if (adjacentFloor.floorStateMachine.currentState.IsBurning())
                    {
                        igniteFloor.Add(floor);
                        break;
                    }
                }
            }
        }
        
        foreach (GFloor floor in allFloors)
        {
            if (floor.wooden && floor.floorStateMachine.currentState.IsBurning())
                burningUpFloor.Add(floor);
        }
        System.Random r = new System.Random();
        int[] rands = new int[igniteFloor.Count + burningUpFloor.Count];
        for (int i = 0; i < rands.Length; i++)
            rands[i] = i;
        rands = rands.OrderBy(x => r.Next()).ToArray();
        for (int i = 0; i < rands.Length; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.03f));
            int rand = rands[i];
            
            if(rand<igniteFloor.Count)
            {
                igniteFloor[rand].state.Ignite();

            }
            else
            {
                burningUpFloor[rand - igniteFloor.Count].state.BurningUp();
            }
            
        }
        //foreach (GFloor floor in igniteFloor)
        //{
        //    floor.floorStateMachine.currentState.Ignite();
        //}
        //foreach (GFloor floor in burningUpFloor)
        //{
        //    floor.floorStateMachine.currentState.BurningUp();
        //}

    }
}
