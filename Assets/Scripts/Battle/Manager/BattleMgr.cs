using System.Collections.Generic;
using UnityEngine;

public struct BattleCfg {
    public int mapWidth;
    public int mapHeight;
}

public enum BattleState
{
    None,       // 战斗未开始
    Amity,      // 我方回合
    Enemy,      // 敌方回合
    End         // 战斗结束
}

public class BattleMgr : Singleton<BattleMgr>
{
    public BattleMap battleMap;
    public BattleTeam battleTeam;

    public BattleRenderer battleRenderer;

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

        battleState = BattleState.Amity;
    }

    // Pass表示敌人会阻碍通过
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
                List<Vector2Int> neighbors = GetCanPassNeighbors(open[j], battleUnit);
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

    public List<Vector2Int> GetCanPassNeighbors(Vector2Int pos, BattleUnit battleUnit)
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

    #region 战斗状态

    public BattleState battleState = BattleState.None;
    public List<MapGrid> canMoveGrids = new List<MapGrid>();
    public BattleUnit selectUnit = null;

    #endregion

    #region 事件

    public void OnPointMapGrid(MapGrid grid)
    {
        if(selectUnit != null)
        {
            if (canMoveGrids.Contains(grid))
            {
                selectUnit.Move(grid);
            }
            OnUnSelectBattleUnit();
        }

        OnDataChange();
    }

    public void OnPointBattleUnit(BattleUnit battleUnit)
    {
        OnSelectBattleUnit(battleUnit);

        OnDataChange();
    }

    private void OnSelectBattleUnit(BattleUnit battleUnit)
    {
        selectUnit = battleUnit;
        SetCanMoveGrids(battleUnit);
    }

    private void OnUnSelectBattleUnit()
    {
        selectUnit = null;
        canMoveGrids.Clear();
    }

    private void OnDataChange()
    {
        battleRenderer.Refresh();
    }

    #endregion

    private void SetCanMoveGrids(BattleUnit battleUnit)
    {
        canMoveGrids.Clear();
        List<Vector2Int> canPass = GetCanPassPos(battleUnit);
        foreach(var pos in canPass)
        {
            MapGrid grid = battleMap.GetMapGrid(pos);
            if (IsGridCanMove(pos)) canMoveGrids.Add(grid);
        }
    }
}