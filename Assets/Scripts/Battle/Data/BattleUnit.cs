using UnityEngine;

public class BattleUnit : EntityBase
{
    public Vector2Int position;
    public BattleCamp camp;
    public BattleAttr attr;

    public BattleUnit(Vector2Int pos, BattleCamp camp)
    {
        position = pos;
        this.camp = camp;
        attr = new BattleAttr();
    }

    public bool CanAction()
    {
        if (BattleMgr.Instance.battleState.ToString() != camp.ToString() || attr.act == 0)
            return false;
        return true;
    }

    public void Move(MapGrid grid)
    {
        position = grid.Position;
        attr.act--;
    }
}