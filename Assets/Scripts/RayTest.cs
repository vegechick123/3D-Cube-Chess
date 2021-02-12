using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : Component
{
    FloorHUD hud;
    Vector2Int targetLocation;
    private void Start()
    {
        hud = new FloorHUD(() =>
        {
            return GridManager.GetLineRange(actor.location, targetLocation).ToArray();
        }, Color.yellow);
        PlayerControlManager.instance.eOverTile.AddListener(ChangeHUD);
    }
    void ChangeHUD(Vector2Int destination)
    {
        targetLocation = destination;
        hud.Refresh();
    }
}
 