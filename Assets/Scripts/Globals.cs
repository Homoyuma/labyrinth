using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    //gamma
    public static int mazeWidth = 3;
    public static int mazeHeight = 3;
    //delta
    public static int triangleMazeLength = 7;
    //theta
    public static int thetaMazeRadius = 7;
    public static int numberOfThetaCells = 16;
    //change later
    public static int mazeType = 1;
    public static List<Cell> cellArray1 = new List<Cell>();
    public static List<Cell> cellArray2 = new List<Cell>();
    public static List<Cell> cellArray3 = new List<Cell>();
    public static List<Cell> cellArray4 = new List<Cell>();
    public static FinishWall finishWall1;
    public static FinishWall finishWall2;
    public static FinishWall finishWall3;
    public static FinishWall finishWall4;
    public static MazeSpawner spawner1;
    public static MazeSpawner spawner2;
    public static MazeSpawner spawner3;
    public static MazeSpawner spawner4;

    public static int getNumberOfCellsInRow(int x)
    {
        int cells = numberOfThetaCells / 4;

        while (x + 1 >= cells)
        {
            cells *= 2;
        }

        return cells * 4;
    }
}
