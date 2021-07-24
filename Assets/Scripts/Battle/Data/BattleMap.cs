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
    }
}