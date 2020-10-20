using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VerticalBulletControl : BulletControl
{
    public override Vector3 Position(float t)
    {
        return finalSpeed * t + origin + gravity * t * t * Vector3.down / 2;
    }


    public override void SpeedCaculate()
    {
        Vector3 identityHor = (destination - origin).normalized;
        Vector3 speedHor = identityHor * speed;
        Vector3 speedVer = speed / tangent * Vector3.down;

        timecost = (destination - origin).magnitude / speed;
        gravity = 2 * speed / timecost / tangent;

        finalSpeed = speedHor - speedVer;
    }
}
