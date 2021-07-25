using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleRenderer : MonoBehaviour
{
    BattleMapRenderer mapRenderer;

    public void Init(BattleData data)
    {
        mapRenderer = GetComponentInChildren<BattleMapRenderer>();
        mapRenderer.Init(data.mapData);
    }
}
