using System.Collections.Generic;

public interface IMap<T>
    where T : INode
{
    List<T> GetNeighbors(T position);
}