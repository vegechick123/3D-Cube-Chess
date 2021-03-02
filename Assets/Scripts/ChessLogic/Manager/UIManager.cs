using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WebSocketSharp;
public enum TextTag
{
    HighTemperture,
    LowTemperture,
    Warm
};
public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public GameObject prefabFloorHDU;
    public GameObject prefabArrowFloorHUD;
    public GameObject prefabHealthBar;
    public GameObject prefabFloatText;

    public GameObject prefabImage;
    public GameObject prefabMessage;
    
    public Gradient tempertureColorGradient;
    public UnityEvent eRefreshFloorHUD = new UnityEvent();
    public Transform panelContainer;
    public Canvas mainUICanvans;
    public SelectChessView selectChessView;
    public SkillDisplayView skillDisplayView;
    public MouseFollowText mouseFollowText; 

    protected List<GameObject> alivePanels = new List<GameObject>();
    protected Dictionary<IGetInfo,GameObject> extraPanels = new Dictionary<IGetInfo, GameObject>();
    protected Outline aliveOutline;
    protected GameObject overTileFloorHUD;
    public AudioClip overTileClip;
    [NonSerialized]
    private bool bShowTemperture = false;

    [SerializeField]
    public List<TextTagInfo> textTags;
    protected override void Awake()
    {
        base.Awake();
        PlayerControlManager.instance.eOverTile.AddListener(OverTile);
        PlayerControlManager.instance.eSelectChess.AddListener(selectChessView.Bind);
        PlayerControlManager.instance.eDeselect.AddListener(selectChessView.UnBind);
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

                int tempature = TempertureManager.instance.GetTempatureAt(new Vector2Int(i, j));
                tempature = Math.Max(tempature, -1);
                tempature = Math.Min(tempature, tempertureColorGradient.colorKeys.Length - 2);
                Color c = tempertureColorGradient.colorKeys[tempature + 1].color;
                t.render.material.SetColor("_Color", c);
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
    public void CreateFloatText(Vector3 position, TextTag tag)
    {
        TextTagInfo data= textTags[(int)tag];
        CreateFloatText(position, data.name, data.color);
    }
    public void CreateFloatText(Vector3 position, string text, Color color)
    {
        GameObject t = Instantiate(prefabFloatText, position, Quaternion.identity, null);
        t.GetComponentInChildren<TextMesh>().text = text;
        t.GetComponentInChildren<TextMesh>().color = color;
    }
    void OverTile(Vector2Int location)
    {
        if (overTileFloorHUD != null)
            Destroy(overTileFloorHUD);
        if (aliveOutline != null)
        {
            aliveOutline.RemoveReference();
            aliveOutline = null;
        }
        if (location != Vector2Int.down)
        {
            overTileFloorHUD = CreateFloorHUD(location, Color.yellow);
            GetComponent<AudioSource>().PlayOneShot(overTileClip, 0.04f);
        }

        GChess t = GridManager.instance.GetChess(location);
        GFloor f = GridManager.instance.GetFloor(location);
        var list = new List<IGetInfo>();
        if (t != null)
        {
            list.AddRange(t.GetInfos());
            aliveOutline = t.outline;
            aliveOutline.AddReference();
        }
        if (f != null)
        {
            list.AddRange(f.GetInfos());
        }
        list.AddRange(GridManager.instance.GetEnvironmentInformation(location));
        CreateMessage(list);
    }
    public void CreateMessage(List<IGetInfo> infos)
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

    }
    public void AddExtraMessage(IGetInfo info)
    {
        GameObject gameObject = GameObject.Instantiate(prefabMessage, panelContainer);
        extraPanels.Add(info,gameObject);
        gameObject.GetComponent<Messages>().Init(info);
    }
    public void RemoveExtraMessage(IGetInfo info)
    {
        Destroy(extraPanels[info]);
        extraPanels.Remove(info);
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
        GameObject gameObject = GameObject.Instantiate(prefabFloorHDU, GridManager.instance.GetChessPosition3D(location) , Quaternion.identity);
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        return gameObject;
    }
    public GameObject CreateArrowFloorHUD(Vector2Int location)
    {
        GameObject gameObject = GameObject.Instantiate(prefabArrowFloorHUD, GridManager.instance.GetFloorPosition3D(location) + new Vector3(0, 0.51f, 0), Quaternion.identity);
        return gameObject;
    }
    public GameObject CreateHealthBar(GChess chess)
    {

        GameObject gameObject = GameObject.Instantiate(prefabHealthBar, chess.render.transform);
        gameObject.GetComponent<GetImage>().Init(chess);

        return gameObject;
    }

    

    public string GetHighTempertureRichText()
    {
        return "<color=yellow>高温</color>";
    }
    public string GetLowTempertureRichText()
    {
        return "<color=yellow>低温</color>";
    }
}
