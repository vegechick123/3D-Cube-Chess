using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "NoBulletShoot", menuName = "SkillVFX/NoBulletShoot")]
public class NoBulletShoot : SkillVFX
{
    public float time = 0.1f;
    public override void  Cast(Vector3 origin, Vector3 destination)
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        GameObject clone = Instantiate(prefabBeginParticle, origin, rotation);
        GameObject end= Instantiate(prefabEndParticle, origin, rotation);
        GameManager.instance.InvokeAfter(eHit.Invoke,time);
    }

}
