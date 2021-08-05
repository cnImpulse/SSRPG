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

public class BattleLogicMgr
{
    private BattleMap map;
    private BattleTeam team;
    private BattleState state = BattleState.None;

    public event Action<BattleState> OnNextBout;

    public void StartBattle(BattleData data)
    {
        map = data.mapData;
        team = data.teamData;

        OnNextBout += team.BattleUnitsRecover;

        NextBout();
        TeamAct();
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
    }

    private void TeamAct()
    {
        BattleUnit actUnit = GetActUnit();
        while (actUnit != null)
        {
            UnitAct(actUnit);
        }
        NextBout();
        TeamAct();
    }

    private void UnitAct(BattleUnit actUnit)
    {
        do
        {
            MoveAI(actUnit);
            AttackAI(actUnit);
        } while (actUnit.attr.act > 0);
    }

    private void MoveAI(BattleUnit actUnit)
    {

    }

    private void AttackAI(BattleUnit actUnit)
    {

    }

    private BattleUnit GetActUnit()
    {
        List<BattleUnit> units = team.GetActTeam(state);
        foreach(var unit in units)
        {
            if(unit.attr.act > 0)
            {
                return unit;
            }
        }
        return null;
    }
}