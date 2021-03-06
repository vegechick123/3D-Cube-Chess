﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    public float totalTime;
    protected float currentTime = 0.0f;
    [HideInInspector]
    public UnityEvent eHit=new UnityEvent();
    public bool finish = false;
    public VisualEffect visualEffect;
    public bool bProjectile=false;
    static float waitTime = 5f;
    private void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (!finish)
        {
            if (bProjectile)
            {
                
                if (currentTime >= totalTime)
                {
                    eHit.Invoke();
                    visualEffect.SendEvent("OnHit");
                    Stop();
                }
                visualEffect.SetFloat("t", currentTime / totalTime);
            }
        }
        else if(currentTime>waitTime&visualEffect.aliveParticleCount == 0)
        {
            Destroy(gameObject);
        }
    }
    public void Stop()
    {
        visualEffect.Stop();
        finish = true;
    }
    public void InitInstantProjectile(Vector3 origin, Vector3 target)
    {
        visualEffect.SetVector3("origin_position", origin);
        visualEffect.SetVector3("target_position", target);
        finish = true;
    }
    public void InitProjectile(Vector3 origin, Vector3 target, float speed)
    {
        bProjectile = true;
        visualEffect.SetVector3("origin_position",origin);
        visualEffect.SetVector3("target_position", target);
        visualEffect.SendEvent("OnShoot");
        totalTime = (target - origin).magnitude/speed;        
    }

}

