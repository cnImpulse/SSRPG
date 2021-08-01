using UnityEngine;

public class BattleUnit : EntityBase
{
    public Vector2Int position;
    public BattleCamp battleCamp;
    public BattleAttr battleAttr;

    public BattleUnit(Vector2Int pos, BattleCamp camp)
    {
        position = pos;
        battleCamp = camp;
        battleAttr = new BattleAttr();
    }

    public bool CanAction()
    {
        if (BattleMgr.Instance.battleState.ToString() != battleCamp.ToString() || battleAttr.act == 0)
            return false;
        return true;
    }
}