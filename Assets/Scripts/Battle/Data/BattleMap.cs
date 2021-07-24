using System.Collections.Generic;
using UnityEngine;

public class BattleMap : EntityBase 
{
    private int m_width = 0;
    private int m_height = 0;

    public int Width 
    {
        get => m_width;
        set 
        {
            if(value<0) value = 0;
            m_width = value;
        }
    }
    public int Height 
    {
        get => m_height;
        set 
        {
            if(value<0) value = 0;
            m_height = value;
        }
    }
    public MapGrid[,] mapGrids;
    public List<MapGrid> normalGrids;
    public List<MapGrid> obstacleGrids;

    public BattleMap(int width, int height) 
    {
        Width  = width; 
        Height = height;
        mapGrids = new MapGrid[Height, Width];
        for(int row = 0; row < Height; ++row) 
        {
            for(int col = 0; col < Width; ++col) 
            {
                mapGrids[row, col] = new MapGrid(row, col);
            }
        }
        RandomGenerateMap();
    }

    public List<MapGrid> GetNeighbors(Vector2Int position)
    {
        if (!IsInMap(position)) return null;
        List<MapGrid> neighbors = new List<MapGrid>();
        for(int i=0; i<4; ++i)
        {
            MapGrid grid = GetMapGrid(position + dirArray[i]);
            if (grid != null) neighbors.Add(grid);
        }
        return neighbors;
    }

    public MapGrid GetMapGrid(Vector2Int gridPos)
    {
        if (!IsInMap(gridPos)) return null;
        return mapGrids[gridPos.y, gridPos.x];
    }

    public bool IsInMap(Vector2Int gridPos)
    {
        if (gridPos.x < 0 || gridPos.x >= Height || gridPos.y < 0 || gridPos.y >= Width)
            return false;
        return true;
    }

    Vector2Int[] dirArray = { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right };
    // 随机游走算法-效率低下版
    public void RandomGenerateMap()
    {
        normalGrids = new List<MapGrid>();
        obstacleGrids = new List<MapGrid>();

        // 随机起点
        int row = Random.Range(0, Height);
        int col = Random.Range(0, Width);
        Vector2Int position = new Vector2Int(col, row);

        // 随机乱走
        int roadNum = 0;
        while (roadNum < 3000)
        {
            MapGrid grid = GetMapGrid(position);
            if (grid != null && grid.GridType != GridType.Normal)
            {
                grid.GridType = GridType.Normal;
                normalGrids.Add(grid);
                ++roadNum;
            }
            List<MapGrid> neighbors = GetNeighbors(position);
            position = neighbors[Random.Range(0, neighbors.Count)].GridPosVec2Int;
        }

        // 画墙壁
        foreach(var grid in normalGrids)
        {
            List<MapGrid> neighbors = GetNeighbors(grid.GridPosVec2Int);
            foreach(var neighbor in neighbors)
            {
                if (neighbor.GridType == GridType.None)
                {
                    neighbor.GridType = GridType.Obstacle;
                    obstacleGrids.Add(neighbor);
                }
            }
        }
    }
}