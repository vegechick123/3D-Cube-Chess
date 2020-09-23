using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Manager<UIManager>
{
    public GameObject prefabFloorHDU;
    public GameObject[] CreateFloorHUD(Vector2Int[] location, Color color)
    {
        GameObject[] gameObject = new GameObject[location.Length];
        for(int i=0;i<location.Length;i++)
        {
            gameObject[i] = CreateFloorHUD(location[i], color);
        }
        return gameObject;
    }
    public GameObject CreateFloorHUD(Vector2Int location,Color color)
    {
        GameObject gameObject= GameObject.Instantiate(prefabFloorHDU, GridManager.GetFloorPosition3D(location) + new Vector3(0, 0.51f, 0),Quaternion.identity);
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        return gameObject;
    }
}
