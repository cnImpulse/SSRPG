using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameOfLife : MonoBehaviour
{
    private Tilemap tilemap;
    private Tile dead;
    private Tile life;
    private BattleMap mapData;

    List<MapGrid> willDie = new List<MapGrid>();
    List<MapGrid> willLive = new List<MapGrid>();

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        dead = Resources.Load("white") as Tile;
        life = Resources.Load("black") as Tile;
    }

    void Start()
    {
        GetMapData();
    }

    private void Update()
    {
        if(flag) GameLife();
    }

    bool flag = false;
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 180, 100), "Next"))
        {
            flag = !false;
        }
    }

    private void GetMapData()
    {
        tilemap.ClearAllTiles();
        mapData = new BattleMap(100, 100);
        for (int row = 0; row < mapData.Height; row++)
        {
            for (int col = 0; col < mapData.Width; ++col)
            {
                //Tile tile = tilemap.GetTile<Tile>(new Vector3Int(col, row, 0));
                //if (tile != null && tile.color == Color.black)
                //{
                //    mapData.mapGrids[row, col].GridType = GridType.Normal;
                //}
                //else
                //{
                //    mapData.mapGrids[row, col].GridType = GridType.None;
                //}
                MapGrid grid = mapData.mapGrids[row, col];
                grid.GridType = (GridType)Random.Range(0, 2);
            }
        }
        tilemap.transform.position = new Vector3(-mapData.Width / 2, -mapData.Height / 2, 0);
        RefreshMap();
    }

    public void RefreshMap()
    {
        if (mapData == null) return;

        for (int row = 0; row < mapData.Height; row++)
        {
            for (int col = 0; col < mapData.Width; ++col)
            {
                MapGrid grid = mapData.mapGrids[row, col];
                RefreshGrid(grid);
            }
        }
    }

    public void RefreshGrid(MapGrid grid)
    {
        if (grid == null) return;
        Tile tile = default;
        switch (grid.GridType)
        {
            case GridType.None: tile = dead; break;
            case GridType.Normal: tile = life; break;
        }
        tilemap.SetTile(grid.GridPosVec3Int, tile);
    }

    // 当前细胞为存活状态时，当周围的存活细胞低于2个时（不包含2个），该细胞变成死亡状态。（模拟生命数量稀少）
    // 当前细胞为存活状态时，当周围有2个或3个存活细胞时，该细胞保持原样。
    // 当前细胞为存活状态时，当周围有超过3个存活细胞时，该细胞变成死亡状态。（模拟生命数量过多）
    // 当前细胞为死亡状态时，当周围有3个存活细胞时，该细胞变成存活状态。（模拟繁殖）
    public void GameLife()
    {
        if (mapData == null) return;

        willDie.Clear();
        willLive.Clear();
        for (int row = 0; row < mapData.Height; ++row)
        {
            for (int col = 0; col < mapData.Width; ++col)
            {
                MapGrid grid = mapData.mapGrids[row, col];
                int lifeCount = mapData.GetNeighborsTypeCount(grid.GridPosVec2Int, BattleMap.dirArray8, GridType.Normal);
                if (grid.GridType == GridType.Normal && (lifeCount <= 1 || lifeCount >= 4))
                {
                    willDie.Add(grid);
                }
                else if (grid.GridType == GridType.None && lifeCount == 3) willLive.Add(grid);
            }
        }

        foreach(var grid in willDie)
        {
            grid.GridType = GridType.None;
            tilemap.SetTile(grid.GridPosVec3Int, dead);
        }
        foreach (var grid in willLive)
        {
            grid.GridType = GridType.Normal;
            tilemap.SetTile(grid.GridPosVec3Int, life);
        }
    }
}