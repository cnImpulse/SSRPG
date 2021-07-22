public class BattleMap : EntityBase {
    private int m_width = 0;
    private int m_height = 0;
    private MapGrid[,] mapGrids;

    public int Width {
        get => m_width;
        set {
            if(value<0) value = 0;
            m_width = value;
        }
    }
    public int Height {
        get => m_height;
        set {
            if(value<0) value = 0;
            m_height = value;
        }
    }

    public BattleMap(int width, int height) {
        Width  = width; 
        Height = height;
        mapGrids = new MapGrid[Width, Height];
    }
}