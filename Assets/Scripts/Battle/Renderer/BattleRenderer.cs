using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleRenderer : MonoBehaviour
{
    BattleData battleData;

    BattleMapRenderer mapRenderer;
    BattleUnitsRenderer unitsRenderer;
    Tilemap canMoveArea;
    TileBase streak;
    GameObject selectEff;

    public void Init(BattleData data)
    {
        battleData = data;

        mapRenderer = GetComponentInChildren<BattleMapRenderer>();
        unitsRenderer = GetComponentInChildren<BattleUnitsRenderer>();
        canMoveArea = transform.Find("CanMoveArea").GetComponent<Tilemap>();
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
                selectEff.SetActive(true);
                Vector3 pos = new Vector3(0.5f + pointPos.x, 0.5f + pointPos.y, 0);
                selectEff.transform.position = pos;
                OnSelectBattleUnit(BattleMgr.Instance.GetBattleUnit(pointPos));
            }
        }
    }

    private void OnUnSelectBattleUnit()
    {
        canMoveArea.ClearAllTiles();
    }

    private void OnSelectBattleUnit(BattleUnit battleUnit)
    {
        if (battleUnit == null) return;
        ShowCanMoveGrids(battleUnit);
    }

    private void ShowCanMoveGrids(BattleUnit battleUnit)
    {
        canMoveArea.ClearAllTiles();
        List<Vector2Int> canMoves = BattleMgr.Instance.GetCanMovePos(battleUnit);
        foreach(var temp in canMoves)
        {
            canMoveArea.SetTile(Utl.ToVec3Int(temp), streak);
        }
    }
}
