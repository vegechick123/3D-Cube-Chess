using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        
    }
    public void OnShoot()
    {
        GetComponentInParent<GChess>()?.AnimationShoot();
    }
    // Update is called once per frame
    public void Update()
    {
        
    }
}
