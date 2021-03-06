﻿using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
[ExecuteInEditMode]
public class GridManager : SingletonMonoBehaviour<GridManager>
{
    public Vector2Int size;
    public Grid grid;
    public Transform chessContainer;
    [NonSerialized]
    public UnityEvent eGridChange = new UnityEvent();
    protected GFloor[,] floors;
    protected float floorYOffest = -0.5f;
    protected float chessYOffest = 0.5f;
    public List<GChess> chesses { get; protected set; }
    public List<GPlayerChess> playerChesses
    {
        get
        {
            return (chesses.FindAll((x) => x is GPlayerChess).Cast<GPlayerChess>().ToList());
        }
    }
    public List<GChest> chests
    {
        get
        {
            return (chesses.FindAll((x) => x is GChest).Cast<GChest>().ToList());
        }
    }
    public List<GAIChess> aiChesses
    {
        get
        {
            return (chesses.FindAll((x) => x is GAIChess).Cast<GAIChess>().ToList());
        }
    }
    protected override void Awake()
    {
        base.Awake();
        floors = new GFloor[size.x, size.y];
        chesses = new List<GChess>();
    }
    private void Start()
    {
        if (Application.isPlaying)
        {
            InitialSpawn();
            foreach (GChess t in FindObjectsOfType<GChess>())
            {
                AddChess(t);
            }
            foreach (GFloor t in FindObjectsOfType<GFloor>())
            {
                AddFloor(t);
            }
        }
    }
    /// <summary>
    /// 判断location是否在size的范围内
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
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
    /// <summary>
    /// 查询location位置上的chess
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public GChess GetChess(Vector2Int location)
    {
        return chesses.Find(x => location == x.location);
    }
    public GChess[] GetChesses(int teamID)
    {
        return chesses.FindAll(x => x.teamID == teamID).ToArray();
    }
    public GChess[] GetChesses()
    {
        return chesses.ToArray();
    }
    public GChess[] GetChessesInRange(Vector2Int[] range)
    {
        return chesses.FindAll(x => range.Contains(x.location)).ToArray();
    }
    public List<GFloor> GetFloorsInRange(Vector2Int[] range)
    {
        List<GFloor> res = new List<GFloor>();
        foreach (Vector2Int location in range)
        {
            res.Add(GetFloor(location));
        }
        return res;
    }
    public List<GFloor> GetAllFloors()
    {
        List<GFloor> result = new List<GFloor>();
        for (int i = 0; i < size.x; i++)
            for (int j = 0; j < size.y; j++)
            {
                GFloor target = GetFloor(new Vector2Int(i, j));
                if (target != null)
                    result.Add(target);
            }
        return result;
    }
    public List<GFloor> GetAdjacentFloor(Vector2Int location)
    {
        List<GFloor> result = new List<GFloor>();
        Vector2Int[] dir = new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0) };
        for (int i = 0; i < 4; i++)
        {
            Vector2Int curLocation = location + dir[i];
            GFloor target = GetFloor(curLocation);
            if (target != null)
                result.Add(target);
        }
        return result;
    }
    public void AddChess(GChess chess)
    {
        chesses.Add(chess);
        chess.GAwake();
        eGridChange.Invoke();
    }
    public void RemoveChess(GChess chess)
    {
        chesses.Remove(chess);
        chess.GEnd();
        eGridChange.Invoke();
    }
    /// <summary>
    /// 如果查询位置超出size大小则会数组越界
    ///如果该位置没有GFloor则返回null
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public GFloor GetFloor(Vector2Int location)
    {
        if (!InRange(location))
        {
            return null;
        }
        return floors[location.x, location.y];
    }
    public void AddFloor(GFloor floor)
    {
        if (floors[floor.location.x, floor.location.y] != null)
        {
            Debug.LogError("同一位置多个Floor");
        }
        floors[floor.location.x, floor.location.y] = floor;
        floor.GAwake();
    }
    public void RemoveFloor(GFloor floor)
    {
        if (floors[floor.location.x, floor.location.y] != floor)
        {
            Debug.LogError("移除错误的Floor");
        }
        floors[floor.location.x, floor.location.y] = null;
        floor.GEnd();
    }
    public Vector3 GetChessPosition3D(Vector2Int location)
    {
        return grid.GetCellCenterWorld(new Vector3Int(location.x, location.y, 0)) + new Vector3(0, chessYOffest, 0);
    }
    public Vector3 GetChessPosition3DCenter(Vector2Int location)
    {
        return GetChessPosition3D(location) + new Vector3(0, grid.cellSize.y / 2, 0);
    }
    public bool CheckTransitability(Vector2Int location)
    {
        if (!InRange(location))
            return false;
        if (GetChess(location) || !GetFloor(location).transitable)
        {
            return false;
        }
        return true;
    }
    public Vector3 GetFloorPosition3D(Vector2Int location)
    {
        return grid.GetCellCenterWorld(new Vector3Int(location.x, location.y, 0)) + new Vector3(0, floorYOffest, 0);
    }
    public NavInfo GetNavInfo(Vector2Int location, int movement, int teamID = -1)
    {
        Queue<ValueTuple<UnityEngine.Vector2Int, int, int>> queue = new Queue<ValueTuple<UnityEngine.Vector2Int, int, int>>();
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        Queue<int> prev = new Queue<int>();
        Queue<bool> occupy = new Queue<bool>();
        Vector2Int[] dir = new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };


        queue.Enqueue((location, movement, 0));
        res.Enqueue(location);
        prev.Enqueue(-1);
        occupy.Enqueue(true);

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
                if (vis.Contains(loc))
                    continue;
                else
                    vis.Add(loc);

                GFloor floor = GetFloor(loc);
                // GChess chess = GetChess(loc);
                if (!floor || !CheckTransitability(loc))
                {
                    continue;
                }
                else
                {
                    queue.Enqueue((loc, node.Item2 - 1, res.Count));
                    res.Enqueue(loc);
                    prev.Enqueue(node.Item3);
                    occupy.Enqueue(false);
                }
            }
        }
        return new NavInfo(res.ToArray(), prev.ToArray(), occupy.ToArray());
    }
    public Vector2Int[] GetCircleRange(Vector2Int origin, int radius)
    {
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        Vector2Int[] dir = new Vector2Int[4];
        dir[0] = new Vector2Int(1, 1);
        dir[1] = new Vector2Int(1, -1);
        dir[2] = new Vector2Int(-1, 1);
        dir[3] = new Vector2Int(-1, -1);
        for (int x = 0; x <= radius; x++)
            for (int y = 0; x + y <= radius; y++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (x == 0 && y == 0 && i >= 1)
                    {
                        break;
                    }
                    if (x == 0 && i >= 2)
                    {
                        break;
                    }
                    if (y == 0 && (i == 1 || i == 3))
                    {
                        continue;
                    }

                    Vector2Int loc = new Vector2Int(x, y) * dir[i] + origin;
                    if (InRange(loc))
                        res.Enqueue(loc);
                }
            }
        return res.ToArray();
    }
    public Vector2Int[] GetFourRayRange(Vector2Int origin, int maxLength, int beginDistance = 1)
    {
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        Vector2Int[] dir = new Vector2Int[4];
        dir[0] = new Vector2Int(1, 0);
        dir[1] = new Vector2Int(-1, 0);
        dir[2] = new Vector2Int(0, 1);
        dir[3] = new Vector2Int(0, -1);
        for (int i = 0; i < 4; i++)
        {
            Vector2Int[] temp = GetOneRayRange(origin, dir[i], maxLength, beginDistance);
            foreach (Vector2Int t in temp)
            {
                res.Enqueue(t);
            }
        }
        return res.ToArray();
    }
    public Vector2Int[] GetRangeWithRangeType(Vector2Int origin, int maxLength, SkillRangeType rangeType, int beginDistance = 1)
    {
        switch (rangeType)
        {
            case SkillRangeType.Circle:
                return GetCircleRange(origin, maxLength);
            case SkillRangeType.Square:
                return null;
            case SkillRangeType.FourRay:
                return GetFourRayRange(origin, maxLength, beginDistance);
        }
        return null;
    }
    public Vector2Int[] GetOneRayRange(Vector2Int origin, Vector2Int dir, int maxLength = -1, int beginDistance = 1, bool stopWhenMeetChess = true)
    {
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        for (int d = 1; d <= maxLength || maxLength == -1; d++)
        {
            Vector2Int nowpos = d * dir + origin;
            GChess t = GetChess(nowpos);
            if (!InRange(nowpos) || (stopWhenMeetChess && t != null))
            {
                if (t != null && d >= beginDistance)
                {
                    res.Enqueue(nowpos);
                }
                break;
            }
            else
                res.Enqueue(nowpos);
        }
        return res.ToArray();
    }
    public Vector2Int[] GetOneLineRange(Vector2Int origin, Vector2Int dir, int maxLength = -1, int beginDistance = 1)
    {
        return GetOneRayRange(origin, dir, maxLength, 1, false);
    }
    public GChess InstansiateChessAt(GameObject prefab, Vector2Int location)
    {
        var res = Instantiate(prefab, chessContainer);
        res.transform.position = GetChessPosition3D(location);
        GChess chess = res.GetComponent<GChess>();
        chess.location = location;
        if (chess is GPlayerChess)
        {
            GPlayerChess playerChess = chess as GPlayerChess;
            GameObject originPrefab = prefab.GetComponent<GPlayerChess>().prefabPrototype;
            if (originPrefab == null)
                originPrefab = prefab;
            playerChess.prefabPrototype = originPrefab;
        }
        AddChess(chess);
        chess.render.GetComponent<Animator>().Play("Birth");
        return chess;
    }

    public List<IGetInfo> GetEnvironmentInformation(Vector2Int location)
    {

        List<IGetInfo> list = new List<IGetInfo>();
        if (location == Vector2Int.down)
            return list;
        int t = TempertureManager.instance.GetTempatureAt(location);
        string info = string.Empty;
        info += "回合结束时，";
        if (t > 0)
        {
            info += "留在这里的角色会受到";
            if (t >= 2)
            {
                info += t;
                info += "点";
            }
            info += UIManager.instance.GetHighTempertureRichText();
        }
        else if (t == 0)
        {
            info = "温度正好";
        }
        else
        {
            info += "留在这里的角色会受到" + UIManager.instance.GetLowTempertureRichText();
        }
        list.Add(new Information("区域温度：" + t, info));
        return list;
    }
    static public List<Vector2Int> GetLineRange(Vector2Int origin, Vector2Int target, bool containOrigin = false)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        //if (containOrigin) result.Add(origin);

        Vector2Int delta = target - origin;
        Vector2Int deltaSign = new Vector2Int(Math.Sign(delta.x), Math.Sign(delta.y));
        Vector2Int absDelta = new Vector2Int(Math.Abs(delta.x), Math.Abs(delta.y));
        bool swapXY = false;
        if (absDelta.y > absDelta.x)
        {
            swapXY = true;
            absDelta = absDelta.SwapXY();
        }
        int d = -absDelta.y + absDelta.x;//这里的d为实际上d的两倍
        int y = 0;
        for (int x = 1; x <= absDelta.x; ++x)
        {
            if (d != 0)
            {
                if (!swapXY)
                    result.Add(origin + new Vector2Int(x, y) * deltaSign);
                else
                    result.Add(origin + new Vector2Int(x, y).SwapXY() * deltaSign);
            }
            d += -2 * absDelta.y;
            if (d <= 0)
            {
                if (d != 0)
                {
                    if (!swapXY)
                        result.Add(origin + new Vector2Int(x, y + 1) * deltaSign);
                    else
                        result.Add(origin + new Vector2Int(x, y + 1).SwapXY() * deltaSign);
                }
                d += 2 * absDelta.x;
                ++y;
            }
        }
        return result;
    }
    public List<Vector2Int> GetRayRange(Vector2Int origin, Vector2Int target, bool containOrigin = false)
    {
        List<Vector2Int> result = new List<Vector2Int>(); ;
        foreach (Vector2Int location in GetLineRange(origin, target, containOrigin))
        {

            result.Add(location);
            if (GetChess(location))
            {
                return result;
            }
        }
        return result;
    }
    public void ChessEnterLocation(GChess chess)
    {
        GetFloor(chess.location).OnChessEnter(chess);
        eGridChange.Invoke();
    }
    public void InitialSpawn()
    {
        GFixSpawner[] fixSpawners = FindObjectsOfType<GFixSpawner>();
        GDataSpawner[] dataSpawners = FindObjectsOfType<GDataSpawner>();
        foreach (GFixSpawner t in fixSpawners)
            t.SpawnFixProtype();
        if(!SaveLoadManager.instance)
        {
            Debug.LogWarning("无存储机制运行");
        }
        else
        {
            SaveData data = SaveLoadManager.instance.currentData;
            for (int i = 0; i < data.playerChessDatas.Count; i++)
                dataSpawners[i].Spawn(data.playerChessDatas[i]);
        }
        
    }
}
