using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : Component
{
    FloorHUD hud;
    Vector2Int targetLocation=Vector2Int.zero;
    private void Start()
    {
        hud = new FloorHUD(() =>
        {
            List<Vector2Int> res = GridManager.instance.GetRayRange(actor.location, targetLocation);
            if (res != null)
                return res.ToArray();
            else
                return new Vector2Int[0];
        }, Color.yellow);
        PlayerControlManager.instance.eOverTile.AddListener(ChangeHUD);
    }
    void ChangeHUD(Vector2Int destination)
    {
        targetLocation = destination;
        hud.Refresh();
    }
}
 