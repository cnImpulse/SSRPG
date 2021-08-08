using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public static class Utl
{
    public static Vector3Int ToVec3Int(Vector2Int vec)
    {
        return new Vector3Int(vec.x, vec.y, 0);
    }

    public static Vector2Int ToVec2Int(Vector3 vec)
    {
        return new Vector2Int((int)vec.x, (int)vec.y);
    }

    public static Vector3 ToVec3(Vector2Int vec)
    {
        return new Vector3(vec.x, vec.y, 0);
    }

    public static List<Vector2Int> GridsToVec2(List<MapGrid> grids)
    {
        if (grids == null) return null;
        List<Vector2Int> res = new List<Vector2Int>();
        foreach(var grid in grids)
        {
            res.Add(grid.Position);
        }
        return res;
    }

    public static Vector3 ToScreenPos(Vector2Int pos)
    {
        return new Vector3(0.5f + pos.x, 0.5f + pos.y, 0);
    }

    public static int GetDistance(Vector2Int pos1, Vector2Int pos2)
    {
        return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
    }
}