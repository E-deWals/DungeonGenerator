using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class DoorGenerator : MonoBehaviour
{
    [SerializeField] private bool animate;
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

        nodeGenerator.enabled = true;
        roomList = dungeonGenerator.done;
    }
    private void Update()
    {
        //draws the doors
        for (int i = 0; i < doors.Count; i++)
        {
            AlgorithmsUtils.DebugRectInt(doors[i], i == 0 ? Color.blue : Color.magenta);
        }
    }
    [Button("Generate door", EButtonEnableMode.Playmode)]
    public IEnumerator StartDoorGeneration()
    {
        doors.Clear();
        fromDoorToRoom.Clear();

        //compares every room to every room other then itself and sees if there is an overlap
        //creates a door based on said overlap and makes sure that the placement is valid 
        for (int fromRoom = 0; fromRoom < roomList.Count; fromRoom++)
        {
            for (int toRoom = fromRoom + 1; toRoom < roomList.Count; toRoom++)
            {
                RectInt overlap = AlgorithmsUtils.Intersect(roomList[fromRoom], roomList[toRoom]);
                if (overlap.width > overlap.height)
                {
                    //moves corner right to prevent corner doors
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
                    //moves corner up to prevent corner doors
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
                    StartCoroutine(nodeGenerator.HandleNodes(fromRoom, toRoom));    
                }
                if (animate) { yield return new WaitForSeconds(0.1f); }
            }
        }
    }
}
