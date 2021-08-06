using System.Collections.Generic;
using UnityEngine;

public struct BattleCfg {
    public int mapWidth;
    public int mapHeight;
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

    // ��ʼս��ʱ����
    public void OnStartBattle()
    {
        if (battleState != BattleState.None) return;

        battleState = BattleState.Amity;
    }

    // Pass��ʾ���˻��谭ͨ��
    // �����������
    public List<Vector2Int> GetCanPassPos(BattleUnit battleUnit)
    {
        List<Vector2Int> open = new List<Vector2Int>();
        List<Vector2Int> close = new List<Vector2Int>();
        open.Add(battleUnit.position);
        for (int i = 0; i <= battleUnit.Mov; ++i)
        {
            int len = open.Count;
            if (len == 0) break;
            for (int j = 0; j < len; ++j)
            {
                List<Vector2Int> neighbors = GetCanPassNeighbors(open[j], battleUnit);
                foreach (var neigh in neighbors)
                {
                    if (!close.Contains(neigh))
                        open.Add(neigh);
                }
                close.Add(open[j]);
            }
        }
        return close;
    }

    public List<Vector2Int> GetCanPassNeighbors(Vector2Int pos, BattleUnit battleUnit)
    {
        List<Vector2Int> res = new List<Vector2Int>();
        List<MapGrid> neighbors = battleMap.GetNeighbors(pos, BattleMap.dirArray4);
        foreach (var neighbor in neighbors)
        {
            if (IsGridCanPass(neighbor.Position, battleUnit))
                res.Add(neighbor.Position);
        }
        return res;
    }

    // ���˻��谭�ƶ�
    public bool IsGridCanPass(Vector2Int destination, BattleUnit battleUnit)
    {
        MapGrid grid = battleMap.GetMapGrid(destination);
        if (grid == null || grid.IsObstacle) return false;

        List<BattleUnit> obstacleUnits = battleTeam.GetCampEnemys(battleUnit.camp);
        foreach (var obstacle in obstacleUnits)
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

        if (battleTeam.IsHasUnit(destination)) return true;
        return false;
    }

    #region ս��״̬
    public ActMod actMod { get; set; } = ActMod.None;
    public BattleState battleState = BattleState.Amity;
    public List<MapGrid> canMoveGrids = new List<MapGrid>();
    public List<MapGrid> canAtkGrids = new List<MapGrid>();
    public BattleUnit selectUnit = null;

    #endregion

    #region �¼�

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
        if (!battleUnit.CanAction())
        {
            OnUnSelectBattleUnit();
            return;
        }
        selectUnit = battleUnit;
        battleRenderer.ShowButtleUnitActCmd();
        OnSelectAtkCmd(battleUnit);
    }

    private void OnUnSelectBattleUnit()
    {
        selectUnit = null;
        canMoveGrids.Clear();
    }

    private void OntSelectMoveCmd(BattleUnit battleUnit)
    {
        SetCanMoveGrids(battleUnit);
        actMod = ActMod.Move;
    }

    private void OnSelectAtkCmd(BattleUnit battleUnit)
    {
        SetCanAtkGrids(battleUnit);
        actMod = ActMod.Atk;
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

    private void SetCanAtkGrids(BattleUnit battleUnit)
    {
        canAtkGrids.Clear();
        canAtkGrids = GetCanAttackPos(battleUnit);
    }

    private List<MapGrid> GetCanAttackPos(BattleUnit battleUnit)
    {
        List<MapGrid> res = new List<MapGrid>();
        List<MapGrid> neighbors = battleMap.GetNeighborsInRange(battleUnit.position, battleUnit.AtkRange, BattleMap.dirArray4);
        foreach (var grid in neighbors)
        {
            if (IsGridCanAtk(battleUnit, grid)) res.Add(grid);
        }
        return neighbors;
    }

    private bool IsGridCanAtk(BattleUnit battleUnit, MapGrid grid)
    {
        if (grid == null || grid.IsObstacle || battleTeam.IsHasCanAtkUnit(battleUnit, grid.Position)) return false;
        return true;
    }
}