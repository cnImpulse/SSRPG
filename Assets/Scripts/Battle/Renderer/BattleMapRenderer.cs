using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleMapRenderer : MonoBehaviour
{
    private BattleMap battleMap;
    private Tilemap tilemap;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();

        BattleCfg battleCfg;
        battleCfg.mapWidth = 18;
        battleCfg.mapHeight = 10;
        BattleMgr.Instance.CreatBattle(battleCfg);
        battleMap = BattleMgr.Instance.battleMap;

        RefreshBattleMap();
    }

    public void RefreshBattleMap()
    {
        if (battleMap == null) return;

        for (int row = 0; row < battleMap.Height; row++)
        {
            for (int col = 0; col < battleMap.Width; ++col)
            {
                MapGrid mapGrid = battleMap.mapGrids[row, col];
                tilemap.SetTile(mapGrid.GridPosVec3Int, Resources.Load("grid") as Tile);
            }
        }
    }
}
