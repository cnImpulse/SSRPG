using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRenderer : MonoBehaviour
{
    BattleData battleData;

    BattleMapRenderer mapRenderer;
    BattleUnitsRenderer unitsRenderer;
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
                selectEff.SetActive(true);
                Vector3 pos = new Vector3(0.5f + pointPos.x, 0.5f + pointPos.y, 0);
                selectEff.transform.position = pos;
            }
        }
    }
}
