using UnityEditor;
using UnityEngine;

public static class Utl
{
    public static Vector3Int ToVec3Int(Vector2Int vec)
    {
        return new Vector3Int(vec.x, vec.y, 0);
    }
}