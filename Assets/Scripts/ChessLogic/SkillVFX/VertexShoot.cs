using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "VertexShoot", menuName = "SkillVFX/VertexShoot")]
public class VertexShoot : SkillVFX
{
    public GameObject bullet;

    public override void Cast(Vector3 origin, Vector3 destination)
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        GameObject clone = Instantiate(bullet, origin, rotation);
        clone.AddComponent<VertexBulletControl>();
        clone.GetComponent<VertexBulletControl>().prefabBeginParticle = prefabBeginParticle;
        clone.GetComponent<VertexBulletControl>().prefabEndParticle = prefabEndParticle;
        clone.GetComponent<VertexBulletControl>().origin = origin;
        clone.GetComponent<VertexBulletControl>().destination = destination;
        clone.GetComponent<VertexBulletControl>().eHit = eHit;
    }

}
