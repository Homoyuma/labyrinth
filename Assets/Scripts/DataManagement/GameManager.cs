using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //data
    public int type;
    //gamma
    public int width;
    public int heigth;
    //delta
    public int length;
    //theta
    public int radius;

    private static GameManager instance;
    private GameManager() { }
    public static GameManager getInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public int getNumberOfCellsInRow(int x)
    {
        //add later to json data
        int cells = 16 / 4;

        while (x + 1 >= cells)
        {
            cells *= 2;
        }

        return cells * 4;
    }
}
