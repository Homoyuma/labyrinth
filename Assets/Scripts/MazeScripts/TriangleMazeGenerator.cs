using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleMazeGeneratorCell
{
    public int indexX;
    public int indexY;

    public float X;
    public float Y;

    public bool WallLeft = true;
    public bool WallRight = true;
    public bool WallBottom = true;
    
    public bool Visited = false;
    public int DistanceFromStart;

    public bool isFinishCell = false;
}
public class TriangleMazeGenerator
{
    public int mazeLength = PlayerPrefs.GetInt("length");

    public TriangleMazeGeneratorCell[,] GenerateMaze()
    {
        TriangleMazeGeneratorCell[,] maze = new TriangleMazeGeneratorCell[mazeLength, mazeLength * 2];

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(0) * 2 - x * 2 - 1; y++) 
            {
                if (y % 2 == 0)
                    maze[x, y] = new TriangleMazeGeneratorCell
                    {
                        X = (float)((x + y * 0.25) * 1.5),
                        Y = (float)(y * Mathf.Sin(Mathf.PI / 3) * 1.5) / 2,
                        indexX = x,
                        indexY = y
                    };
                else
                    maze[x, y] = new TriangleMazeGeneratorCell
                    {
                        X = (float)((x + 1.25 + y * 0.25) * 1.5),
                        Y = (float)(((y + 1) * Mathf.Sin(Mathf.PI / 3)) * 1.5) / 2,
                        indexX = x,
                        indexY = y
                    };

            }
        }
        removeWallsWithBacktracker(maze);

        placeExit(maze);

        return maze;
    }
    private void removeWallsWithBacktracker(TriangleMazeGeneratorCell[,] maze)
    {
        TriangleMazeGeneratorCell current = maze[0, 0];
        current.Visited = true;
        current.DistanceFromStart = 0;

        Stack<TriangleMazeGeneratorCell> stack = new Stack<TriangleMazeGeneratorCell>();
        do
        {
            List<TriangleMazeGeneratorCell> unvisitedNeighbours = new List<TriangleMazeGeneratorCell>();

            int x = current.indexX;
            int y = current.indexY;
            if (y < mazeLength * 2 - (x + 1) * 2 && !maze[x, y + 1].Visited) unvisitedNeighbours.Add(maze[x, y + 1]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedNeighbours.Add(maze[x, y - 1]);
            if (y % 2 == 0)
            {
                if (x > 0 && y < mazeLength * 2 - 2 * (x + 1) && !maze[x - 1, y + 1].Visited) unvisitedNeighbours.Add(maze[x - 1, y + 1]);
            }
            else if (x < mazeLength - 1 && y > 0 && !maze[x + 1, y - 1].Visited) unvisitedNeighbours.Add(maze[x + 1, y - 1]);
            

            if (unvisitedNeighbours.Count > 0)
            {
                TriangleMazeGeneratorCell chosen = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, chosen);
                chosen.Visited = true;
                current = chosen;
                stack.Push(chosen);
                chosen.DistanceFromStart = stack.Count;
            }
            else
            {
                current = stack.Pop();
            }

        } while (stack.Count > 0);
    }
    private void RemoveWall(TriangleMazeGeneratorCell a, TriangleMazeGeneratorCell b)
    {
        if (a.indexX == b.indexX)
        {
            if (a.indexY > b.indexY)
            {
                if (a.indexY % 2 == 0)
                {
                    a.WallBottom = false;
                    b.WallBottom = false;
                }
                else
                {
                    a.WallRight = false;
                    b.WallRight = false;
                }
            }
            else
            {
                if (b.indexY % 2 == 0)
                {
                    a.WallBottom = false;
                    b.WallBottom = false;
                }
                else
                {
                    a.WallRight = false;
                    b.WallRight = false;
                }
            }

        }
        else
        {
            a.WallLeft = false;
            b.WallLeft = false;
        }
    }
    
    private void placeExit(TriangleMazeGeneratorCell[,] maze)
    {
        //finding furthest cell
        TriangleMazeGeneratorCell furthest = maze[0, 0];

        for (int x = 0; x < maze.GetLength(0); x++) 
        {
            if (maze[x, 0].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, 0];
        }
        for (int y = 0; y < maze.GetLength(0); y++)
        {
            if (maze[0, y].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[0, y * 2];
        }

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            if (maze[x, mazeLength * 2 - (x + 1) * 2].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, mazeLength * 2 - (x + 1) * 2];
        }
        furthest.isFinishCell = true;
        //determining which side it's on
        if (furthest.indexX == 0)
        {
            furthest.WallLeft = false;
        }
        else if (furthest.indexY == 0)
        {
            furthest.WallBottom = false;
        }
        else
        {
            furthest.WallRight = false;
        }
    }
}
