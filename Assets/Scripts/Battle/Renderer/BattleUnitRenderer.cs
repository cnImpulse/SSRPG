using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitRenderer : MonoBehaviour
{
    private SpriteRenderer sprite;
    private BattleUnit battleUnit;
    private Color amityColor, enemyColor;

    public void BindData(BattleUnit data)
    {
        if (data == null) return;

        sprite = GetComponent<SpriteRenderer>();
        ColorUtility.TryParseHtmlString("#70FFF0", out amityColor);
        ColorUtility.TryParseHtmlString("#FF7070", out enemyColor);

        battleUnit = data;
        Refresh();
    }

    public void Refresh()
    {
        RefreshPos();
        RefreshColor();
    }

    private void RefreshColor()
    {
        switch (battleUnit.camp)
        {
            case BattleCamp.Amity: sprite.color = amityColor; break;
            case BattleCamp.Enemy: sprite.color = enemyColor; break;
        }
    }

    private void RefreshPos()
    {
        transform.position = Utl.ToScreenPos(battleUnit.position);
    }
}
