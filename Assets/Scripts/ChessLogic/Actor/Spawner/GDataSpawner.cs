using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDataSpawner : GSpawner
{
    public GPlayerChess Spawn(PlayerChessData data)
    {

        GPlayerChess chess = Spawn(data.prototype);
        chess.InitWithSaveData(data);
        return chess;
    }
}
