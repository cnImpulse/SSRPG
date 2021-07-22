public struct GridPos
{
    public int row;
    public int col;
}

public enum GridType
{
    Normal,     //平地
    Obstacle    //障碍
}

public class MapGrid : EntityBase
{
    private GridPos m_gridPos;
    private GridType m_gridType;

    private MapGrid() { }
    public MapGrid(GridPos gridPos)
    {
        m_gridPos = gridPos;
    }
}