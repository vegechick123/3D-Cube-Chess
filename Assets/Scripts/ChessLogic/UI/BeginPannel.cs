using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BeginPannel : MonoBehaviour
{
    public GameObject startButton;
    private void Awake()
    {
        startButton.SetActive(false);
    }
    public void LoadScene(int index)
    {
        int curIndex=SceneManager.GetActiveScene().buildIndex;
        if(curIndex!=index)
            SceneManager.LoadScene(index);
        else
        {
            gameObject.SetActive(false);
            //startButton.SetActive(true);
            GameManager.instance.GameStart();
        }
    }
}
