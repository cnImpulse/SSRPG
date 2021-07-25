using UnityEngine;

public enum BattleCamp
{
    Amity,      //友好
    Enemy,      //敌对
    Neutrality  //中立
}

public class BattleUnit : EntityBase
{
    private Vector2Int position;
    private BattleCamp battleCamp;
    private BattleAttr battleAttr;

    public BattleUnit(Vector2Int pos, BattleCamp camp)
    {
        position = pos;
        battleCamp = camp;
        battleAttr = new BattleAttr();
    }
}