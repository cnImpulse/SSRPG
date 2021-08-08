using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Test : MonoBehaviour
{
    private void Start()
    {
        string path = Application.dataPath + "/BattleDatas/default.json";
        StreamReader sr = new StreamReader(path);
        string json = sr.ReadLine();
        BattleData battleData = JsonConvert.DeserializeObject<BattleData>(json);
        if (battleData.mapData == null) Debug.LogError("gan");
        //BattleMgr.Instance.CreatBattle(battleData);
        BattleLogicMgr.Instance.StartBattle(battleData);
    }
}
