using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        BattleCfg battleCfg;
        battleCfg.mapWidth = 18;
        battleCfg.mapHeight = 10;
        BattleMgr.Instance.CreatBattle(battleCfg);
    }

    void Update()
    {
        
    }
}
