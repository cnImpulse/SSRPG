using System.Collections.Generic;
using UnityEngine;

public struct BattleCfg {
    public int mapWidth;
    public int mapHeight;
}

public class BattleMgr : Singleton<BattleMgr> 
{
    public BattleMap battleMap;
    public List<BattleUnit> amityUnits;
    public List<BattleUnit> enemyUnits;

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
        amityUnits = new List<BattleUnit>();
        enemyUnits = new List<BattleUnit>();
    }
    #endregion
}