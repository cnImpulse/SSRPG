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
    public GridPos GridPos { get; set; }
    public Vector2Int Position
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
    public bool IsObstacle => GridType == GridType.Obstacle;

    public GridType GridType { get; set; }

    private MapGrid() { }

    public MapGrid(GridPos gridPos) {
        GridPos = gridPos;
    }

    public MapGrid(int row, int col) {
        GridPos = new GridPos(row, col);
    }

    public MapGrid(Vector2Int pos)
    {
        GridPos = new GridPos(pos.y, pos.x);
    }
}