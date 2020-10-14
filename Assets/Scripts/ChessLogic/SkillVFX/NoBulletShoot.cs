using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "NoBulletlShoot", menuName = "SkillVFX/NoBulletlShoot")]
public class NoBulletlShoot : SkillVFX
{ 

    public override void  Cast(Vector3 origin, Vector3 destination)
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        GameObject clone = Instantiate(prefabBeginParticle, origin, rotation);
        GameObject end= Instantiate(prefabEndParticle, origin, rotation);
        eHit.Invoke();
    }

}
