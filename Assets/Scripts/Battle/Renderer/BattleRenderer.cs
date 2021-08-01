using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleRenderer : MonoBehaviour
{
    BattleData battleData;

    BattleMapRenderer mapRenderer;
    BattleUnitsRenderer unitsRenderer;
    Tilemap gridsEff;
    TileBase streak;
    GameObject selectEff;

    public void Init(BattleData data)
    {
        battleData = data;

        mapRenderer = GetComponentInChildren<BattleMapRenderer>();
        unitsRenderer = GetComponentInChildren<BattleUnitsRenderer>();
        gridsEff = transform.Find("CanMoveArea").GetComponent<Tilemap>();
        streak = Resources.Load<TileBase>("streak");
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
        gridsEff.ClearAllTiles();
        canMovePos.Clear();
        selectUnit = null;
        selectEff.SetActive(false);
    }

    private void OnSelectBattleUnit(BattleUnit battleUnit)
    {
        selectUnit = battleUnit;
        ShowCanMoveGrids(battleUnit);
    }

    BattleUnit selectUnit = null;
    List<Vector2Int> canMovePos = new List<Vector2Int>();
    private void ShowCanMoveGrids(BattleUnit battleUnit)
    {
        gridsEff.ClearAllTiles();
        canMovePos.Clear();
        List<Vector2Int> canPass = BattleMgr.Instance.GetCanPassPos(battleUnit);
        foreach(var pos in canPass)
        {
            if (BattleMgr.Instance.IsGridCanMove(pos))
            {
                gridsEff.SetTile(Utl.ToVec3Int(pos), streak);
                gridsEff.SetColor(Utl.ToVec3Int(pos), Color.yellow);
                canMovePos.Add(pos);
            }
        }
    }

    List<Vector2Int> canAttack = new List<Vector2Int>();
    private void ShowCanAttackGrids(BattleUnit battleUnit)
    {
        
    }

    private void ShowSelectEff(Vector2Int pos)
    {
        selectEff.SetActive(true);
        selectEff.transform.position = new Vector3(0.5f + pos.x, 0.5f + pos.y, 0); ;
    }
}
