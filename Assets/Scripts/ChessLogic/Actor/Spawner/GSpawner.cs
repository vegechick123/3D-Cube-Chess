using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSpawner : GActor
{
    public GPlayerChess Spawn(GameObject prototype)
    {

        GameObject go = Instantiate(prototype, GridManager.instance.chessContainer);
        go.transform.position = GridManager.instance.GetChessPosition3D(location);
        GPlayerChess chess = go.GetComponent<GPlayerChess>();
        chess.location = location;
        chess.prefabPrototype = prototype;
        Destroy(gameObject);
        return chess;
    }
}
