using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
public class GTestChess : GChess
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnSelect()
    {
        base.OnSelect();


        navComponent.GenNavInfo();
        GameObject[] gameObjects = UIManager.instance.CreateFloorHUD(navComponent.navInfo.range, new Color(0, 1f, 0, 0.5f));
        RangeCommand<Action<GFloor>> moveCommand = new RangeCommand<Action<GFloor>>(navComponent.navInfo.range, this, MoveTo);
        moveCommand.eTaskEnd.AddListener(() =>
        {
            foreach (var t in gameObjects)
                Destroy(t);
        });
        PlayerControlManager.instance.GenMoveCommand(moveCommand);

    }
}
