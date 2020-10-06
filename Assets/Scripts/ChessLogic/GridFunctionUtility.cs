using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class GridFunctionUtility
{/// <summary>
/// 判断location是否在range范围内
/// </summary>
/// <param name="range"></param>
/// <param name="location"></param>
/// <returns></returns>
    public static bool InRange(this Vector2Int[] range, Vector2Int location)
    {
        foreach(var p in range)
        {
            if (p == location)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="range"></param>
    /// <param name="location"></param>
    /// <param name="bias">range整体偏移量</param>
    /// <returns></returns>
    public static bool InRange(this Vector2Int[] range, Vector2Int location,Vector2Int bias)
    {
        location -= bias;
        foreach (var p in range)
        {
            if (p == location)
                return true;
        }
        return false;
    }

    public static void DestoryAll(this GameObject[] gameObjects)
    {
        foreach (var t in gameObjects)
        {
            GameObject.Destroy(t);
        }
    }
    public static IEnumerator InvokeAfter(Action task, float time)
    {
        yield return new WaitForSeconds(time);
        task();

    }
    public static GameObject CreateParticleAt(GameObject prefab,GActor actor)
    {
        return GameObject.Instantiate(prefab, actor.transform.position, prefab.transform.rotation, null);
    }
}

