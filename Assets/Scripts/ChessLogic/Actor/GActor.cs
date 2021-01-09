using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    //TODO: 应当移动到ElementComponet的元素粒子效果
    public GameObject fireParticle;
    public AudioClip fireSoundEffect;
    public GameObject iceParticle;
    public AudioClip iceSoundEffect;


    public string title;
    public string info;
    
    virtual protected void Awake()
    {
        //注册事件
        GameManager.instance.eRoundStart.AddListener(OnRoundStart);
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
    //TODO 把更多逻辑放到ElementComponet里
    public virtual void ElementReaction(Element element)
    {
        if (elementComponent)
        {
            switch (element)
            {
                case Element.Fire:
                    CreateFloatTextOnHead(TextTag.HighTemperture);
                    if (fireParticle != null)
                    {
                        GridFunctionUtility.CreateParticleAt(fireParticle, this);
                    }
                    if(fireSoundEffect!=null)
                    {
                        AudioSource.PlayClipAtPoint(fireSoundEffect,transform.position,1f);
                    }
                    break;
                case Element.Ice:
                    CreateFloatTextOnHead(TextTag.LowTemperture);
                    if (iceParticle != null)
                    {
                        GridFunctionUtility.CreateParticleAt(iceParticle, this);
                    }
                    if (iceSoundEffect != null)
                    {
                        render.GetComponent<AudioSource>().PlayOneShot(iceSoundEffect, 0.3f);
                    }
                    break;
                    
                default:
                    break;
            }
            elementComponent.OnHitElement(element);
            eElementReaction.Invoke(element);
        }
    }
    public void CreateFloatTextOnHead(TextTag tag)
    {
        UIManager.instance.CreateFloatText(transform.position+Vector3.up, tag);
    }
    virtual protected void OnRoundStart()
    {

    }
    virtual public void OnPlayerTurnEnd()
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
    public virtual string GetTitle()
    {
        return title;
    }

    public virtual string GetInfo()
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
