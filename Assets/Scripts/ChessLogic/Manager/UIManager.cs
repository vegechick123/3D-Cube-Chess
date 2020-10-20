using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WebSocketSharp;

public class UIManager : Manager<UIManager>
{
    public GameObject prefabFloorHDU;
    public GameObject prefabHealthBar;
    public GameObject prefabSkillButton;


    public GameObject prefabImage;
    public GameObject prefabMessage;
    [NonSerialized]
    public GameObject[] skillButtons;
    public GameObject skillBar;
    public Gradient tempertureColorGradient;
    public UnityEvent eRefreshFloorHUD = new UnityEvent();
    public Transform panelContainer; 
    protected List<GameObject> alivePanels = new List<GameObject>();
    protected Outline aliveOutline;
    protected GameObject overTileFloorHUD;
    [NonSerialized]
    private bool bShowTemperture=false; 
    protected override void Awake()
    {
        base.Awake();
        PlayerControlManager.instance.eOverTile.AddListener(OverTile);
    }
    public void RefreshTemperture()
    {
        if (bShowTemperture)
            ShowTemperture();
    }
    public void ShowTemperture()
    {
        bShowTemperture = true;   
        for (int i = 0; i < GridManager.instance.size.x; i++)
            for (int j = 0; j < GridManager.instance.size.y; j++)
            {
                var t = GridManager.instance.GetFloor(new Vector2Int(i, j));
                if (t == null)
                    continue;
                Color c = tempertureColorGradient.colorKeys[TempertureManager.instance.GetTempatureAt(new Vector2Int(i, j))+1].color;
                t.render.material.SetColor("_Color",c);
                t.render.material.SetFloat("_Blend", 0.5f);
            }
    }
    public void HideTemperture()
    {
        bShowTemperture = false;
        for (int i = 0; i < GridManager.instance.size.x; i++)
            for (int j = 0; j < GridManager.instance.size.y; j++)
            {
                var t = GridManager.instance.GetFloor(new Vector2Int(i, j));
                //Color c = tempertureColorGradient.colorKeys[TempertureManager.instance.GetTempatureAt(new Vector2Int(i, j)) + 1].color;
                t.render.material.SetColor("_Color", Color.white);
                t.render.material.SetFloat("_Blend", 0);
            }
    }
    
    void OverTile(Vector2Int location)
    {
        if (overTileFloorHUD != null)
            Destroy(overTileFloorHUD);
        if(aliveOutline!=null)
        {
            aliveOutline.RemoveReference();
            aliveOutline = null;
        }
        if(location!=Vector2Int.down)
        {
            overTileFloorHUD = CreateFloorHUD(location, Color.yellow);
        }

        GChess t = GridManager.instance.GetChess(location);
        GFloor f = GridManager.instance.GetFloor(location);
        var list = new List<IGetInfo>();
        if (t != null)
        {
            list.AddRange(t.GetInfos());
            aliveOutline =t.outline;
            aliveOutline.AddReference();
        }
        if (f != null)
        {
            list.AddRange(f.GetInfos());
        }
        list.AddRange(GridManager.instance.GetEnvironmentInformation(location));
        CreateMessage(list);
    }
    public GameObject CreateMessage(List<IGetInfo> infos)
    {
        //GChess t = GridManager.instance.GetChess(location);
        foreach (GameObject temp in alivePanels)
        {
            Destroy(temp);
        }
        alivePanels.Clear();
        foreach (IGetInfo info in infos)
        {
            GameObject gameObject = GameObject.Instantiate(prefabMessage, panelContainer);
            alivePanels.Add(gameObject);
            gameObject.GetComponent<Messages>().Init(info);            
        }

        return gameObject;
    }

    public GameObject[] CreateFloorHUD(Vector2Int[] location, Color color)
    {
        GameObject[] gameObject = new GameObject[location.Length];
        for (int i = 0; i < location.Length; i++)
        {
            gameObject[i] = CreateFloorHUD(location[i], color);
        }
        return gameObject;
    }
    public GameObject CreateFloorHUD(Vector2Int location, Color color)
    {
        GameObject gameObject = GameObject.Instantiate(prefabFloorHDU, GridManager.instance.GetFloorPosition3D(location) + new Vector3(0, 0.51f, 0), Quaternion.identity);
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        return gameObject;
    }
    public GameObject CreateHealthBar(GChess chess)
    {

        GameObject gameObject = GameObject.Instantiate(prefabHealthBar, chess.render.transform);
        gameObject.GetComponent<GetImage>().Init(chess);

        return gameObject;
    }

    public void CleanSkillButton()
    {
        if (skillButtons != null)
            foreach (GameObject gameObject in skillButtons)
            {
                Destroy(gameObject);
            }
    }
    public GameObject CreatButtonFromSkill(PlayerSkill skill)
    {
        GameObject gameObject = Instantiate(prefabSkillButton, skillBar.transform);
        gameObject.GetComponent<Image>().sprite = skill.icon;
        gameObject.GetComponent<Button>().onClick.AddListener(skill.CreateCommand);
        gameObject.GetComponent<SkillButton>().skill = skill;
        return gameObject;
    }
    public GameObject[] CreatButtonFromSkill(PlayerSkill[] skills)
    {
        GameObject[] gameObjects = new GameObject[skills.Length];
        for (int i = 0; i < skills.Length; i++)
        {
            gameObjects[i] = CreatButtonFromSkill(skills[i]);
        }
        return gameObjects;
    }
    public void SwitchSkillButton(PlayerSkill[] skills)
    {
        CleanSkillButton();
        skillButtons = CreatButtonFromSkill(skills);
    }
    public void DisableSkillButton()
    {
        foreach (GameObject t in skillButtons)
        {
            t.GetComponent<Button>().interactable = false;
        }
    }
    public void ActiveSkillButton()
    {
        foreach (GameObject t in skillButtons)
        {
            t.GetComponent<Button>().interactable = true;
        }
    }

    public string GetHighTempertureRichText()
    {
        return "<color=orange>高温</color>";
    }
    public string GetLowTempertureRichText()
    {
        return "<color=cyan>低温</color>";
    }
}
