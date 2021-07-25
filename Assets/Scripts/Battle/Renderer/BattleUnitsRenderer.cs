using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleUnitsRenderer : MonoBehaviour
{
    private List<BattleUnit> unitsData;
    private Tilemap tilemap;
    private Tile amity, enemy;

    public void Init(List<BattleUnit> units)
    {
        tilemap = GetComponent<Tilemap>();
        amity = Resources.Load("amity") as Tile;
        enemy = Resources.Load("enemy") as Tile;

        unitsData = units;
        Refresh();
    }

    private void Refresh()
    {
        if (unitsData == null) return;

        foreach(var unit in unitsData)
        {
            Tile tile = default;
            switch (unit.battleCamp)
            {
                case BattleCamp.Amity: tile = amity; break;
                case BattleCamp.Enemy: tile = enemy; break;
            }
            tilemap.SetTile(Utl.ToVec3Int(unit.position), tile);
        }
    }
}