using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardPathFinder : MonoBehaviour
{
    private int[,] grid;
    private List<Vector2Int> path= new List<Vector2Int>();
    Vector2Int start = new Vector2Int(0,0);
    private readonly Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0) };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = new int[3, 4] { { 1, 1, 1, 1 }, 
                               { 1, 1, 1, 1 }, 
                               { 1, 1, 1, 1 } };
        foreach (var pos in path)
        {
            Debug.Log($"°æ·Î :({pos.x},{pos.y})");
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DFS(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x > 4 || pos.y > 3 || grid[pos.y, pos.x] == 0)
            return;
        grid[pos.y, pos.x] = 0;
        path.Add(pos);
        foreach (var dir in directions)
        {
            Vector2Int nextPos = pos + dir;
            DFS(nextPos);
        

        }
    
    
    }
}
