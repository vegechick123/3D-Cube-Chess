using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : SingletonMonoBehaviour<EnvironmentManager>
{
    
    public int stormNum;
    public GameObject prefabStorm;
    public AudioClip stormAudio;
    public AudioClip stormTakeEffectAudio;
    [Range(0, 1)]
    public float stormAudioAmplitude=0.5f;
    public int stormAroundDistance=1;
    List<Vector2Int> stormLocation = new List<Vector2Int>();
    List<GameObject> stormParticle = new List<GameObject>();
    public void PreTurn()
    {
        StartCoroutine(PreEnvironmentTurn());
    }
    public void PostTurn()
    {
        StartCoroutine(PostEnvironmentTurn());
    }
    IEnumerator PreEnvironmentTurn()
    {
        if (stormAudio && stormNum > 0)
            GetComponent<AudioSource>().PlayOneShot(stormAudio, stormAudioAmplitude);
        Vector2Int yRange = GridExtensions.GetPlayerChessyRange();
        yRange.x = Mathf.Max(0, yRange.x - stormAroundDistance);
        yRange.y = Mathf.Min(GridManager.instance.size.y - 1, yRange.y + stormAroundDistance);
        for (int i = 0; i < stormNum; i++)
        {
            Vector2Int location;
            do
            {
                 location= GridExtensions.GetRandomLocation(yRange);
            }
            while (stormLocation.Contains(location));
            stormLocation.Add(location);
        }
        foreach (var location in stormLocation)
        {
            GameObject t = GridExtensions.CreateParticleAt(prefabStorm, location);
            Material mat = t.GetComponent<MeshRenderer>().material;
            Color color = mat.GetColor("_Color");
            mat.SetColor("_Color", Color.yellow);
            stormParticle.Add(t);
            yield return new WaitForSeconds(1f);
            mat.SetColor("_Color", color);
        }
    }
    IEnumerator PostEnvironmentTurn()
    {

        for (int i = 0; i < stormLocation.Count; i++)
        {
            Vector2Int location = stormLocation[i];
            GChess t=GridManager.instance.GetChess(location);
            if(t!=null)
            {
                t.ElementReaction(Element.Ice);
            }
            if(stormTakeEffectAudio!=null)
                AudioSource.PlayClipAtPoint(stormTakeEffectAudio, GridManager.instance.GetChessPosition3D(location));
            stormParticle[i].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.yellow);
            Destroy(stormParticle[i],1f);
            yield return new WaitForSeconds(1f);
        }
        stormLocation.Clear();
        stormParticle.Clear();
        //GameManager.instance.EnvironmentPostTurnEnd();
    }
    public List<IGetInfo> GetInfos(Vector2Int location)
    {
        List<IGetInfo> list= new List<IGetInfo>();
        if(stormLocation.Contains(location))
        {
            list.Add(new Information("冰风暴","回合结束时,留在这里的<color=yellow>任何</color>角色都会受到"+ UIManager.instance.GetLowTempertureRichText()));
        }
        return list;
    }
    
}
