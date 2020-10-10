using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetImage : MonoBehaviour
{

    public GameObject prefabImage;
    public GameObject bloodContainer;
    public GameObject bloodFramework;
    [NonSerialized]
    public GameObject[] blood;
    //public float width=0.5f;
    GChess chess;
    Vector3 originScale;
    //用来保证scale不会随父物体的scale而改变
    public void Init(GChess chess)
    {
        originScale=transform.localScale;
        float numController = chess.health;
        bloodContainer.GetComponent<RectTransform>().localScale = new Vector3(1/ numController, 1, 1);
        //bloodFramework.GetComponent<RectTransform>().localScale = new Vector3(width,1,1);
        this.chess = chess;
        blood = new GameObject[chess.health];
        for (int i = 0; i < chess.health; i++)
        {
            blood[chess.health-1-i] = CreateImage(chess);
        }
    }
    public GameObject CreateImage(GChess chess)
    {

        GameObject gameObject = GameObject.Instantiate(prefabImage, bloodContainer.transform);

        return gameObject;

    }
    private void Update()
    {
        //保证血条不会随父物体形变
        transform.localScale=originScale.Divide(transform.parent.lossyScale);
    }
    public void Refresh()
    {
            
        int health = chess.health;
        for (int i = 0; i < health; i++)
        {
            if(i<chess.curHealth)
                blood[i].GetComponent<Image>().enabled=true;
            else
                blood[i].GetComponent<Image>().enabled = false;
        }
    }

}
