using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletControl : MonoBehaviour
{
    public GameObject prefabBeginParticle;
    public GameObject prefabEndParticle;
    public Vector3 origin;
    public Vector3 destination;
    public float speed;
    public float tangent;

    protected Vector3 finalSpeed;
    protected float gravity;
    protected float timecost;
    protected float currentTime = 0.0f;
    public UnityEvent eHit;


    // Start is called before the first frame update
    void Start()
    {
        SpeedCaculate();
        if(prefabBeginParticle)
            Instantiate(prefabBeginParticle, origin, prefabBeginParticle.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timecost)
        {
            if (prefabEndParticle)
                Instantiate(prefabEndParticle, destination, prefabEndParticle.transform.rotation);
            eHit.Invoke();
            gameObject.GetComponent<ParticleSystem>().Stop();
            Destroy(this);
        }
        transform.position = Position(currentTime);
    }

    public virtual Vector3 Position(float t)
    {
        return destination;
    }

    public virtual void SpeedCaculate() { }
}

