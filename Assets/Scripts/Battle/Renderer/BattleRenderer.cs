using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleRenderer : MonoBehaviour
{
    BattleData battleData;

    BattleMapRenderer mapRenderer;
    BattleUnitsRenderer unitsRenderer;
    ActionRangeRenderer actionRenderer;
    GameObject selectEff;

    public void Init(BattleData data)
    {
        battleData = data;

        mapRenderer = GetComponentInChildren<BattleMapRenderer>();
        unitsRenderer = GetComponentInChildren<BattleUnitsRenderer>();
        actionRenderer = GetComponentInChildren<ActionRangeRenderer>();
        selectEff = transform.Find("SelectEff").gameObject;
        selectEff.SetActive(false);

        mapRenderer.Init(data.mapData);
        unitsRenderer.Init(data.unitsData);

        Camera.main.transform.position = new Vector3(data.mapData.Width / 2, data.mapData.Height / 2, -10);
        transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int pointPos = Utl.ToVec2Int(worldPos);
            BattleUnit battleUnit = BattleMgr.Instance.battleTeam.GetBattleUnit(pointPos);
            if(battleUnit != null)
            {
                BattleMgr.Instance.OnPointBattleUnit(battleUnit);
            }
            else
            {
                MapGrid grid = battleData.mapData.GetMapGrid(pointPos);
                if (grid != null)
                {
                    BattleMgr.Instance.OnPointMapGrid(grid);
                }
            }
        }
    }

    public void Refresh()
    {
        actionRenderer.Refresh();
        unitsRenderer.Refresh();
        RefreshSelectEff();
    }

    private void RefreshSelectEff()
    {
        BattleUnit battleUnit = BattleMgr.Instance.selectUnit;
        if(battleUnit == null)
        {
            selectEff.SetActive(false);
            return;
        }

        selectEff.SetActive(true);
        selectEff.transform.position = new Vector3(0.5f + battleUnit.position.x, 0.5f + battleUnit.position.y, 0);
    }
}
