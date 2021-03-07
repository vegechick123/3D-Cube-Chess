using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    static int startMenuIndex = 0;
    public static int firstLevel = 1;
    public void NewGame()
    {
        SaveLoadManager.instance.CleanCurrentSaveData();
        SaveLoadManager.instance.Save();
        SceneManager.LoadScene(firstLevel);
    }
    public void LoadGame()
    {
        SaveLoadManager.instance.LoadSaveData();
        SaveData data= SaveLoadManager.instance.currentData;
        SceneManager.LoadScene(data.levelIndex);
    }
    public void LoadNextLevel()
    {
        int index = GetNextLevelIndex();
        SaveLoadManager.instance.RecordNewData(index);
        SaveLoadManager.instance.Save();
        SceneManager.LoadScene(index);
    }
    public void ReturnMenu()
    {
        SaveLoadManager.instance.CleanCurrentSaveData();
        SceneManager.LoadScene(startMenuIndex);
    }
    int GetNextLevelIndex()
    {
        return SceneManager.GetActiveScene().buildIndex+1;
    }
}
