using System.Collections.Generic;
using UnityEngine;

public struct BattleCfg {
    public int mapWidth;
    public int mapHeight;
}

public enum BattleState
{
    None,       // ս��δ��ʼ
    Fighting,   // ս����
    End         // ս������
}

public class BattleMgr : Singleton<BattleMgr>
{
    public BattleMap battleMap;
    public List<BattleUnit> amityUnits;
    public List<BattleUnit> enemyUnits;

    public BattleRenderer battleRenderer;

    public BattleState battleState;

    public void CreatBattle(BattleCfg battleCfg) 
    {
        InitBattleData(battleCfg);
    }

    public void CreatBattle(BattleData data)
    {
        battleMap = data.mapData;
        amityUnits = new List<BattleUnit>();
        enemyUnits = new List<BattleUnit>();

        CreatRenderer(data);
    }

    public void CreatRenderer(BattleData data)
    {
        battleRenderer = GameObject.Instantiate(Resources.Load<BattleRenderer>("Prefabs/Battle"));
        battleRenderer.name = "Battle";
        battleRenderer.Init(data);
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

    // ��ʼս��ʱ����
    public void OnStartBattle()
    {
        if (battleState != BattleState.None) return;

        battleState = BattleState.Fighting;

    }

    // �����ս����λʱ����
    public void OnPointBattleUnit()
    {

    }
}