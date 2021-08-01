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
            if (t.battleCamp == BattleCamp.Amity) amitys.Add(t);
            else if (t.battleCamp == BattleCamp.Enemy) enemys.Add(t);
        }
    }

    public BattleUnit GetBattleUnit(Vector2Int pos)
    {
        foreach (var t in amitys)
            if (t.position == pos) return t;
        foreach (var t in enemys)
            if (t.position == pos) return t;
        return null;
    }

    public List<BattleUnit> GetEnemys(BattleCamp camp)
    {
        if (camp == BattleCamp.Amity)
            return enemys;
        else
            return amitys;
    }

    public List<BattleUnit> GetAmitys(BattleCamp camp)
    {
        if (camp == BattleCamp.Enemy)
            return enemys;
        else
            return amitys;
    }
}