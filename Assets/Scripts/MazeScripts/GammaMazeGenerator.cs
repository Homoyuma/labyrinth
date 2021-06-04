using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorCell
{
    public int X;
    public int Y;

    //this is for ordinary cell
    public bool WallLeft = true;
    public bool WallBottom = true;
    //this is for finish cell only
    public bool WallRight = false;
    public bool WallTop = false;

    public bool Visited = false;
    public int DistanceFromStart = -1;

    public bool IsFinishCell= false;
}
public class MazeGenerator
{
    public int width = PlayerPrefs.GetInt("width") + 1;
    public int height = PlayerPrefs.GetInt("height") + 1;
    public MazeGeneratorCell[,] GenerateMaze()
    {
        MazeGeneratorCell[,] maze = new MazeGeneratorCell[width, height];

        for(int x=0; x<maze.GetLength(0); x++)
        {
            for(int y=0; y<maze.GetLength(1);y++)
            {
                maze[x, y] = new MazeGeneratorCell { X = x, Y = y };
            }
        }

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            maze[x, height - 1].WallLeft = false;
        }
        for (int y = 0; y < maze.GetLength(1); y++)
        {
            maze[width - 1, y].WallBottom = false;
        }

        RemoveWallsWithBacktracker(maze);
        //BinaryTreeAlgorithm(maze);

        PlaceMazeExit(maze);

        return maze;
    }

    private void RemoveWallsWithBacktracker(MazeGeneratorCell[,] maze)
    {
        Globals.startX = UnityEngine.Random.Range(0, width - 1);
        Globals.startY = UnityEngine.Random.Range(0, height - 1);
        MazeGeneratorCell current = maze[0, 0];
        current.Visited = true;
        current.DistanceFromStart = 0;

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();
        do
        {
            List<MazeGeneratorCell> unvisitedNeighbours = new List<MazeGeneratorCell>();

            int x = current.X;
            int y = current.Y;

            if (x > 0 && !maze[x - 1, y].Visited) unvisitedNeighbours.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedNeighbours.Add(maze[x, y - 1]);
            if (x < width - 2 && !maze[x + 1, y].Visited) unvisitedNeighbours.Add(maze[x + 1, y]);
            if (y < height - 2 && !maze[x, y + 1].Visited) unvisitedNeighbours.Add(maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0)
            {
                MazeGeneratorCell chosen = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
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

        } while (stack.Count>0);
    }
    private void BinaryTreeAlgorithm(MazeGeneratorCell[,] maze)
    {
        for (int x = 0; x < maze.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < maze.GetLength(1) - 1; y++)
            {
                if (x > 0 && y > 0)
                {
                    int random = UnityEngine.Random.Range(1, 3);
                    if (random == 1)
                    {
                        maze[x, y].WallLeft = false;
                    }
                    else
                    {
                        maze[x, y].WallBottom = false;
                    }
                }
                else if (x > 0 && y == 0)
                {
                    maze[x, y].WallLeft = false;
                }
                else if (x == 0 && y > 0)
                {
                    maze[x, y].WallBottom = false;
                }
            }
        }
        maze[0, 0].DistanceFromStart = 0;

        List<MazeGeneratorCell> list = new List<MazeGeneratorCell>();

        for (int x = 0; x < maze.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < maze.GetLength(1) - 1; y++)
            {
                if (x > 0 && maze[x, y].WallLeft == false) list.Add(maze[x - 1, y]);
                if (y > 0 && maze[x, y].WallBottom == false) list.Add(maze[x, y - 1]);
                
                foreach (MazeGeneratorCell cell in list)
                {
                    if (cell.DistanceFromStart < maze[x, y].DistanceFromStart || maze[x, y].DistanceFromStart == -1)
                        maze[x, y].DistanceFromStart = cell.DistanceFromStart + 1;
                }
                list.Clear();
            }
        }
    }
    
    private void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        if (a.X == b.X)
        {
            if (a.Y > b.Y) a.WallBottom = false;
            else b.WallBottom = false;
        }
        else
        {
            if (a.X > b.X) a.WallLeft = false;
            else b.WallLeft = false;
        }
    }

    private void PlaceMazeExit(MazeGeneratorCell[,] maze)
    {
        //finding furthest cell
        MazeGeneratorCell furthest = maze[Globals.startX, Globals.startY];

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            if (maze[x, height - 2].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, height - 2];
            if (maze[x, 0].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, 0];
        }

        for (int y = 0; y < maze.GetLength(1); y++)
        {
            if (maze[width - 2, y].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[width - 2, y];
            if (maze[0, y].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[0, y];
        }
        //determining which side it's on
        if (furthest.X == 0)
        {
            furthest.WallLeft = false;
            furthest.IsFinishCell = true;
        }
        else if (furthest.Y == 0)
        {
            furthest.WallBottom = false;
            furthest.IsFinishCell = true;
        }
        else if (furthest.X == width - 2)
        {
            maze[furthest.X + 1, furthest.Y].WallLeft = false;
            maze[furthest.X + 1, furthest.Y].IsFinishCell = true;
        }
        else if (furthest.Y == height - 2)
        {
            maze[furthest.X, furthest.Y + 1].WallBottom = false;
            maze[furthest.X, furthest.Y + 1].IsFinishCell = true;
        }
    }
}
