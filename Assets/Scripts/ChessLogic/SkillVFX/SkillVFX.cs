 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SkillVFX : ScriptableObject
{
    public GameObject prefabBeginParticle;
    public GameObject prefabEndParticle;
    [HideInInspector]
    public UnityEvent eHit;
    public virtual void Cast(Vector3 origin,Vector3 destination)
    {
   
    }
    public virtual void Cast(Vector2Int origin,Vector2Int destination)
    {
        Cast(GridManager.instance.GetChessPosition3D(origin) + new Vector3(0, 0.5f, 0), GridManager.instance.GetChessPosition3D(destination) + new Vector3(0, 0.5f, 0));
    }
}
