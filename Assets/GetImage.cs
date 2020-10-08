using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetImage : MonoBehaviour
{

    public GameObject prefabImage;
    public GameObject bloodContainer;
    public GameObject bloodFramework;
    public GameObject[] blood;
    GChess chess;
    //用来保证scale不会随父物体的scale而改变
    public void Init(GChess chess)
    {
        float numController = chess.health;
        bloodContainer.GetComponent<RectTransform>().localScale = new Vector3(1 / numController, 1, 1);

        this.chess = chess;
        blood = new GameObject[chess.health];
        for (int i = 0; i < chess.health; i++)
        {
            blood[i] = CreateImage(chess);
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
        transform.localScale=Vector3.one.Divide(transform.parent.lossyScale);
    }
    public void Refresh()
    {
            
        int health = chess.health;
        for (int i = 0; i < health; i++)
        {
            if(i<chess.curHealth)
                blood[i].SetActive(true);
            else
                blood[i].SetActive(false);
        }
    }

}
