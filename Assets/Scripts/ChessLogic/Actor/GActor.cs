using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public abstract class GActor : MonoBehaviour, IGetInfo
{
    /// <summary>
    /// 表示Actor在网格中的位置
    /// 请勿直接更改
    /// 使用GChes中的MoveTo或MoveToDirectly来修改
    /// </summary>
    public Vector2Int location;
    [HideInInspector]
    public MeshRenderer render;
    [HideInInspector]
    public MeshFilter meshFilter;
    [HideInInspector]
    public Material originMaterial;
    [HideInInspector]
    public CElementComponent elementComponent;
    public UnityEvent<Element> eElementReaction = new EventWrapper<Element>();
    public GameObject fireParticle;
    public GameObject iceParticle;
    public string title;
    public string info;
    
    virtual protected void Awake()
    {
        //注册事件
        GameManager.instance.eRoundStart.AddListener(OnRoundStart);
        GameManager.instance.eRoundEnd.AddListener(OnRoundEnd);
        //GameManager.instance.ePlayerTurnEnd.AddListener(OnPlayerTurnEnd);
        GameManager.instance.eGameAwake.AddListener(OnGameAwake);
        GameManager.instance.eGameStart.AddListener(OnGameStart);
        GameManager.instance.eGameEnd.AddListener(OnGameEnd);
        render = GetComponent<MeshRenderer>();
        if (render == null)
        {
            render = GetComponentInChildren<MeshRenderer>();

        }
        meshFilter = render.GetComponent<MeshFilter>();

        elementComponent = GetComponent<CElementComponent>();

        originMaterial = render.material;
    }
    public virtual void ElementReaction(Element element)
    {
        if (elementComponent)
        {
            switch (element)
            {
                case Element.Fire:
                    CreateTextOnHead(TextTag.HighTemperture);
                    if (fireParticle != null)
                    {
                        GridFunctionUtility.CreateParticleAt(fireParticle, this);
                    }
                    break;
                case Element.Ice:
                    CreateTextOnHead(TextTag.LowTemperture);
                    if (iceParticle != null)
                    {
                        GridFunctionUtility.CreateParticleAt(iceParticle, this);
                    }
                    break;
                    
                default:
                    break;
            }
            elementComponent.OnHitElement(element);
            eElementReaction.Invoke(element);
        }
    }
    public void CreateTextOnHead(TextTag tag)
    {
        UIManager.instance.CreateFloatText(transform.position+Vector3.up, tag);
    }
    virtual protected void OnRoundStart()
    {

    }
    virtual public void OnPlayerTurnEnd()
    {

    }
    virtual protected void OnRoundEnd()
    {

    }
    virtual public void OnGameAwake()
    {
        var arr = GetComponents<Component>();
        foreach (Component c in arr)
        {
            c.OnGameAwake();
        }

    }
    virtual public void OnGameStart()
    {
        var arr = GetComponents<Component>();
        foreach (Component c in arr)
        {
            c.OnGameStart();
        }

    }
    virtual protected void OnGameEnd()
    {

    }
    virtual public List<IGetInfo> GetInfos()
    {
        List<IGetInfo> result = new List<IGetInfo>();
        result.AddRange(GetComponents<CExtraInformation>());
        return result;
    }
    public string GetTitle()
    {
        return title;
    }

    public string GetInfo()
    {
        return info;
    }
    public void OnValidate()
    {
        if (GridManager.instance != null)
        {
            Vector3 position = GridManager.instance.GetFloorPosition3D(location);
            transform.position = new Vector3(position.x, transform.position.y, position.z);
        }

    }
}
