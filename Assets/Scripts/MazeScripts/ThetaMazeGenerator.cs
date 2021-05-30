using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThetaMazeCell
{
    //index in array
    public int indexX;
    public int indexY;
    //polar coordinates
    public float r;
    public float angle;

    public bool WallBottom = true;
    public bool WallRight = true;

    public bool Visited = false;
    public int DistanceFromStart;

    public bool isFinishCell = false;
}
public class ThetaMazeGenerator
{
    public int radius = PlayerPrefs.GetInt("radius") + 1;

    public int startCell;

    public ThetaMazeCell[,] GenerateMaze()
    {
        ThetaMazeCell[,] maze = new ThetaMazeCell[radius, GameManager.getInstance().getNumberOfCellsInRow(radius)];
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.getInstance().getNumberOfCellsInRow(x); y++)
            {
                maze[x, y] = new ThetaMazeCell
                {
                    indexX = x,
                    indexY = y,
                    r = (float)(x * 1.5),
                    angle = y * 2 * Mathf.PI / (Mathf.PI / ((float)180.0 / GameManager.getInstance().getNumberOfCellsInRow(x)))
                };
            }
        }

        RemoveInnerCircle(maze);

        RemoveWallsWithBacktracker(maze);

        RemoveOuterWalls(maze);

        PlaceExit(maze);

        return maze;
    }
    private void PlaceExit(ThetaMazeCell[,] maze)
    {
        ThetaMazeCell furthest = maze[radius - 2, 0];
        for (int y = 0; y < GameManager.getInstance().getNumberOfCellsInRow(radius - 2); y++)
        {
            if (maze[radius - 2, y].DistanceFromStart > furthest.DistanceFromStart)
                furthest = maze[radius - 2, y];
        }
        int x0 = furthest.indexX;
        int y0 = furthest.indexY;
        if (GameManager.getInstance().getNumberOfCellsInRow(x0) == GameManager.getInstance().getNumberOfCellsInRow(x0 + 1))
        {
            maze[x0 + 1, y0].isFinishCell = true;
            maze[x0 + 1, y0].WallBottom = false;
        }
        else
        {
            maze[x0 + 1, y0 * 2].isFinishCell = true;
            maze[x0 + 1, y0 * 2].WallBottom = false;
        }
    }
    private void RemoveWallsWithBacktracker(ThetaMazeCell[,] maze)
    {
        ThetaMazeCell current = maze[0, startCell];
        current.Visited = true;
        current.DistanceFromStart = 0;

        Stack<ThetaMazeCell> stack = new Stack<ThetaMazeCell>();
        do
        {
            List<ThetaMazeCell> unvisitedNeighbours = new List<ThetaMazeCell>();

            int x = current.indexX;
            int y = current.indexY;

            List<ThetaMazeCell> neighbours = getNeighbours(maze, x, y);
            foreach(ThetaMazeCell cell in neighbours)
            {
                if (!cell.Visited) unvisitedNeighbours.Add(cell);
            }
            neighbours.Clear();

            if (unvisitedNeighbours.Count > 0)
            {
                ThetaMazeCell chosen = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
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
    private List<ThetaMazeCell> getNeighbours(ThetaMazeCell[,] maze, int x, int y)
    {
        List<ThetaMazeCell> neighbours = new List<ThetaMazeCell>();

        int numberOfCells = GameManager.getInstance().getNumberOfCellsInRow(x);
        //circle below
        if (x > 0)
        {
            if (numberOfCells == GameManager.getInstance().getNumberOfCellsInRow(x - 1))
            {
                neighbours.Add(maze[x - 1, y]);
            }
            else
            {
                neighbours.Add(maze[x - 1, y / 2]);
            }
        }
        //same circle
        neighbours.Add(maze[x, (y + 1) % numberOfCells]);
        if (y > 0)
        {
            neighbours.Add(maze[x, y - 1]);
        }
        else
        {
            neighbours.Add(maze[x, numberOfCells - 1]);
        }
        //circle above
        if (x < radius - 2)
        {
            if (numberOfCells == GameManager.getInstance().getNumberOfCellsInRow(x + 1))
            {
                neighbours.Add(maze[x + 1, y]);
            }
            else
            {
                neighbours.Add(maze[x + 1, y * 2]);
                neighbours.Add(maze[x + 1, y * 2 + 1]);
            }
        }
        return neighbours;
    }
    private void RemoveOuterWalls(ThetaMazeCell[,] maze)
    {
        for (int y = 0; y < GameManager.getInstance().getNumberOfCellsInRow(radius - 1); y++)
        {
            maze[radius - 1, y].WallRight = false;
        }
    }
    private void RemoveWall(ThetaMazeCell a, ThetaMazeCell b)
    {
        if (a.indexX > b.indexX)
        {
            a.WallBottom = false;
        }
        else if (b.indexX > a.indexX)
        {
            b.WallBottom = false;
        }
        if (a.indexX == b.indexX)
        {
            if (!(a.indexY == 0 && b.indexY == GameManager.getInstance().getNumberOfCellsInRow(b.indexX) - 1) 
                && !(b.indexY == 0 && a.indexY == GameManager.getInstance().getNumberOfCellsInRow(a.indexX) - 1))
            {
                if (a.indexY > b.indexY)
                {
                    a.WallRight = false;
                }
                else
                {
                    b.WallRight = false;
                }
            }
            else if (a.indexY == 0)
            {
                a.WallRight = false;
            }
            else if (b.indexY == 0)
            {
                b.WallRight = false;
            }
        }
    }
    private void RemoveInnerCircle(ThetaMazeCell[,] maze)
    {
        startCell = UnityEngine.Random.Range(0, GameManager.getInstance().getNumberOfCellsInRow(0) - 1);
        
        maze[0, startCell].WallBottom = false;
    }
}
