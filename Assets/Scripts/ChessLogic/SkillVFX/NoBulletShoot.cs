using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NoBulletShoot", menuName = "SkillVFX/NoBulletShoot")]
public class NoBulletShoot : SkillVFX
{
    public override void Cast(Vector3 origin, Vector3 destination)
    {
        GameObject clone = Instantiate(prefabBeginParticle, origin, prefabBeginParticle.transform.rotation);
        GameObject end = Instantiate(prefabEndParticle, origin, prefabEndParticle.transform.rotation);
        eHit.Invoke();
    }
}