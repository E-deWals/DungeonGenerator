using System.Collections;
using System.Text;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;


public class TileMapGenerator : MonoBehaviour
{
    [SerializeField] private bool animate;
    [SerializeField] private UnityEvent onTileMapGenerated;

    private DungeonGenerator dungeonGenerator;
    private DoorGenerator doorGenerator;

    private GameObject parent; 

    [SerializeField] GameObject[] prefabsToSpawn = new GameObject[16];
    public int[,] _tileMap;

    private void Start()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
        doorGenerator = GetComponent<DoorGenerator>();
    }

    [Button("", EButtonEnableMode.Playmode)]
    public IEnumerator GenerateTileMap()
    {
        if (parent != null) { Destroy (parent); }

        parent = new GameObject ("walls");

        int[,] tileMap = new int[dungeonGenerator.GetBounds().height, dungeonGenerator.GetBounds().width];
        int rows = tileMap.GetLength(0);
        int cols = tileMap.GetLength(1);

        //Fill the map with empty spaces
        foreach (RectInt room in dungeonGenerator.done)
        {
            AlgorithmsUtils.FillRectangleOutline(tileMap, room, 1);
        }
        foreach (RectInt door in doorGenerator.doors)
        {
            AlgorithmsUtils.FillRectangle(tileMap, door, 0);
        }
        
        //goes through each row to check if it has to place a wall and if it does checks wich wall to place
        for (int i = 0; i < rows - 1; i++)
        {
            for (int j = 0; j < cols - 1; j++)
            {
                int caseToCheck = tileMap[i, j] * 8 + tileMap[i, j + 1] + tileMap[i + 1, j] * 4 + tileMap[i + 1, j + 1] * 2;
                if (prefabsToSpawn[caseToCheck] == null)
                {
                    continue;
                }

                GameObject wall = Instantiate(prefabsToSpawn[caseToCheck], new Vector3(j + 1f, 0, i + 1f), transform.rotation, parent.transform);
                wall.name = "wall coordinates: " + j + " , " + i ;
            }
            if (animate) { yield return new WaitForSeconds(0.1f); }
        }
        _tileMap = tileMap;
        onTileMapGenerated.Invoke();
    }

    public int[,] GetTileMap()
    {
        return _tileMap.Clone() as int[,];
    }
}
