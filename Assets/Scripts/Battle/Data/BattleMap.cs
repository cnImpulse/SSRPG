using System.Collections.Generic;
using UnityEngine;

public class BattleMap : EntityBase, IMap<MapGrid>
{
    public int Width = 0;
    public int Height = 0;

    public MapGrid[,] mapGrids;
    public List<MapGrid> normalGrids;
    public List<MapGrid> obstacleGrids;

    public BattleMap(int width, int height) 
    {
        Width  = width; 
        Height = height;
        mapGrids = new MapGrid[Height, Width];
        normalGrids = new List<MapGrid>();
        obstacleGrids = new List<MapGrid>();

        for (int row = 0; row < Height; ++row) 
        {
            for(int col = 0; col < Width; ++col) 
            {
                mapGrids[row, col] = new MapGrid(row, col);
            }
        }
    }

    public List<MapGrid> GetNeighbors(MapGrid center)
    {
        if (center == null) return null;
        Vector2Int[] dirArray = dirArray4;
        List<MapGrid> neighbors = new List<MapGrid>();
        for (int i = 0; i < dirArray.Length; ++i)
        {
            MapGrid grid = GetMapGrid(center.Position + dirArray[i]);
            if (grid != null) neighbors.Add(grid);
        }
        return neighbors;
    }

    public List<MapGrid> GetNeighbors(Vector2Int center, Vector2Int[] dirArray)
    {
        if (!IsInMap(center)) return null;
        List<MapGrid> neighbors = new List<MapGrid>();
        for (int i = 0; i < dirArray.Length; ++i)
        {
            MapGrid grid = GetMapGrid(center + dirArray[i]);
            if (grid != null) neighbors.Add(grid);
        }
        return neighbors;
    }

    public List<MapGrid> GetNeighborsInRange(Vector2Int center, int distance, Vector2Int[] dirArray)
    {
        MapGrid grid = GetMapGrid(center);
        if (grid == null) return null;
        List<MapGrid> open = new List<MapGrid>();
        List<MapGrid> close = new List<MapGrid>();
        open.Add(grid);
        for (int i = 0; i <= distance; ++i)
        {
            int len = open.Count;
            if (len == 0) break;
            for (int j = 0; j < len; ++j)
            {
                List<MapGrid> neighbors = GetNeighbors(center, dirArray);
                foreach (var neigh in neighbors)
                {
                    if (!close.Contains(neigh))
                        open.Add(neigh);
                }
                close.Add(open[j]);
            }
        }
        return close;
    }

    public int GetNeighborsTypeCount(Vector2Int position, Vector2Int[] dirArray, GridType gridType)
    {
        int count = 0;
        List<MapGrid> neighbors = GetNeighbors(position, dirArray);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.GridType == gridType) ++count;
        }
        return count;
    }

    public MapGrid GetMapGrid(Vector2Int gridPos)
    {
        if (!IsInMap(gridPos)) return null;
        return mapGrids[gridPos.y, gridPos.x];
    }

    public bool SetMapGrid(Vector2Int position, GridType gridType)
    {
        if (!IsInMap(position)) return false;
        GetMapGrid(position).GridType = gridType;
        return true;
    }

    public bool IsInMap(Vector2Int gridPos)
    {
        if (gridPos.x < 0 || gridPos.x >= Width || gridPos.y < 0 || gridPos.y >= Height)
            return false;
        return true;
    }

    public bool IsBoundary(Vector2Int gridPos)
    {
        if (gridPos.x == 0 || gridPos.x == Height-1 || gridPos.y ==0 || gridPos.y == Width-1)
            return true;
        return false;
    }

    public static Vector2Int[] dirArray4 = { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right };
    public static Vector2Int[] dirArray8 = { Vector2Int.up, Vector2Int.one, Vector2Int.right, new Vector2Int(1, -1),
                               Vector2Int.down, new Vector2Int(-1, -1), Vector2Int.left, new Vector2Int(-1, 1)};
    // 随机游走算法-效率低下版
    public void RandomGenerateMap()
    {
        // 随机起点
        int row = Random.Range(0, Height);
        int col = Random.Range(0, Width);
        Vector2Int position = new Vector2Int(col, row);

        // 随机乱走
        int roadNum = 0;
        while (roadNum < Width * Height / 2)
        {
            MapGrid grid = GetMapGrid(position);
            if (grid != null && grid.GridType != GridType.Normal)
            {
                grid.GridType = GridType.Normal;
                normalGrids.Add(grid);
                ++roadNum;
            }
            List<MapGrid> neighbors = GetNeighbors(position, dirArray4);
            position = neighbors[Random.Range(0, neighbors.Count)].Position;
        }

        // 画墙壁
        foreach(var grid in normalGrids)
        {
            List<MapGrid> neighbors = GetNeighbors(grid.Position, dirArray4);
            foreach(var neighbor in neighbors)
            {
                if (neighbor.GridType == GridType.None)
                {
                    neighbor.GridType = GridType.Obstacle;
                    obstacleGrids.Add(neighbor);
                }
            }
        }

        // 清除孤立的墙壁
        List<MapGrid> needRemove = new List<MapGrid>();
        for (int i = 0; i < 4; ++i)
        {
            foreach (var grid in obstacleGrids)
            {
                int normalCount = GetNeighborsTypeCount(grid.Position, dirArray8, GridType.Normal);
                if (normalCount >= 7) needRemove.Add(grid);
            }
            foreach (var grid in needRemove)
            {
                grid.GridType = GridType.Normal;
                obstacleGrids.Remove(grid);
                normalGrids.Add(grid);
            }
            needRemove.Clear();
        }
    }

    // 思路参考细胞自动机
    public void RandomGenerateMapCell()
    {
        // 随机生成地面和墙壁
        for (int row = 0; row < Height; ++row)
        {
            for (int col = 0; col < Width; ++col)
            {
                MapGrid grid = mapGrids[row, col];
                if (IsBoundary(new Vector2Int(col, row))) grid.GridType = GridType.Obstacle;
                else grid.GridType = (GridType)Random.Range(1, 3);

                if (grid.GridType == GridType.Normal) normalGrids.Add(grid);
                else if (grid.GridType == GridType.Obstacle) obstacleGrids.Add(grid);
            }
        }


        List<MapGrid> needSetObstacle = new List<MapGrid>();
        List<MapGrid> needSetNormal = new List<MapGrid>();
        for (int i = 0; i < 5; ++i)
        {
            for (int row = 0; row < Height; ++row)
            {
                for (int col = 0; col < Width; ++col)
                {
                    MapGrid grid = mapGrids[row, col];
                    int count = 8 - GetNeighbors(grid.Position, dirArray8).Count +
                        GetNeighborsTypeCount(grid.Position, dirArray8, GridType.Obstacle);
                    if (count > 4 && grid.GridType == GridType.Normal)
                        needSetObstacle.Add(grid);
                    else if (count < 4 && grid.GridType == GridType.Obstacle)
                        needSetNormal.Add(grid);
                }
            }
            foreach (var grid in needSetNormal) grid.GridType = GridType.Normal;
            foreach (var grid in needSetObstacle) grid.GridType = GridType.Obstacle;
            needSetNormal.Clear();
            needSetObstacle.Clear();
        }
    }
}