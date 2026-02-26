using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
            if (generationTypes == GenerationType.Horizontal)
            {
                SplitHorizontal(toDo[listNumber]);
            }
            else if (generationTypes == GenerationType.Vertical)
            {
                SplitVertical(toDo[listNumber]);
            }
            else if (generationTypes == GenerationType.both)
            {
                SplitMixed(toDo[listNumber]);
            }
        }

        foreach (RectInt room in toDo) AlgorithmsUtils.DebugRectInt(room, Color.green);

    }

    private void SplitHorizontal(RectInt startRoom)
    {
        Debug.Log("INPUT:" + startRoom);


        int widthRoom1 = Random.Range(0 + minRoomSize , startRoom.width - minRoomSize);

        if (startRoom.width > minRoomSize * 2)
        {
            RectInt room1 = new RectInt(startRoom.x, startRoom.y, widthRoom1, height);
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

    }

    private void SplitVertical(RectInt startRoom)
    {
        Debug.Log("Input:" + startRoom);

        int lengthRoom1 = Random.Range(0 + minRoomSize, startRoom.height - minRoomSize);

        if (startRoom.height > minRoomSize * 2)
        {
            RectInt room1 = new RectInt(startRoom.x, startRoom.y, width, lengthRoom1);
            RectInt room2 = new RectInt(startRoom.x, startRoom.y + lengthRoom1, width, (startRoom.height - room1.height));
            room1.height++;

            toDo.Add(room1);
            toDo.Add(room2);
        }
        else
        {
            done.Add(startRoom);
        }
        listNumber++;
    }

    private void SplitMixed(RectInt startRoom)
    {
        
    }

}
