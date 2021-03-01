using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSystem : MonoBehaviour
{
    async public static UniTask ApplyElementAtAsync(Vector2Int location, Element element, int damage = 0)
    {
        await ApplyElementAtAsync(new Vector2Int[] { location }, element,damage);
    }
    async public static UniTask ApplyElementAtAsync(Vector2Int[] location, Element element, int damage = 0)
    {
        if (damage != 0)
            foreach (GChess target in GridManager.instance.GetChessesInRange(location))
            {
                target.Damage(damage);
            }
        if (element == Element.Thunder)
        {
            //List<GActor> targets = FindCuductive(location);
            //foreach (GActor target in targets)
            //{
            //    target.ElementReaction(Element.Thunder);
            //}
        }
        else if (element == Element.Fire)
        {
            List<GFloor> targets = FindCombustible(location);
            foreach (GFloor target in targets)
            {
                target.readyToBurst = true;
            }
            float lastTime = Time.time;
            foreach (GFloor target in targets)
            {
                Debug.Log(Time.time - lastTime);
                lastTime = Time.time;
                target.ElementReaction(Element.Fire);

                await UniTask.Delay(TimeSpan.FromSeconds(0.015f));

            }

        }
        else
        {
            foreach (GChess target in GridManager.instance.GetChessesInRange(location))
            {
                target.ElementReaction(element);
            };
            foreach (GFloor target in GridManager.instance.GetFloorsInRange(location))
            {
                target.ElementReaction(element);
            };
        }
    }
    public static List<GActor> FindCuductive(Vector2Int source)
    {
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        List<GActor> res = new List<GActor>();
        if (GridManager.instance.GetFloor(source)?.elementComponent.conductive == true)
        {
            queue.Enqueue(new Vector3Int(source.x, source.y, 0));
            res.Add(GridManager.instance.GetFloor(source));
        }
        if (GridManager.instance.GetChess(source)?.elementComponent.conductive == true)
        {
            queue.Enqueue(new Vector3Int(source.x, source.y, 1));
            res.Add(GridManager.instance.GetChess(source));
        }
        HashSet<Vector3Int> vis = new HashSet<Vector3Int>();

        Vector3Int[] dir = new Vector3Int[] { new Vector3Int(0, 1, 0), new Vector3Int(1, 0, 0), new Vector3Int(0, -1, 0), new Vector3Int(-1, 0, 0) };
        while (queue.Count != 0)
        {
            var node = queue.Dequeue();

            Vector3Int[] locs = new Vector3Int[5];
            locs[0] = new Vector3Int(node.x, node.y, node.z == 1 ? 0 : 1);
            for (int i = 0; i < 4; i++)
            {
                locs[i + 1] = node + dir[i];
            }
            foreach (Vector3Int loc in locs)
            {
                GActor t;

                if (loc.z == 0)
                {
                    t = GridManager.instance.GetFloor(new Vector2Int(loc.x, loc.y));
                    if (t?.elementComponent.conductive == true)
                        res.Add(t);
                    else
                        continue;
                }
                else
                {
                    t = GridManager.instance.GetChess(new Vector2Int(loc.x, loc.y));
                    if (t?.elementComponent.conductive == true)
                        res.Add(t);
                    else
                        continue;
                }
                if (vis.Contains(loc))
                    continue;
                else
                {
                    queue.Enqueue(loc);
                    vis.Add(loc);
                }
            }
        }
        return res;
    }
    public static List<GFloor> FindCombustible(Vector2Int[] source)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        List<GFloor> res = new List<GFloor>();
        foreach (Vector2Int loc in source)
        {
            res.Add(GridManager.instance.GetFloor(loc));
            if (GridManager.instance.GetFloor(loc)?.explosive==true&& GridManager.instance.GetFloor(loc)?.readyToBurst == false)
            {
                queue.Enqueue(new Vector2Int(loc.x, loc.y));
                
            }
        }
        HashSet<Vector2Int> vis = new HashSet<Vector2Int>();

        Vector2Int[] dir = new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0) };
        while (queue.Count != 0)
        {
            var node = queue.Dequeue();

            Vector2Int[] locs = new Vector2Int[4];
            locs[0] = new Vector2Int(node.x, node.y);
            for (int i = 0; i < 4; i++)
            {
                locs[i] = node + dir[i];
            }
            foreach (Vector2Int loc in locs)
            {
                GFloor t;
                t = GridManager.instance.GetFloor(new Vector2Int(loc.x, loc.y));
                if (t?.explosive == true && t?.readyToBurst == false)
                    res.Add(t);
                else
                    continue;

                if (vis.Contains(loc))
                    continue;
                else
                {
                    queue.Enqueue(loc);
                    vis.Add(loc);
                }
            }
        }
        return res;
    }
}
