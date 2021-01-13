using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GWaterFloor : GFloor
{
    public GameObject prefabFoam;
    public override void OnChessEnter(GChess chess)
    {
        base.OnChessEnter(chess);
        chess.Die();
        var particle = GameObject.Instantiate(prefabFoam, GridManager.instance.GetChessPosition3D(location), prefabFoam.transform.rotation);
    }
}
