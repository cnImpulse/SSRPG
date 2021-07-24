using UnityEngine;

public struct GridPos {
    public int row;
    public int col;
}

public enum GridType {
    Normal,     //平地
    Obstacle    //障碍
}

public class MapGrid : EntityBase {
    private GridPos m_gridPos;
    private GridType m_gridType = GridType.Normal;

    public GridPos GridPos => m_gridPos;
    public Vector3Int GridPosVec3Int
    {
        get
        {
            return new Vector3Int(GridPos.col, GridPos.row, 0);
        }
    }
    public GridType GridType => m_gridType;

    private MapGrid() { }

    public MapGrid(GridPos gridPos) {
        m_gridPos = gridPos;
    }

    public MapGrid(int row, int col) {
        m_gridPos.row = row;
        m_gridPos.col = col;
    }
}