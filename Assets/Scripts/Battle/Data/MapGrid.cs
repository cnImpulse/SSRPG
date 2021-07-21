struct GridPos
{
    public int row;
    public int col;
}

enum GridType
{
    Normal,     //平地
    Obstacle    //障碍
}

public class MapGrid : EntityBase
{
    private GridPos gridPos;
    private GridType gridType;
}