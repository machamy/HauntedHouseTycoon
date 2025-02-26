using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
   
    public List<Card> FindPath(Card startCard,Card exitCard)
    {
        Card[,] grid = manager.cardGrid;
        if (grid == null) return null;
        Vector2Int startPos = FindCardPosition(grid,startCard);
        Vector2Int exitPos= FindCardPosition(grid, exitCard);
        Queue<List<Vector2Int>> queue = new Queue<List<Vector2Int>>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        queue.Enqueue(new List<Vector2Int>{ startPos});
        visited.Add(startPos);
        while (queue.Count > 0)
        {
            List<Vector2Int> currentPath = queue.Dequeue();
            Vector2Int currentPos = currentPath[currentPath.Count-1];
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
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (grid[x, y] == targetCard)
                    return new Vector2Int(x, y);
            
            }
        
        }
        return new Vector2Int(0,0);

    
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
