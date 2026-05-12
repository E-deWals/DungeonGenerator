using System.Text;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(3)]
public class TileMapGenerator : MonoBehaviour
{

    [SerializeField]
    private UnityEvent onTileMapGenerated;

    [SerializeField]
    DungeonGenerator dungeonGenerator;
    DoorGenerator doorGenerator;

    [SerializeField] GameObject[] prefabsToSpawn = new GameObject[16];
    private int[,] _tileMap;

    private void Start()
    {
        dungeonGenerator = GetComponent<DungeonGenerator>();
        doorGenerator = GetComponent<DoorGenerator>();
    }

    [Button]
    public void GenerateTileMap()
    {
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
        

        for (int i = 0; i < rows - 1; i++)
        {
            for (int j = 0; j < cols - 1; j++)
            {
                int caseToCheck = tileMap[i, j] * 8 + tileMap[i, j + 1] + tileMap[i + 1, j] * 4 + tileMap[i + 1, j + 1] * 2;
                if (prefabsToSpawn[caseToCheck] == null)
                {
                    continue;
                }

                Instantiate(prefabsToSpawn[caseToCheck], new Vector3(j + 1f, 0, i + 1f), transform.rotation);

            }
        }
        _tileMap = tileMap;

        onTileMapGenerated.Invoke();
    }

    public string ToString(bool flip)
    {
        if (_tileMap == null) return "Tile map not generated yet.";

        int rows = _tileMap.GetLength(0);
        int cols = _tileMap.GetLength(1);

        var sb = new StringBuilder();

        int start = flip ? rows - 1 : 0;
        int end = flip ? -1 : rows;
        int step = flip ? -1 : 1;

        for (int i = start; i != end; i += step)
        {
            for (int j = 0; j < cols; j++)
            {
                sb.Append((_tileMap[i, j] == 0 ? '0' : '#')); //Replaces 1 with '#' making it easier to visualize
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public int[,] GetTileMap()
    {
        return _tileMap.Clone() as int[,];
    }

    [Button]
    public void PrintTileMap()
    {
        Debug.Log(ToString(true));
    }


}
