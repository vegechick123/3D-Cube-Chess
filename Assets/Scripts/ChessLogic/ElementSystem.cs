using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSystem : MonoBehaviour
{
    public static void ApplyElementAt(Vector2Int location, Element element)
    {
        if (element == Element.Thunder)
        {
            List<GActor> targets = FindCuductive(location);
            foreach(GActor target in targets)
            {
                target.ElementReaction(Element.Thunder);
            }
        }
        else
        {
            GridManager.instance.GetChess(location)?.ElementReaction(element);
            GridManager.instance.GetFloor(location)?.ElementReaction(element);
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
}
