using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NodeGenerator : MonoBehaviour
{
    private List<RectInt> roomList = new();
    public Dictionary<int, List<int>> connectedRooms = new();
    public Dictionary<int, List<int>> fromDoorToRoom = new();

    private DoorGenerator doorGenerator;
    private DungeonGenerator dungeonGenerator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        roomList.Clear();
        connectedRooms.Clear();
        fromDoorToRoom.Clear();

        doorGenerator = GetComponent<DoorGenerator>();
        dungeonGenerator = GetComponent<DungeonGenerator>();
    }

    public IEnumerator HandleNodes(int fromRoom, int toRoom)
    {

        //connected rooms
        if (!connectedRooms.ContainsKey(fromRoom))
        {
            connectedRooms[fromRoom] = new();
        }
        if (!connectedRooms.ContainsKey(toRoom))
        {
            connectedRooms[toRoom] = new();
        }
        if (!fromDoorToRoom.ContainsKey(doorGenerator.doors.Count - 1))
        {
            fromDoorToRoom[doorGenerator.doors.Count - 1] = new();
        }

        connectedRooms[fromRoom].Add(toRoom);
        connectedRooms[toRoom].Add(fromRoom);

        fromDoorToRoom[doorGenerator.doors.Count - 1].Add(toRoom);
        fromDoorToRoom[doorGenerator.doors.Count - 1].Add(fromRoom);

        yield return new WaitForSeconds(0.1f);
    }
    private void OnDrawGizmos()
    {
        roomList = doorGenerator.roomList;

        if (connectedRooms.Count == 0)
        {
            return;
        }

        Gizmos.color = Color.yellow;

        foreach (int roomIndex in connectedRooms.Keys)
        {
            RectInt room = dungeonGenerator.done[roomIndex];

            Vector2 center = room.center;
            Vector3 center3D = new Vector3(center.x, 0, center.y);

            Gizmos.DrawSphere(center3D, 0.25f);
        }

        foreach (int doorIndex in fromDoorToRoom.Keys)
        {
            RectInt door = doorGenerator.doors[doorIndex];
            Vector2 doorCenter = door.center;
            Vector3 doorCenter3D = new Vector3(doorCenter.x, 0, doorCenter.y);


            Gizmos.DrawSphere(doorCenter3D, 0.25f);

            foreach (int roomIndex in fromDoorToRoom[doorIndex])
            {
                RectInt room = roomList[roomIndex];
                Vector2 roomCenter = room.center;
                Vector3 roomCenter3D = new Vector3(roomCenter.x, 0, roomCenter.y);

                Gizmos.DrawLine(doorCenter3D, roomCenter3D);
            }
        }
    }
}
