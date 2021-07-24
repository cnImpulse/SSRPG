using UnityEngine;

public struct GridPos {
    public int row;
    public int col;

    public GridPos(int row, int col)
    {
        this.row = row;
        this.col = col;
    }
}

public enum GridType {
    None,
    Normal,     //平地
    Obstacle    //障碍
}

public class MapGrid : EntityBase {
    private GridPos m_gridPos;

    public GridPos GridPos => m_gridPos;
    public Vector2Int GridPosVec2Int
    {
        get
        {
            return new Vector2Int(GridPos.col, GridPos.row);
        }
    }
    public Vector3Int GridPosVec3Int
    {
        get
        {
            return new Vector3Int(GridPos.col, GridPos.row, 0);
        }
    }
    public GridType GridType { get; set; }

    private MapGrid() { }

    public MapGrid(GridPos gridPos) {
        m_gridPos = gridPos;
    }

    public MapGrid(int row, int col) {
        m_gridPos.row = row;
        m_gridPos.col = col;
    }
}