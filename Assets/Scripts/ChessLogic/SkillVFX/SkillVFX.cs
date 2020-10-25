 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SkillVFX : ScriptableObject
{
    public GameObject prefabBeginParticle;
    public GameObject prefabEndParticle;
    public AudioClip beginAudio;
    public AudioClip endAudio;
    [Range(0, 1f)]
    public float volume;
    [HideInInspector]
    public UnityEvent eHit;
    public virtual void Cast(Vector3 origin,Vector3 destination)
    {
        if (beginAudio != null)
            AudioSource.PlayClipAtPoint(beginAudio, origin, 0.3f);
        if (endAudio != null)
            eHit.AddListenerForOnce(() =>
            {
                AudioSource.PlayClipAtPoint(endAudio, destination);
            });

        
    }
    public virtual void Cast(Vector2Int origin,Vector2Int destination)
    {
        Cast(GridManager.instance.GetChessPosition3D(origin) + new Vector3(0, 0.5f, 0), GridManager.instance.GetChessPosition3D(destination) + new Vector3(0, 0.5f, 0));
    }
}
