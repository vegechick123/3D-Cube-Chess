using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public Vector3 origin;
    protected Vector3 directionDelta;
    protected float h;

    public float totalTime;
    protected float currentTime = 0.0f;
    public UnityEvent eHit=new UnityEvent();
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= totalTime)
        {
            eHit.Invoke();
            Destroy(gameObject);
        }
        transform.position = GetTrajectory(currentTime / totalTime);
    }

    public Vector3 GetTrajectory(float t)
    {
        float delta = -t * (t - 1);
        return origin + directionDelta * t + Vector3.up * h * delta;
    }

    public void Init(Vector3 origin, Vector3 destination, float speed, float aspect)
    {
        this.origin = origin;
        directionDelta = (destination - origin);
        totalTime = directionDelta.magnitude / speed;
        h = aspect * directionDelta.magnitude;
    }
}

