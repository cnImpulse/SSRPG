using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionRangeRenderer : MonoBehaviour
{
    private Tilemap tilemap;
    private TileBase streak;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        streak = Resources.Load<TileBase>("streak");
    }

    public void Refresh()
    {
        tilemap.ClearAllTiles();
        foreach (var grid in BattleMgr.Instance.canMoveGrids)
        {
            tilemap.SetTile(Utl.ToVec3Int(grid.Position), streak);
            tilemap.SetColor(Utl.ToVec3Int(grid.Position), Color.yellow);
        }
    }
}
