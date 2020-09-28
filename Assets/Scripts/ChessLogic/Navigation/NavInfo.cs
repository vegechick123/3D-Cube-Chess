using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 这是存储寻路信息的类
/// 你可以从中得到能到达的移动范围以及相应的路径
/// </summary>
public class NavInfo
{

    public Vector2Int[] range;
    private int[] prev;
    public NavInfo(Vector2Int[] coveredGround, int[] routinePrev) => (this.range, this.prev) = (coveredGround, routinePrev);
    public Vector2Int[] GetPath(Vector2Int destination)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        int index = Array.FindIndex(range, t => t == destination);
        if (index == -1) return null;
        queue.Enqueue(range[index]);
        while (prev[index] != -1)
        {
            index = prev[index];
            queue.Enqueue(range[index]);
        }
        Vector2Int[] res = queue.ToArray();
        Array.Reverse(res);
        return res;
    }
}
