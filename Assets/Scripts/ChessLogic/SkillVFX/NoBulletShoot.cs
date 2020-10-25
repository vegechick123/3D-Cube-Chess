using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NoBulletShoot", menuName = "SkillVFX/NoBulletShoot")]
public class NoBulletShoot : SkillVFX
{
    public float hitTime;
    public override void Cast(Vector3 origin, Vector3 destination)
    {
        base.Cast(origin,destination);
        GameObject clone = Instantiate(prefabBeginParticle, origin, prefabBeginParticle.transform.rotation);
        eHit.AddListenerForOnce(()=>Instantiate(prefabEndParticle, destination, prefabEndParticle.transform.rotation));
        GameManager.instance.InvokeAfter(eHit.Invoke,hitTime);
    }
}