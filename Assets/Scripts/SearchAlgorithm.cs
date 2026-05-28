using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class SearchAlgorithm : MonoBehaviour
{
    private NodeGenerator nodeGenerator;
    private Dictionary<int, List<int>> connectedRooms;

    private void Awake()
    {
        nodeGenerator = GetComponent<NodeGenerator>();
        connectedRooms = nodeGenerator.connectedRooms;
    }
    public List<int> GetNeighbors(int node)
    {
        
        if (connectedRooms.ContainsKey(node))
        {
            return new List<int>(connectedRooms[node]);
        }
        else
        {
            return new();
        }
    }

    [Button("BFS", EButtonEnableMode.Playmode)]
    public void BFS()
    {
        int currentNode = 1;

        HashSet<int> discovered = new();
        Queue<int> queue = new();
        queue.Enqueue(currentNode);

        while (queue.Count > 0)
        {
            currentNode = queue.Dequeue();
            discovered.Add(currentNode);
            foreach (var neighbor in GetNeighbors(currentNode))
            {
                if (!discovered.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    discovered.Add(neighbor);
                }
            }
        }

        if (discovered.Count == connectedRooms.Count)
        {
            Debug.Log("BFS completed, amount of rooms: " + discovered.Count);
        }
    }

    [Button("DFS", EButtonEnableMode.Playmode)]
    public void DFS()
    {
        int currentNode = 1;

        HashSet<int> discovered = new();
        Stack<int> stack = new();
        stack.Push(currentNode);

        while (stack.Count > 0)
        {
            currentNode = stack.Pop();
            discovered.Add(currentNode);
            foreach (var neighbor in GetNeighbors(currentNode))
            {
                if (!discovered.Contains(neighbor))
                {
                    stack.Push(neighbor);
                    discovered.Add(neighbor);
                }
            }
        }

        if (discovered.Count == connectedRooms.Count)
        {
            Debug.Log("DFS completed, amount of rooms: " + discovered.Count);
        }
    }
}
