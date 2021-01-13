using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void Reload()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnMenu()
    {
        LoadLevel(0);
    }

}
