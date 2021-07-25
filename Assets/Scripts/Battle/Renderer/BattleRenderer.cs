using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleRenderer : MonoBehaviour
{
    BattleMapRenderer mapRenderer;
    BattleUnitsRenderer unitsRenderer;

    public void Init(BattleData data)
    {
        mapRenderer = GetComponentInChildren<BattleMapRenderer>();
        unitsRenderer = GetComponentInChildren<BattleUnitsRenderer>();
        mapRenderer.Init(data.mapData);
        unitsRenderer.Init(data.unitsData);

        transform.position = new Vector3(-data.mapData.Width / 2, -data.mapData.Height / 2);
    }
}
