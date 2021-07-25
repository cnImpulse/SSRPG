using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;
using System.IO;
using UnityEditor;

public class BattleEditor : MonoBehaviour
{
    private int width = 32;
    private int height = 18;
    private Tilemap map;
    private Tile normal;
    private Tile obstacle;
    private Tilemap borns;
    private Tile amity;
    private Tile enemy;

    private BattleMap battleMap;
    private List<Vector2Int> amitys;
    private List<Vector2Int> enemys;

    private void Awake()
    {
        map = transform.Find("BattleMap").GetComponent<Tilemap>();
        borns = transform.Find("BattleBorns").GetComponent<Tilemap>();
        normal = Resources.Load("white") as Tile;
        obstacle = Resources.Load("black") as Tile;
        amity = Resources.Load("amity") as Tile;
        enemy = Resources.Load("enemy") as Tile;

        battleMap = new BattleMap(width, height);
        amitys = new List<Vector2Int>();
        enemys = new List<Vector2Int>();
    }

    private void Start()
    {
        for(int i=0; i<height; ++i)
        {
            for(int j=0;j<width; ++j)
            {
                Vector2Int position2 = new Vector2Int(j, i);
                Vector3Int position3 = new Vector3Int(j, i, 0);
                Tile mapTile = map.GetTile<Tile>(position3);
                if (mapTile == normal) battleMap.SetMapGrid(position2, GridType.Normal);
                else if (mapTile == obstacle) battleMap.SetMapGrid(position2, GridType.Obstacle);
                else Debug.LogError("错误");

                Tile bornsTile = borns.GetTile<Tile>(position3);
                if (bornsTile == amity) amitys.Add(position2);
                else if (bornsTile == enemy) enemys.Add(position2);
            }
        }

        SaveData();
    }

    private void SaveData()
    {
        Debug.Log("SaveStart!");
        BattleData battleData = new BattleData();
        battleData.mapData = battleMap;
        battleData.amitys = amitys;
        battleData.enemys = enemys;

        string json = JsonConvert.SerializeObject(battleData);
        string path = Application.dataPath + "/BattleData/default.json";
        FileInfo file = new FileInfo(path);
        StreamWriter sw = file.CreateText();
        sw.Write(json);
        sw.Close();
        sw.Dispose();
        AssetDatabase.Refresh();
        Debug.Log("Done!");
    }
}