using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ElementStateBase : ScriptableObject
{
    protected GActor owner;
    protected CElementComponent stateMachine;
    //爆发性粒子，默认会在10秒后销毁
    public GameObject prefabEnterParticle;
    public GameObject prefabExitParticle;
    public GameObject prefabPersistentParticle;

    private GameObject persistentParticle;
    public virtual void Init(GActor owner, CElementComponent stateMachine)
    {
        this.owner = owner;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter()
    {
        if (prefabEnterParticle != null)
        {
            var t =GridFunctionUtility.CreateParticleAt(prefabEnterParticle,owner);
        }
        if (prefabPersistentParticle != null)
        {
            persistentParticle = GridFunctionUtility.CreateParticleAt(prefabPersistentParticle, owner);
            persistentParticle.transform.parent = owner.render.transform;
        }
    }
    public virtual void OnHitElement(Element element)
    {

    }
    public virtual void Exit()
    {
        if (prefabExitParticle != null)
        {
            var t = GridFunctionUtility.CreateParticleAt(prefabExitParticle, owner);
        }
        if (persistentParticle!= null)
        {
            persistentParticle.GetComponent<ParticleSystem>().Stop();
            Destroy(persistentParticle, 10);
            persistentParticle = null;
        }
    }

}
