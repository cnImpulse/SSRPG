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
            if (battleData.mapData.IsInMap(pointPos))
            {
                OnPointMap(pointPos);
            }
        }
    }

    private void OnPointMap(Vector2Int pos)
    {
        ShowSelectEff(pos);

        BattleUnit battleUnit = BattleMgr.Instance.battleTeam.GetBattleUnit(pos);
        if (battleUnit != null)
        {
            OnSelectBattleUnit(battleUnit);
            return;
        }
        if (selectUnit != null)
        {
            if (canMovePos.Contains(pos))
            {
                OnMoveBattleUnit(pos);
            }
            OnUnSelectBattleUnit();
        }
    }

    private void OnMoveBattleUnit(Vector2Int pos)
    {
        if (selectUnit == null) return;
        unitsRenderer.ChangeBattleUnitPos(selectUnit, pos);
    }

    private void OnUnSelectBattleUnit()
    {
        canMovePos.Clear();
        selectUnit = null;
        selectEff.SetActive(false);
    }

    private void OnSelectBattleUnit(BattleUnit battleUnit)
    {
        selectUnit = battleUnit;
    }

    BattleUnit selectUnit = null;
    List<Vector2Int> canMovePos = new List<Vector2Int>();

    private void ShowSelectEff(Vector2Int pos)
    {
        selectEff.SetActive(true);
        selectEff.transform.position = new Vector3(0.5f + pos.x, 0.5f + pos.y, 0); ;
    }
}
