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
    public BattleTeam battleTeam;

    public BattleRenderer battleRenderer;

    public BattleState battleState;

    public void CreatBattle(BattleCfg battleCfg) 
    {
        InitBattleData(battleCfg);
    }

    public void CreatBattle(BattleData data)
    {
        battleMap = data.mapData;
        battleTeam = new BattleTeam(data.unitsData);

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
    }

    private void InitBattleMap(int width, int height) 
    {
        battleMap = new BattleMap(width, height);
    }

    #endregion

    // 开始战斗时调用
    public void OnStartBattle()
    {
        if (battleState != BattleState.None) return;

        battleState = BattleState.Fighting;
    }

    // 广度优先搜索
    public List<Vector2Int> GetCanPassPos(BattleUnit battleUnit)
    {
        List<Vector2Int> open = new List<Vector2Int>();
        List<Vector2Int> close = new List<Vector2Int>();
        open.Add(battleUnit.position);
        for (int i = 0; i <= battleUnit.battleAttr.mov; ++i)
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

    public List<Vector2Int> GetCanAttackPos(BattleUnit battleUnit)
    {
        List<Vector2Int> res = new List<Vector2Int>();
        List<BattleUnit> notAttacks = battleTeam.GetAmitys(battleUnit.battleCamp);
        List<MapGrid> neighbors = battleMap.GetNeighborsInRange(battleUnit.position, battleUnit.battleAttr.atkRange, BattleMap.dirArray4);
        foreach(var grid in neighbors)
        {
            if (grid.IsObstacle) continue;
            
        }
        return res;
    }


    public List<Vector2Int> GetCanMoveNeighbors(Vector2Int pos, BattleUnit battleUnit)
    {
        List<Vector2Int> res = new List<Vector2Int>();
        List<MapGrid> neighbors = battleMap.GetNeighbors(pos, BattleMap.dirArray4);
        foreach(var neighbor in neighbors)
        {
            if (IsGridCanPass(neighbor.Position, battleUnit))
                res.Add(neighbor.Position);
        }
        return res;
    }

    // 敌人会阻碍移动
    public bool IsGridCanPass(Vector2Int destination, BattleUnit battleUnit)
    {
        MapGrid grid = battleMap.GetMapGrid(destination);
        if(grid == null || grid.IsObstacle) return false;

        List<BattleUnit> obstacleUnits = battleTeam.GetEnemys(battleUnit.battleCamp);
        foreach(var obstacle in obstacleUnits) 
        {
            if (destination == obstacle.position)
                return false;
        }
        return true;
    }

    public bool IsGridCanMove(Vector2Int destination)
    {
        MapGrid grid = battleMap.GetMapGrid(destination);
        if (grid == null || grid.IsObstacle) return false;

        if (battleTeam.GetBattleUnit(destination) == null) return true;
        return false;
    }
}