using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ActMod
{
    None,
    Move,
    Atk,
}

public class ActRangeRenderer : MonoBehaviour
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
        ActMod mod = BattleMgr.Instance.actMod;
        if (mod == ActMod.None) return;
        tilemap.ClearAllTiles();

        Color color = Color.white;
        List<MapGrid> grids = null;
        switch (mod)
        {
            case ActMod.Move: {
                color = Color.yellow;
                grids = BattleMgr.Instance.canMoveGrids;
                break;
            }
            case ActMod.Atk: {
                color = Color.red;
                grids = BattleMgr.Instance.canAtkGrids;
                break;
            }
        }
        foreach (var grid in grids)
        {
            tilemap.SetTile(Utl.ToVec3Int(grid.Position), streak);
            tilemap.SetColor(Utl.ToVec3Int(grid.Position), color);
        }
    }
}
