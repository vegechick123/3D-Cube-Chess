using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "HorizontalShoot", menuName = "SkillVFX/HorizontalShoot")]
public class HorizontalShoot : SkillVFX
{
    public GameObject bullet;

    public override void Cast(Vector3 origin, Vector3 destination)
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        GameObject clone = Instantiate(bullet, origin, bullet.transform.rotation);
        clone.AddComponent<HorizontalBulletControl>();
        clone.GetComponent<HorizontalBulletControl>().prefabBeginParticle = prefabBeginParticle;
        clone.GetComponent<HorizontalBulletControl>().prefabEndParticle = prefabEndParticle;
        clone.GetComponent<HorizontalBulletControl>().origin = origin;
        clone.GetComponent<HorizontalBulletControl>().destination = destination;
        clone.GetComponent<HorizontalBulletControl>().eHit = eHit;
    }

}
