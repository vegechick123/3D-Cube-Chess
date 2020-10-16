using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject loseUI;
  
   
    public void Awake()
    {
        GameManager.instance.eGameLose.AddListener(Losetext);
        gameObject.SetActive(false);
    }
    
    void Losetext()
    {
      
       loseUI.SetActive(true);

        Debug.Log("你输了");
    }

}

