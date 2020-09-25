using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Manager<GridManager>
{
    public Vector2Int size;
    public Grid grid;
    protected GFloor[,] floors;
    protected List<GChess> chesses;
    protected override void Awake()
    {
        base.Awake();
        floors = new GFloor[size.x, size.y];
        chesses = new List<GChess>();
    }
    public bool InRange(Vector2Int location)
    {
        if (0 <= location.x &&
            location.x < size.x &&
            0 <= location.y &&
            location.y < size.y)
            return true;
        else
            return false;
    }
    public GChess GetChess(Vector2Int location)
    {
        return chesses.Find(x => location == x.location); 
    }
    public void AddChess(GChess chess)
    {
        chesses.Add(chess);
    }
    public void RemoveChess(GChess chess)
    {
        chesses.Remove(chess);
    }
    //如果查询位置超出size大小则会数组越界
    //如果该位置没有GFloor则返回null
    public GFloor GetFloor(Vector2Int location)
    {
        if(!InRange(location))
        {
            return null;
        }
        return floors[location.x,location.y];
    }
    public void AddFloor(GFloor floor)
    {
        if(floors[floor.location.x, floor.location.y]!=null)
        {
            Debug.LogError("同一位置多个Floor");
        }
        floors[floor.location.x, floor.location.y]=floor;
    }
    public void RemoveFloor(GFloor floor)
    {
        if (floors[floor.location.x, floor.location.y] != floor)
        {
            Debug.LogError("移除错误的Floor");
        }
        floors[floor.location.x, floor.location.y] = null;
    }
    public Vector3 GetChessPosition3D(Vector2Int location)
    {
        return grid.GetCellCenterWorld(new Vector3Int(location.x, location.y, 0)) +new Vector3(0, 0.5f, 0);
    }
    public Vector3 GetFloorPosition3D(Vector2Int location)
    {
        return grid.GetCellCenterWorld(new Vector3Int(location.x, location.y,0))- new Vector3(0, 0.5f, 0);
    }
    public NavInfo GetNavInfo(Vector2Int location,int movement)
    {
        Queue<ValueTuple<UnityEngine.Vector2Int, int, int>> queue = new Queue<ValueTuple<UnityEngine.Vector2Int, int, int>>();
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        Queue<int> prev = new Queue<int>();
        Vector2Int[] dir = new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };


        queue.Enqueue((location, movement, 0));
        res.Enqueue(location);
        prev.Enqueue(-1);


        HashSet<Vector2Int> vis = new HashSet<Vector2Int>();
        vis.Add(location);
        while (queue.Count != 0)
        {
            var node = queue.Dequeue();
            if (node.Item2 <= 0)
                continue;
            foreach (Vector2Int curDir in dir)
            {
                Vector2Int loc = node.Item1 + curDir;
                GFloor floor = GetFloor(loc);
                if (!floor || vis.Contains(loc) || loc == location)
                {
                    continue;
                }
                else
                {
                    vis.Add(loc);
                    queue.Enqueue((loc, node.Item2 - 1, res.Count));
                    res.Enqueue(loc);
                    prev.Enqueue(node.Item3);
                }
            }
        }
        return new NavInfo(res.ToArray(), prev.ToArray());
    }

}
