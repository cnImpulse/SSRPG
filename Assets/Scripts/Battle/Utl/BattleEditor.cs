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

    private BattleMap mapData;
    private List<BattleUnit> unitsData;

    private void Awake()
    {
        map = transform.Find("BattleMap").GetComponent<Tilemap>();
        borns = transform.Find("BattleBorns").GetComponent<Tilemap>();
        normal = Resources.Load("white") as Tile;
        obstacle = Resources.Load("black") as Tile;
        amity = Resources.Load("amity") as Tile;
        enemy = Resources.Load("enemy") as Tile;

        mapData = new BattleMap(width, height);
        unitsData = new List<BattleUnit>();
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
                if (mapTile == normal) mapData.SetMapGrid(position2, GridType.Normal);
                else if (mapTile == obstacle) mapData.SetMapGrid(position2, GridType.Obstacle);
                else Debug.LogError("错误");

                Tile bornsTile = borns.GetTile<Tile>(position3);
                if (bornsTile == amity)
                {
                    unitsData.Add(new BattleUnit(position2, BattleCamp.Amity));
                }
                else if (bornsTile == enemy)
                {
                    unitsData.Add(new BattleUnit(position2, BattleCamp.Enemy));
                }
            }
        }

        SaveData();
    }

    private void SaveData()
    {
        Debug.Log("SaveStart!");
        BattleData battleData = new BattleData();
        battleData.mapData = mapData;
        battleData.unitsData = unitsData;

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