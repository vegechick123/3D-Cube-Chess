using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinWhenEnterTargetPosition : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2Int targetLocation;
    private void Awake()
    {
        GetComponent<GChess>().eLocationChange.AddListener(() =>
        {
            if (GetComponent<GChess>().location==targetLocation)
            {
                GameManager.instance.GameWin();
            }
        });
    }
}
