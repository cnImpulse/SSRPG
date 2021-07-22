public struct BattleCfg {
    public int mapWidth;
    public int mapHeight;
}

public class BattleMgr : Singleton<BattleMgr> {
    private BattleMap m_battleMap;
    private BattleUnit[] m_battleUnits;

    public void CreatBattle(BattleCfg battleCfg) {
        InitBattle(battleCfg);
    }

    private void InitBattle(BattleCfg battleCfg) {
        int mapWidth = battleCfg.mapWidth;
        int mapHeight = battleCfg.mapHeight;

        InitBattleMap(mapWidth, mapHeight);
        InitBattleUnits();
    }

    private void InitBattleMap(int width, int height) {
        m_battleMap = new BattleMap(width, height);
    }

    private void InitBattleUnits() {
        
    }
}