using UnityEngine;

public interface INode
{
    Vector2Int Position { get; set; }
    int GetDistance(Vector2Int position);
}