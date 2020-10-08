using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetImage : MonoBehaviour
{

    public GameObject prefabImage;
    public GameObject bloodContainer;
    public GameObject numberController;
    public GameObject[] blood;
    GChess chess;

    public void Init(GChess chess)
    {
        float numController = chess.health;
        numberController.GetComponent<RectTransform>().localScale = new Vector3(1 / numController, 1, 1);
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
