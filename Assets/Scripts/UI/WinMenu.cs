using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public static WinMenu staticWinMenu;
    public GameObject WinMenuUI;

    private void Start()
    {
        staticWinMenu = this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Victory()
    {
        WinMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResetGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadSettingMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
