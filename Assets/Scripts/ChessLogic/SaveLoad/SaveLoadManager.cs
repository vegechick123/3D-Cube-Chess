using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : SingletonMonoBehaviour<SaveLoadManager>
{
    static string savePath { get { return Application.persistentDataPath + "/saveData.json"; } }
    public SaveData currentData;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    public void LoadSaveData()
    {
        string json = File.ReadAllText(savePath);
        SaveData result = JsonUtility.FromJson<SaveData>(json);
        currentData = result; 
    }
    public void CleanCurrentSaveData()
    {
        currentData = new SaveData();
        currentData.levelIndex = LevelManager.firstLevel;
    }
    public void Save()
    {
        string json = JsonUtility.ToJson(currentData);
        File.WriteAllText(savePath, json);
        Debug.Log(json);
    }
    public void RecordNewData(int nextLevel)
    {
        SaveData res = new SaveData();
        res.levelIndex = nextLevel;
        List<GPlayerChess> results = GridManager.instance.playerChesses.FindAll(x => x.persistentSave);
        foreach (GPlayerChess t in results)
        {
            res.playerChessDatas.Add(t.GetSaveData());
        }
        currentData = res;
    }

}
[Serializable]
public class SaveData
{
    public int levelIndex;
    public List<PlayerChessData> playerChessDatas = new List<PlayerChessData>();
}
[Serializable]
public class PlayerChessData
{
    public GameObject prototype;
    public List<PlayerSkillData> skills = new List<PlayerSkillData>();
}
[Serializable]
public class PlayerSkillData
{
    public PlayerSkill prototype;
    public int useCount;
}