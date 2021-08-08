using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BattleState
{
    None,       // 战斗未开始
    Amity,      // 我方回合
    Enemy,      // 敌方回合
    End         // 战斗结束
}

public class BattleLogicMgr : Singleton<BattleLogicMgr>
{
    private BattleMap map;
    private BattleTeam team;
    private BattleState state = BattleState.None;

    public event Action<BattleState> OnNextBout;

    public void StartBattle(BattleData data)
    {
        map = data.mapData;
        team = new BattleTeam(data.unitsData);

        OnNextBout += team.BattleUnitsRecover;

        NextBout();
    }

    private void NextBout()
    {
        if(state == BattleState.None || state == BattleState.Enemy)
        {
            state = BattleState.Amity;
        }
        else if(state == BattleState.Amity)
        {
            state = BattleState.Enemy;
        }
        OnNextBout(state);
        TeamAct();
    }

    private void TeamAct()
    {
        BattleUnit actUnit = GetActUnit();
        while (actUnit != null)
        {
            UnitAct(actUnit);
            actUnit = GetActUnit();
        }
    }

    private void UnitAct(BattleUnit actUnit)
    {
        do
        {
            UnitActAI(actUnit);
        } while (actUnit.Act > 0);
    }

    private void UnitActAI(BattleUnit actUnit)
    {
        // 选择目标
        BattleUnit target = null;
        int minDistance = int.MaxValue;
        List<BattleUnit> enemys = team.GetCampEnemys(actUnit.camp);
        foreach (var enemy in enemys)
        {
            int d = Utl.GetDistance(actUnit.position, enemy.position);
            if (minDistance > d)
            {
                minDistance = d;
                target = enemy;
            }
        }

        // 移动
        List<MapGrid> path, search;
        Navigator<BattleMap, MapGrid>.Instance.Navigate(map, map.GetMapGrid(actUnit.position), map.GetMapGrid(target.position), out path, out search);
        foreach(var p in path)
        {
            Debug.Log(p.Position);
        }
        Debug.Log("--------------");
        actUnit.Act--;
    }

    private BattleUnit GetActUnit()
    {
        List<BattleUnit> units = team.GetActTeam(state);
        foreach(var unit in units)
        {
            if(unit.Act > 0)
            {
                return unit;
            }
        }
        return null;
    }

    private void BattleUnitAttack(BattleUnit attacker, BattleUnit target)
    {
        target.Hp -= attacker.Atk;
    }

    private void IsBattleEnd()
    {
        
    }
}