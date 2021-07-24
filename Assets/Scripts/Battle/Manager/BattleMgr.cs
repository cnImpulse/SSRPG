using UnityEngine;

public struct BattleCfg {
    public int mapWidth;
    public int mapHeight;
}

public class BattleMgr : Singleton<BattleMgr> 
{
    public BattleMap battleMap;
    public BattleUnit[] battleUnits;

    public void CreatBattle(BattleCfg battleCfg) 
    {
        InitBattleData(battleCfg);
    }

    #region InitData
    private void InitBattleData(BattleCfg battleCfg) 
    {
        int mapWidth = battleCfg.mapWidth;
        int mapHeight = battleCfg.mapHeight;

        InitBattleMap(mapWidth, mapHeight);
        InitBattleUnits();
    }

    private void InitBattleMap(int width, int height) 
    {
        battleMap = new BattleMap(width, height);
    }

    private void InitBattleUnits() 
    {
        
    }
    #endregion
}