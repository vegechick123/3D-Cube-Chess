using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBulletControl : BulletControl
{
    public override Vector3 Position(float t)
    {
        return finalSpeed * t + origin;
    }


    public override void SpeedCaculate()
    {
        Vector3 identityHor = (destination - origin).normalized;
        float distance = Vector3.Distance(origin, destination);
        timecost = distance / speed;
        Vector3 speedHor = identityHor * speed;
        finalSpeed = speedHor;
    }

}
