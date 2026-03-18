using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class DoorGenerator : MonoBehaviour
{
    [SerializeField] private int doorwidth;
    [SerializeField] private int roomToCheck ;
    private DungeonGenerator dungeonGenerator;

    private List<RectInt> toDo = new();
    private List<RectInt> done = new();
    private Dictionary<int,List<int>> connectedRooms = new();
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
        toDo = dungeonGenerator.done;
    }
    private void Update()
    {
        for (int i = 0; i < done.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(done[i], i == 0 ? Color.blue : Color.magenta);
        }
    }
    [Button("Generate door", EButtonEnableMode.Playmode)]
    public void StartDoorGeneration()
    {
        done.Clear();
        for (int fromRoom = 0; fromRoom < toDo.Count; fromRoom++)
        {
            for (int toRoom = fromRoom + 1; toRoom < toDo.Count; toRoom++)
            {
                RectInt overlap = AlgorithmsUtils.Intersect(toDo[fromRoom], toDo[toRoom]);
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
                    HandleNotes(fromRoom, toRoom);
                    done.Add(overlap);
                }

            }
        }
        //bootcamp code for showing notes
        foreach (var node in connectedRooms)
        {
            Debug.Log($"{node.Key}: {string.Join(", ", node.Value)}");
        }
    }

    private void HandleNotes(int fromRoom, int toRoom)
    {
        if (!connectedRooms.ContainsKey(fromRoom))
        {
            connectedRooms[fromRoom] = new();
        }
        if (!connectedRooms.ContainsKey(toRoom))
        {
            connectedRooms[toRoom] = new();
        }

        connectedRooms[fromRoom].Add(toRoom);
        connectedRooms[toRoom].Add(fromRoom);

    }
}
