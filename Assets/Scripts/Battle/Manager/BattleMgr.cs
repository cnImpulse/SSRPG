using System.Collections.Generic;
using UnityEngine;

public struct BattleCfg {
    public int mapWidth;
    public int mapHeight;
}

public enum BattleState
{
    None,       // 战斗未开始
    Fighting,   // 战斗中
    End         // 战斗结束
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
        foreach(var t in data.unitsData)
        {
            if (t.battleCamp == BattleCamp.Amity) amityUnits.Add(t);
            else if(t.battleCamp == BattleCamp.Enemy) enemyUnits.Add(t);
        }

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

    // 开始战斗时调用
    public void OnStartBattle()
    {
        if (battleState != BattleState.None) return;

        battleState = BattleState.Fighting;
    }

    // 广度优先搜索
    public List<Vector2Int> GetCanMovePos(BattleUnit battleUnit)
    {
        List<Vector2Int> open = new List<Vector2Int>();
        List<Vector2Int> close = new List<Vector2Int>();
        open.Add(battleUnit.position);
        for (int i = 0; i < battleUnit.battleAttr.mov; ++i)
        {
            int len = open.Count;
            if (len == 0) break;
            for (int j = 0; j < len; ++j)
            {
                List<Vector2Int> neighbors = GetCanMoveNeighbors(open[j], battleUnit);
                foreach(var neigh in neighbors)
                {
                    if (!close.Contains(neigh))
                        open.Add(neigh);
                }
                close.Add(open[j]);
            }
        }
        return close;
    }

    public List<Vector2Int> GetCanMoveNeighbors(Vector2Int pos, BattleUnit battleUnit)
    {
        List<Vector2Int> res = new List<Vector2Int>();
        List<MapGrid> neighbors = battleMap.GetNeighbors(pos, BattleMap.dirArray4);
        foreach(var neighbor in neighbors)
        {
            if (IsGridCanArrive(neighbor.Position, battleUnit))
                res.Add(neighbor.Position);
        }
        return res;
    }

    // 敌人会阻碍移动
    public bool IsGridCanArrive(Vector2Int destination, BattleUnit battleUnit)
    {
        MapGrid grid = battleMap.GetMapGrid(destination);
        if(grid == null || grid.IsObstacle) return false;

        List<BattleUnit> obstacleUnits = null;
        if (battleUnit.battleCamp == BattleCamp.Amity)
            obstacleUnits = enemyUnits;
        else if (battleUnit.battleCamp == BattleCamp.Enemy)
            obstacleUnits = amityUnits;

        foreach(var obstacle in obstacleUnits) {
            if (destination == obstacle.position)
                return false;
        }
        return true;
    }


    public BattleUnit GetBattleUnit(Vector2Int pos)
    {
        foreach(var t in amityUnits)
            if (t.position == pos) return t;
        foreach (var t in enemyUnits)
            if (t.position == pos) return t;
        return null;
    }
}