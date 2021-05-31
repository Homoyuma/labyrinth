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
    public const int n = 10;
    public static List<Cell>[] cellArray = new List<Cell>[n];
    public static FinishWall[] finishWall = new FinishWall[n];
    public static MazeSpawner[] spawner = new MazeSpawner[n];

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
