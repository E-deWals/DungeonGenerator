using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 50;
    [SerializeField] private int minRoomSize = 5;
    [Tooltip("number between 0 and 1")] 
    [SerializeField] private float horizontalBias = 0.50f;

    List<RectInt> toDo = new();
    List<RectInt> done = new();
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

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        while (listNumber < toDo.Count)
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
        //}

        foreach (RectInt room in toDo) AlgorithmsUtils.DebugRectInt(room, Color.green);
        foreach (RectInt room in done) AlgorithmsUtils.DebugRectInt(room, Color.red);

    }

    private void SplitHorizontal(RectInt startRoom)
    {
        //Debug.Log("INPUT:" + startRoom);


        int widthRoom1 = Random.Range(0 + minRoomSize , startRoom.width - minRoomSize);

        if (startRoom.width > minRoomSize * 2)
        {
            RectInt room1 = new RectInt(startRoom.x, startRoom.y, widthRoom1, startRoom.height);
            RectInt room2 = new RectInt((room1.x + room1.width), startRoom.y, (startRoom.width - room1.width), startRoom.height);
            room1.width++;

            toDo.Add(room1);
            toDo.Add(room2);

        }
        else if (startRoom.height > minRoomSize * 2)
        {
            SplitVertical(startRoom);
        }
        else
        {
            done.Add(startRoom);
        }

    }

    private void SplitVertical(RectInt startRoom)
    {
        //Debug.Log("Input:" + startRoom);

        int lengthRoom1 = Random.Range(0 + minRoomSize, startRoom.height - minRoomSize);

        if (startRoom.height > minRoomSize * 2)
        {
            RectInt room1 = new RectInt(startRoom.x, startRoom.y, startRoom.width, lengthRoom1);
            RectInt room2 = new RectInt(startRoom.x, startRoom.y + lengthRoom1, startRoom.width, (startRoom.height - room1.height));
            room1.height++;

            toDo.Add(room1);
            toDo.Add(room2);
        }
        else if (startRoom.width > minRoomSize * 2)
        {
            SplitHorizontal(startRoom);
        }
        else
        {
            done.Add(startRoom);
        }
    }

    private void SplitMixed(RectInt startRoom)
    {
        float number = Random.value;
        if (number > horizontalBias)
        {
            SplitHorizontal(startRoom);
        }
        else
        {
            SplitVertical(startRoom);
        }
        listNumber++;
    }

}
