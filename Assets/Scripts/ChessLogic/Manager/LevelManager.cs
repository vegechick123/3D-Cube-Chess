using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LevelManager : Manager<LevelManager>
{
    // Start is called before the first frame update
    public GChess chess;

    //public Vector2Int[] path;
    void Start()
    {
        //chess.GetComponent<CMoveComponent>().RequestMove(path);
        //chess.GetComponent<CNavComponent>().GenNavInfo();
        //Vector2Int[] range = chess.GetComponent<CNavComponent>().navInfo.range;
        //foreach (var t in range)
        //    Debug.Log(t);
        //Color c = Color.green;
        //c.a = 0.5f;
        //HUDManager.instance.CreateFloorHUD(new Vector2Int(0, 1), c);
    }


}
