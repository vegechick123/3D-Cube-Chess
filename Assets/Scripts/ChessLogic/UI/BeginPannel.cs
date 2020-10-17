using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BeginPannel : MonoBehaviour
{
    public GameObject startButton;
    public bool ignoreLoadScene=false;
    private void Awake()
    {
        startButton.SetActive(false);
    }
    public void LoadScene(int index)
    {
        int curIndex=SceneManager.GetActiveScene().buildIndex;
        if(curIndex!=index&&!ignoreLoadScene)
            SceneManager.LoadScene(index);
        else
        {
            gameObject.SetActive(false);
            //startButton.SetActive(true);
            GameManager.instance.GameStart();
        }
    }
}
