using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu]
public class SkillVFX : ScriptableObject
{
    public GameObject prefabBeginParticle;
    public GameObject prefabShootParticle;
    public float speed;
    public float aspect;
    public GameObject prefabProjectileParticle;
    public GameObject prefabHitParticle;

    private Vector3 origin { get { return GridManager.instance.GetChessPosition3D(skill.owner.location); }}
    private Vector3 destination;
    private GameObject beginParticle;
    private Skill skill;
    public void Init(Skill skill)
    {
        this.skill = skill;
        skill.eBegin.AddListener(CreateBeginParticle);
        skill.eEnd.AddListener(()=>Destroy(beginParticle));        
    }
    public void SetTarget(Vector2Int destination)
    {
        SetTarget(GridManager.instance.GetChessPosition3D(destination));
    }
    public void SetTarget(Vector3 destination)
    {
        this.destination = destination;
    }
    public void CreateBeginParticle()
    {
        if (beginParticle)
        {
            Debug.LogWarning("重复创建粒子特效");
            Destroy(beginParticle);
        }
        if(prefabBeginParticle)
            beginParticle = Instantiate(prefabBeginParticle, origin, Quaternion.identity);
    }
    public void CreateShootParticle()
    {
        if (prefabShootParticle)
            Instantiate(prefabShootParticle, origin, Quaternion.identity);
    }
    public async UniTask CreateProjectileParticle()
    {
        if (prefabProjectileParticle)
        {
            GameObject go = Instantiate(prefabProjectileParticle);
            Projectile t = go.AddComponent<Projectile>();
            var p = MyUniTaskExtensions.WaitUntilEvent(t.eHit);
            t.Init(origin, destination, speed, aspect);
            await p;
        }
    }
    public void CreateHitParticle()
    {
        if (prefabHitParticle)
            Instantiate(prefabHitParticle, destination, Quaternion.identity);
    }
    
}
