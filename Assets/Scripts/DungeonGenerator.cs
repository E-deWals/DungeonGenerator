using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 50;
    [SerializeField] private int minRoomSize = 5;

    List<RectInt> toDo = new List<RectInt>();
    List<RectInt> done = new List<RectInt>();
    private enum GenerationType {Horizontal, Vertical, both, random}
    [SerializeField] private GenerationType generationTypes;

    private int listNumber = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RectInt startRoom = new RectInt(0, 0, width, height);
        toDo.Add(startRoom);
    }

    // Update is called once per frame
    void Update()
    {

        //RectInt startRoom = new RectInt(0, 0, width, height);
        //AlgorithmsUtils.DebugRectInt(startRoom, Color.magenta);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (generationTypes == GenerationType.Vertical)
            {
                SplitVertically(toDo[listNumber]);
            }
            else if (generationTypes == GenerationType.Horizontal)
            {
                SplitHorizontally(toDo[listNumber]);
            }
            else if (generationTypes == GenerationType.both)
            {
                SplitMixed(toDo[listNumber]);
            }
        }

        foreach (RectInt room in toDo) AlgorithmsUtils.DebugRectInt(room, Color.green);

    }

    private void SplitVertically(RectInt startRoom)
    {
        Debug.Log("INPUT:" + startRoom);


        int r1w = Random.Range(0 + minRoomSize , startRoom.width - minRoomSize);

        if (startRoom.width > minRoomSize * 2)
        {
            RectInt room1 = new RectInt(startRoom.x, startRoom.y, r1w, height);
            RectInt room2 = new RectInt((room1.x + room1.width), startRoom.y, (startRoom.width - room1.width), height);
            room1.width++;

            toDo.Add(room1);
            toDo.Add(room2);

        }
        else
        { 
            done.Add(startRoom);
        }

        listNumber++;

        //AlgorithmsUtils.DebugRectInt(room1, Color.cyan);
        //AlgorithmsUtils.DebugRectInt(room2, Color.green);

        //RectInt room12 = new RectInt(room1.x, room1.y, (room1.width / 2) + 1, room1.height);
        //AlgorithmsUtils.DebugRectInt(room12, Color.blue);

        //RectInt room11 = new RectInt((room1.xMin + room1.xMax) / 2, room1.y, (room1.width / 2) + 1, room1.height);
        //AlgorithmsUtils.DebugRectInt(room11, Color.black);

        //RectInt room21 = new RectInt(room2.x, room2.y, (room2.width / 2) + 1, room2.height);
        //AlgorithmsUtils.DebugRectInt(room21, Color.yellow);

        //RectInt room22 = new RectInt((room2.xMin + room2.xMax) / 2, room2.y, (room2.width / 2) + 1, room2.height);
        //AlgorithmsUtils.DebugRectInt(room22, Color.white);

        //RectInt room222 = new RectInt((room22.xMin + room22.xMax) / 2, room22.y, (room22.width / 2) + 1, room22.height);
        //AlgorithmsUtils.DebugRectInt(room222, Color.red);


        //RectInt theoryRoomVerticalRight = new RectInt(currentRoom.x, currentRoom.y, (currentRoom.width / 2) + 1, currentRoom.height)
        //RectInt theoryRoomVerticalLeft = new RectInt((currentRoom.xMin + currentroom.xMax) / 2, currentRoom.y, (currentRoom.width / 2) + 1, currentRoom.height)

    }

    private void SplitHorizontally(RectInt startRoom)
    {
        RectInt room1 = new RectInt(startRoom.x, startRoom.y, width, (height / 2) + 1);
        AlgorithmsUtils.DebugRectInt(room1, Color.cyan);

        RectInt room2 = new RectInt(startRoom.x, (startRoom.yMin + startRoom.yMax) / 2, width, (height / 2));
        AlgorithmsUtils.DebugRectInt(room2, Color.green);

        RectInt room11 = new RectInt(room1.x, room1.y, room1.width, (room1.height / 2) + 1);
        AlgorithmsUtils.DebugRectInt(room11, Color.black);

        RectInt room12 = new RectInt(room1.x, (room1.yMin + room1.yMax) / 2, room1.width, (room1.height / 2));
        AlgorithmsUtils.DebugRectInt(room12, Color.blue);

        RectInt room21 = new RectInt(room2.x, room2.y, room2.width, (room2.height / 2) + 1);
        AlgorithmsUtils.DebugRectInt(room21, Color.yellow);

        RectInt room22 = new RectInt(room1.x, (room2.yMin + room2.yMax) / 2, room2.width, (room2.height / 2));
        AlgorithmsUtils.DebugRectInt(room22, Color.white);

        //RectInt theoryRoomHorizontalTop new RectInt(currentroom.x, (currentroom.yMin + currentroom.yMax) / 2, currentroom.width, (currentroom.height / 2));
        //RectInt theoryRoomHorizontalBottom new RectInt(currentroom.x, currentroom.y, currentroom.width, (currentroom.height / 2) + 1)
    }

    private void SplitMixed(RectInt startRoom)
    {
        RectInt room1 = new RectInt(startRoom.x, startRoom.y, width, (height / 2) + 1);
        AlgorithmsUtils.DebugRectInt(room1, Color.cyan);

        RectInt room2 = new RectInt(startRoom.x, (startRoom.yMin + startRoom.yMax) / 2, width, (height / 2));
        AlgorithmsUtils.DebugRectInt(room2, Color.green);

        RectInt room12 = new RectInt(room1.x, room1.y, (room1.width / 2) + 1, room1.height);
        AlgorithmsUtils.DebugRectInt(room12, Color.blue);

        RectInt room11 = new RectInt((room1.xMin + room1.xMax) / 2, room1.y, (room1.width / 2), room1.height);
        AlgorithmsUtils.DebugRectInt(room11, Color.black);

        RectInt room21 = new RectInt(room2.x, room2.y, room2.width, (room2.height / 2) + 1);
        AlgorithmsUtils.DebugRectInt(room21, Color.white);

        RectInt room22 = new RectInt(room2.x, (room2.yMin + room2.yMax) / 2, room2.width, (room2.height / 2) +1);
        AlgorithmsUtils.DebugRectInt(room22, Color.yellow);
    }

}
