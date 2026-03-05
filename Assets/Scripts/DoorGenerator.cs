using System.Collections.Generic;
using UnityEngine;

public class DoorGenerator : MonoBehaviour
{
    private DungeonGenerator dungeonGenerator;

    private List<RectInt> toDo = new();
    private List<RectInt> done = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
        toDo = dungeonGenerator.done;
    }
    private void Update()
    {
        foreach (RectInt room in done)
        {
            AlgorithmsUtils.DebugRectInt(room, Color.magenta);
        }
    }
    public void StartDoorGeneration()
    {
        for (int i = 0; i < toDo.Count; i++)
        {
            for (int j = i + 1; j < toDo.Count; j++)
            {
               RectInt overlap = AlgorithmsUtils.Intersect(toDo[i], toDo[j]);
               done.Add(overlap);
            }
        }


    }
}
