using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Tilemaps;
using UnityEditor;

[CreateAssetMenu(fileName = "GActorBrush", menuName = "2D Extras/Brushes/GActor Brush", order = 359)]
[CustomGridBrush(false, true, false, "GActor Brush")]
public class GActorBrush : PrefabBrush
{
    /// <summary>
    /// Paints GameObject from containg Prefab into a given position within the selected layers.
    /// The PrefabBrush overrides this to provide Prefab painting functionality.
    /// </summary>
    /// <param name="grid">Grid used for layout.</param>
    /// <param name="brushTarget">Target of the paint operation. By default the currently selected GameObject.</param>
    /// <param name="position">The coordinates of the cell to paint data to.</param>
    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        // Do not allow editing palettes
        if (brushTarget.layer == 31 || brushTarget == null)
        {
            return;
        }

        var objectsInCell = GetObjectsInCell(grid, brushTarget.transform, position);
        var existPrefabObjectInCell = objectsInCell.Any(objectInCell => PrefabUtility.GetCorrespondingObjectFromSource(objectInCell) == m_Prefab);

        if (!existPrefabObjectInCell)
        {
            var instance =  base.InstantiatePrefabInCell(grid, brushTarget, position, m_Prefab);
            GActor actor = instance.GetComponent<GActor>();
            if(actor==null)
            {
                Debug.LogError("非GActor");
            }
            else
            {
                actor.location = new Vector2Int(position.x, position.y);
            }
        }

    }
}
