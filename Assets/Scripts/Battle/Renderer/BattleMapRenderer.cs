using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleMapRenderer : MonoBehaviour
{
    private BattleMap mapData;
    private Tilemap tilemap;
    private Tile normal, obstacle;

    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0, 0, 180, 100), "Next"))
    //    {
    //        GetBattleMapData();
    //    }
    //}

    private void GetBattleMapData()
    {
        tilemap.ClearAllTiles();
        BattleCfg battleCfg;
        battleCfg.mapWidth = 100;
        battleCfg.mapHeight = 100;
        BattleMgr.Instance.CreatBattle(battleCfg);
        mapData = BattleMgr.Instance.battleMap;
        //battleMap.RandomGenerateMapCell();
        mapData.RandomGenerateMap();
        RefreshMap();
    }

    public void Init(BattleMap data)
    {
        tilemap = GetComponent<Tilemap>();
        normal = Resources.Load("white") as Tile;
        obstacle = Resources.Load("black") as Tile;

        mapData = data;
        RefreshMap();
    }

    private void RefreshMap()
    {
        if (mapData == null) return;

        for (int row = 0; row < mapData.Height; row++)
        {
            for (int col = 0; col < mapData.Width; ++col)
            {
                MapGrid grid = mapData.mapGrids[row, col];
                if (grid == null || grid.GridType == GridType.None) continue;

                Tile tile = default;
                switch (grid.GridType)
                {
                    case GridType.Normal: tile = normal; ; break;
                    case GridType.Obstacle: tile = obstacle; break;
                }
                tilemap.SetTile(grid.GridPosVec3Int, tile);
            }
        }
    }
}
