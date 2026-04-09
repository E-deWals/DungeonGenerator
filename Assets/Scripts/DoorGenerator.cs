using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class DoorGenerator : MonoBehaviour
{
    [SerializeField] private int doorwidth;
    [SerializeField] private int roomToCheck;
    private DungeonGenerator dungeonGenerator;

    [HideInInspector]public List<RectInt> roomList = new();
    [HideInInspector]public List<RectInt> doors = new();
    public Dictionary<int, List<int>> connectedRooms = new();
    public Dictionary<int, List<int>> fromDoorToRoom = new();

    private NodeGenerator nodeGenerator;

    private void Awake()
    {
        nodeGenerator = GetComponent<NodeGenerator>();
        dungeonGenerator = GetComponent<DungeonGenerator>();
        roomList = dungeonGenerator.done;
    }
    private void Update()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(doors[i], i == 0 ? Color.blue : Color.magenta);
        }
    }
    [Button("Generate door", EButtonEnableMode.Playmode)]
    public void StartDoorGeneration()
    {
        doors.Clear();
        connectedRooms.Clear();
        fromDoorToRoom.Clear();
        for (int fromRoom = 0; fromRoom < roomList.Count; fromRoom++)
        {
            for (int toRoom = fromRoom + 1; toRoom < roomList.Count; toRoom++)
            {
                RectInt overlap = AlgorithmsUtils.Intersect(roomList[fromRoom], roomList[toRoom]);
                if (overlap.width > overlap.height)
                {
                    //moves corner right
                    overlap.x += overlap.height;
                    overlap.width -= overlap.height * 2;

                    if (overlap.width < doorwidth)
                    {
                        continue;
                    }

                    int newX = Random.Range(overlap.xMin, overlap.xMax - doorwidth);
                    overlap.x = newX;
                    overlap.width = doorwidth;

                }
                else
                {
                    //moves corner up
                    overlap.y += overlap.width;
                    overlap.height -= overlap.width * 2;

                    if (overlap.height < doorwidth)
                    {
                        continue;
                    }

                    int newY = Random.Range(overlap.yMin, overlap.yMax - doorwidth);
                    overlap.y = newY;
                    overlap.height = doorwidth;
                }

                if (overlap.width != 0 && overlap.height != 0)
                {
                    doors.Add(overlap);
                    nodeGenerator.HandleNodes(fromRoom, toRoom);
                }
            }
        }
        //bootcamp code for showing notes
        //foreach (var node in connectedRooms)
        //{
        //    Debug.Log($"{node.Key}: {string.Join(", ", node.Value)}");
        //}
    }

    public List<int> GetNeighbors(int node)
    {
        return new List<int>(connectedRooms[node]);
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
