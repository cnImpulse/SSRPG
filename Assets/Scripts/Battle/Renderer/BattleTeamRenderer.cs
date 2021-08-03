using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleTeamRenderer : MonoBehaviour
{
    private List<BattleUnit> unitsData;
    private List<BattleUnitRenderer> unitsRenderer = new List<BattleUnitRenderer>();
    private BattleUnitRenderer unitPrefab;

    public void Init(List<BattleUnit> units)
    {
        unitPrefab = Resources.Load<BattleUnitRenderer>("Prefabs/BattleUnit");
        unitsData = units;
        foreach (var unit in unitsData)
        {
            BattleUnitRenderer render = Instantiate(unitPrefab, transform);
            unitsRenderer.Add(render);
            render.BindData(unit);
        }
        Refresh();
    }

    public void Refresh()
    {
        foreach(var render in unitsRenderer)
        {
            render.Refresh();
        }
    }
}