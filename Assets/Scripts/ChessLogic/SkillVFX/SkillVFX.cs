using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[CreateAssetMenu]
public class SkillVFX : ScriptableObject
{
    public GameObject prefabBeginParticle;
    public GameObject prefabProjectileParticle;
    public bool instant;
    public float speed;
    private Vector3 origin { get { return GridManager.instance.GetChessPosition3DCenter(skill.owner.location); } }
    private Vector3 destination;
    private VFXController beginParticle;
    private Skill skill;
    VFXObserver vfXObserver = null;
    public void Init(Skill skill)
    {
        this.skill = skill;
        if (prefabBeginParticle != null)
        {
            skill.eBegin.AddListener(CreateBeginParticle);
            skill.eEnd.AddListener(() =>
            {
                beginParticle.Stop();
                beginParticle = null;
            });
        }
    }
    public void SetTarget(Vector2Int destination)
    {
        SetTarget(GridManager.instance.GetChessPosition3D(destination));
    }
    public void SetTarget(Vector3 destination)
    {
        this.destination = destination;
    }
    public void SetObserver(VFXObserver observer)
    {
        vfXObserver=observer;
    }
    public void CreateBeginParticle()
    {
        if (beginParticle)
        {
            Debug.LogError("已存在beginParticle");
        }
        if (prefabBeginParticle)
        {
            beginParticle = CreateVFXWithAutoDestory(prefabBeginParticle);
            beginParticle.visualEffect.SetVector3("origin_position", origin);
        }
    }
    public async UniTask CreateProjectileParticle()
    {
        if (prefabProjectileParticle)
        {
            GameObject go = Instantiate(prefabProjectileParticle);
            VFXController t = go.AddComponent<VFXController>();
            t.observer = vfXObserver;
            if (instant)
            {
                t.InitInstantProjectile(origin, destination);
            }
            else
            {
                var p = MyUniTaskExtensions.WaitUntilEvent(t.eHit);
                t.InitProjectile(origin, destination, speed);
                await p;
            }

        }
            
    }
    VFXController CreateVFXWithAutoDestory(GameObject prefab)
    {
        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        return go.AddComponent<VFXController>();
    }
}
public class VFXObserver
{
    public UnityEvent<int> eEnterTile= new EventWrapper<int>();
}
