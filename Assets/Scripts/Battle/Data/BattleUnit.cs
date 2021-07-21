enum BattleCamp
{
    Amity,      //友好
    Hostility,  //敌对
    Neutrality  //中立
}

public class BattleUnit : EntityBase
{
    private GridPos gridPos;
    private BattleCamp battleCamp;
    private BattleAttr battleAttr;
}