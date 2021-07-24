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
        battleCfg.mapWidth = 30;
        battleCfg.mapHeight = 30;
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
                if (mapGrid.GridType == GridType.None) continue;

                Tile tile = default;
                switch (mapGrid.GridType)
                {
                    case GridType.Normal: tile = Resources.Load("white") as Tile; ; break;
                    case GridType.Obstacle: tile = Resources.Load("black") as Tile; break;
                }
                tilemap.SetTile(mapGrid.GridPosVec3Int, tile);
            }
        }
        tilemap.transform.position = new Vector3(-battleMap.Width / 2, -battleMap.Height / 2, 0);
    }
}
