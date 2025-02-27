using UnityEngine;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
    public CardManager manager;
    private LineRenderer lineRenderer;
    
    private Vector2Int[] leftPriorityDirections = new Vector2Int[]
    {
        new Vector2Int(-1,0),
        new Vector2Int(0,1),
        new Vector2Int(0,-1),
        new Vector2Int(1,0)
    };

    public List<Card> FindPath(Card startCard, Card exitCard)
    {
        Card[,] grid = manager.cardGrid;
        if (grid == null || startCard == null || exitCard == null) return null;

        Vector2Int startPos = FindCardPosition(grid, startCard);
        Vector2Int exitPos = FindCardPosition(grid, exitCard);

        Queue<List<Vector2Int>> queue = new Queue<List<Vector2Int>>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(new List<Vector2Int> { startPos });
        visited.Add(startPos);

        while (queue.Count > 0)
        {
            List<Vector2Int> currentPath = queue.Dequeue();
            Vector2Int currentPos = currentPath[currentPath.Count - 1];

            if (currentPos == exitPos)
            {
                return ConvertToCardPath(currentPath, grid);
            }

            foreach (Vector2Int dir in leftPriorityDirections)
            {
                Vector2Int nextPos = currentPos + dir;
                if (IsValidPosition(nextPos, grid) && !visited.Contains(nextPos))
                {
                    List<Vector2Int> newPath = new List<Vector2Int>(currentPath) { nextPos };
                    queue.Enqueue(newPath);
                    visited.Add(nextPos);
                }
            }
        }
        return null;
    }

    public Vector2Int FindCardPosition(Card[,] grid, Card targetCard)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, y] == targetCard)
                    return new Vector2Int(x, y);
            }
        }
        return new Vector2Int(-1, -1); // 오류 방지를 위해 -1 반환
    }

    private bool IsValidPosition(Vector2Int pos, Card[,] grid)
    {
        return pos.x >= 0 && pos.x < grid.GetLength(0) &&
               pos.y >= 0 && pos.y < grid.GetLength(1) &&
               grid[pos.x, pos.y] != null;
    }

    private List<Card> ConvertToCardPath(List<Vector2Int> path, Card[,] grid)
    {
        List<Card> cardPath = new List<Card>();
        foreach (Vector2Int pos in path)
        {
            cardPath.Add(grid[pos.x, pos.y]);
        }
        return cardPath;
    }

    private void DrawPath(List<Card> path)
    {
        if (path == null || path.Count == 0) return;

        lineRenderer.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, path[i].transform.position);
        }
    }

    public void ShowPath(Card startCard, Card exitCard)
    {
        List<Card> path = FindPath(startCard, exitCard);
        DrawPath(path);
    }

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 0;
    }
}
