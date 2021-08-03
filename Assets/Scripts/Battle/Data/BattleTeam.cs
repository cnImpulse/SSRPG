using System.Collections.Generic;
using UnityEngine;

public enum BattleCamp
{
    Amity,      //友好
    Enemy,      //敌对
}

public class BattleTeam
{
    public List<BattleUnit> amitys = new List<BattleUnit>();
    public List<BattleUnit> enemys = new List<BattleUnit>();

    public BattleTeam(List<BattleUnit> unitsData)
    {
        foreach (var t in unitsData)
        {
            if (t.camp == BattleCamp.Amity) amitys.Add(t);
            else if (t.camp == BattleCamp.Enemy) enemys.Add(t);
        }
    }

    public BattleUnit GetBattleUnit(Vector2Int pos)
    {
        BattleUnit unit = GetAmity(pos);
        if (unit != null) return unit;
        return GetEnemy(pos);
    }

    public BattleUnit GetAmity(Vector2Int pos)
    {
        foreach (var unit in amitys)
            if (unit.position == pos) return unit;
        return null;
    }

    public BattleUnit GetEnemy(Vector2Int pos)
    {
        foreach (var unit in enemys)
            if (unit.position == pos) return unit;
        return null;
    }

    public List<BattleUnit> GetCampEnemys(BattleCamp camp)
    {
        if (camp == BattleCamp.Amity)
            return enemys;
        else
            return amitys;
    }

    public List<BattleUnit> GetCampAmitys(BattleCamp camp)
    {
        if (camp == BattleCamp.Enemy)
            return enemys;
        else
            return amitys;
    }

    public bool IsHasUnit(Vector2Int pos)
    {
        return GetBattleUnit(pos) != null;
    }

    public bool IsHasCanAtkUnit(BattleUnit battleUnit, Vector2Int pos)
    {
        if (battleUnit.camp == BattleCamp.Amity) return GetEnemy(pos) != null;
        return GetAmity(battleUnit.position) != null;
    }
}