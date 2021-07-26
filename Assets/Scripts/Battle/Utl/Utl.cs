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
}