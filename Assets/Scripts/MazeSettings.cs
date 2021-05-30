using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MazeSettings : MonoBehaviour
{
    //delta settings
    public TMP_InputField widthInput;
    public TMP_InputField heightInput;
    //gamma settings
    public TMP_InputField lengthInput;
    //theta settings
    public TMP_InputField radiusInput;
    public TMP_Dropdown dropdown;

    public GameObject gammaSettings;
    public GameObject deltaSettings;
    public GameObject thetaSettings;

    public JsonHandler.DataCollection data;

    string dropdownText = "gamma";

    public void StartGame()
    {
        JsonHandler.getInstance().LoadField();
        data = new JsonHandler.DataCollection 
        {
            type = PlayerPrefs.GetInt("type"),
            gammaHeight = PlayerPrefs.GetInt("height"),
            gammaWidth = PlayerPrefs.GetInt("width"),
            deltaLength = PlayerPrefs.GetInt("length"),
            thetaRadius = PlayerPrefs.GetInt("radius")
        };
        getInputs();
        if (inputsAreCorrect())
        {
            setGlobalVariables();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void getInputs()
    {
        if (dropdownText == "gamma")
        {
            if (widthInput.text != "")
            {
                data.gammaWidth = int.Parse(widthInput.text);
            }

            if (heightInput.text != "")
            {
                data.gammaHeight = int.Parse(heightInput.text);
            }
            data.type = 1;
        }
        if (dropdownText == "delta")
        {
            if (lengthInput.text != "")
            {
                data.deltaLength = int.Parse(lengthInput.text);
            }
            data.type = 2;
        }
        if (dropdownText == "theta")
        {
            if (radiusInput.text != "")
            {
                data.thetaRadius = int.Parse(radiusInput.text);
            }
            data.type = 3;
        }
    }
    public bool inputsAreCorrect()
    {
        bool flag = true;
        if (dropdownText == "gamma")
        {
            if (data.gammaWidth <= 1) flag = false;
            if (data.gammaHeight <= 1) flag = false;
        }
        if (dropdownText == "delta")
        {
            if (data.deltaLength <= 3) flag = false;
        }
        if (dropdownText == "theta")
        {
            if (data.thetaRadius <= 0) flag = false;
        }
        return flag;
    }
    public void setGlobalVariables()
    {
        PlayerPrefs.SetInt("type", data.type);
        PlayerPrefs.SetInt("height", data.gammaHeight);
        PlayerPrefs.SetInt("width", data.gammaWidth);
        PlayerPrefs.SetInt("length", data.deltaLength);
        PlayerPrefs.SetInt("radius", data.thetaRadius);
    }
    public void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        dropdownText = dropdown.options[index].text.ToLower();

        switch (dropdownText)
        {
            case "gamma":
                gammaSettings.SetActive(true);
                deltaSettings.SetActive(false);
                thetaSettings.SetActive(false);
                break;
            case "delta":
                deltaSettings.SetActive(true);
                gammaSettings.SetActive(false);
                thetaSettings.SetActive(false);
                break;
            case "theta":
                thetaSettings.SetActive(true);
                gammaSettings.SetActive(false);
                deltaSettings.SetActive(false);
                break;
        }
    }
}
