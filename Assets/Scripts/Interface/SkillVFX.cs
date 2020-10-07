 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SkillVFX : ScriptableObject
{
    public GameObject prefabBeginParticle;
    public GameObject prefabEndParticle;
    public UnityEvent eHit;
    public void Cast(Vector3 origin,Vector3 destination)
    {
        
    }

}
