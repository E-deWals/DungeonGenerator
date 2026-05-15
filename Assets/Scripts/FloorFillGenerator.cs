using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.AI.Navigation;
using UnityEngine;

public class FloorFillGenerator : MonoBehaviour
{
   
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private NavMeshSurface navMeshSurface;

    private TileMapGenerator tileMapGenerator;
    private GameObject parent;


    private void Awake()
    {
        tileMapGenerator = GetComponent<TileMapGenerator>();
    }

    public void BFS(Vector2 startPosition)
    {
        if (parent != null) { Destroy(parent); }

        parent = new GameObject("floors");

        HashSet<Vector2> discovered = new();
        Queue<Vector2> queue = new();
        queue.Enqueue(startPosition);

        while (queue.Count > 0)
        {
            Vector2 currentTile = queue.Dequeue();
            discovered.Add(currentTile);
            if (tileMapGenerator._tileMap[((int)currentTile.y), ((int)currentTile.x)] == 0)
            {
                GameObject floor = Instantiate(prefabToSpawn,new Vector3(currentTile.x ,0, currentTile.y), transform.rotation, parent.transform);
                floor.name = "floor " + currentTile.x + " , " + currentTile.y;

                Vector2 up = new Vector2(currentTile.x, currentTile.y + 1);
                Vector2 down = new Vector2(currentTile.x, currentTile.y - 1);
                Vector2 right = new Vector2(currentTile.x + 1, currentTile.y);
                Vector2 left = new Vector2(currentTile.x - 1, currentTile.y);

                if (!discovered.Contains(up))
                {
                    queue.Enqueue(up);
                    discovered.Add(up);
                }

                if (!discovered.Contains(down))
                {
                    queue.Enqueue(down);
                    discovered.Add(down);
                }

                if (!discovered.Contains(right))
                {
                    queue.Enqueue(right);
                    discovered.Add(right);
                }

                if (!discovered.Contains(left))
                {
                    queue.Enqueue(left);
                    discovered.Add(left);
                }

            }
        }
        BakeNavMesh();
    }

    public void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
