using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BulletShoot", menuName = "SkillVFX/BulletShoot")]
public class BulletShoot : SkillVFX
{
    public GameObject bullet;
    public float speed;
    public bool isVertical;
    public float tangent;

    public override void Cast(Vector3 origin, Vector3 destination)
    {
        if (isVertical)
        {
            GameObject clone = Instantiate(bullet, origin, bullet.transform.rotation);
            clone.AddComponent<VerticalBulletControl>();
            clone.GetComponent<VerticalBulletControl>().prefabBeginParticle = prefabBeginParticle;
            clone.GetComponent<VerticalBulletControl>().prefabEndParticle = prefabEndParticle;
            clone.GetComponent<VerticalBulletControl>().origin = origin;
            clone.GetComponent<VerticalBulletControl>().destination = destination;
            clone.GetComponent<VerticalBulletControl>().speed = speed;
            clone.GetComponent<VerticalBulletControl>().tangent = tangent;
            clone.GetComponent<VerticalBulletControl>().eHit = eHit;
        }
        else
        {
            GameObject clone = Instantiate(bullet, origin, bullet.transform.rotation);
            clone.AddComponent<HorizontalBulletControl>();
            clone.GetComponent<HorizontalBulletControl>().prefabBeginParticle = prefabBeginParticle;
            clone.GetComponent<HorizontalBulletControl>().prefabEndParticle = prefabEndParticle;
            clone.GetComponent<HorizontalBulletControl>().origin = origin;
            clone.GetComponent<HorizontalBulletControl>().destination = destination;
            clone.GetComponent<HorizontalBulletControl>().speed = speed;
            clone.GetComponent<HorizontalBulletControl>().tangent = tangent;
            clone.GetComponent<HorizontalBulletControl>().eHit = eHit;
        }
    }
}
