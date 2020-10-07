using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ElementStateBase : ScriptableObject
{
    protected GActor owner;
    protected CElementComponent stateMachine;
    public GameObject prefabEnterParticle;
    public GameObject prefabExitParticle;
    public GameObject prefabPersistentParticle;
    [HideInInspector]
    public bool disableParticle;
    private GameObject persistentParticle;
    public virtual void Init(GActor owner, CElementComponent stateMachine)
    {
        this.owner = owner;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter()
    {
        if (prefabEnterParticle != null&&!disableParticle)
        {
            GridFunctionUtility.CreateParticleAt(prefabEnterParticle,owner);
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
            GridFunctionUtility.CreateParticleAt(prefabExitParticle, owner);
        }
        if (persistentParticle!= null)
        {
            persistentParticle.GetComponent<ParticleSystem>().Stop();
            Destroy(persistentParticle, 10);
            persistentParticle = null;
        }
    }
    public virtual int ProcessDamage(Element element,int damage)
    {
        return damage;
    }
}
