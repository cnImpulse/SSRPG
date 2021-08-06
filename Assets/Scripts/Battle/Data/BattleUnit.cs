using UnityEngine;

public class BattleUnit : EntityBase
{
    public Vector2Int position;
    public BattleCamp camp;

    private int hp = 10;

    public int Hp
    {
        get => hp;
        set
        {
            if (value <= 0)
            {
                value = 0;
                OnDie();
            }
            else if (value > HpLimit)
            {
                value = HpLimit;
            }
            if (value < hp)
            {
                OnHurt();
            }
            else if (value > hp)
            {
                OnCure();
            }
            hp = value;
        }
    }
    public int HpLimit = 10;
    public int Atk = 1;
    public int AtkRange = 1;
    public int Mov = 5;
    public int Act = 1;

    public BattleUnit(Vector2Int pos, BattleCamp camp)
    {
        position = pos;
        this.camp = camp;
    }

    public bool CanAction()
    {
        if (BattleMgr.Instance.battleState.ToString() != camp.ToString() || Act == 0)
            return false;
        return true;
    }

    public void Move(MapGrid grid)
    {
        position = grid.Position;
        Act--;
    }

    public void BoutRecover()
    {
        Act = 1;
    }

    private void OnDie()
    {

    }

    private void OnHurt()
    {

    }

    private void OnCure()
    {

    }
}