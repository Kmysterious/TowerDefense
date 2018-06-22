using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpeedManager : MonoBehaviour {


    public GameObject[] speedUpButtons;
    public GameObject PauseButton;
    float saveSpeed = 1f;
    public void ClickToSpeedUp()
    {
        if (Time.timeScale == 0)
            return;

        Time.timeScale++;
        if (Time.timeScale > 3)
            Time.timeScale = 1;
        switch((int)Time.timeScale)
        {//切換該顯示的按鈕
            case 1:
                speedUpButtons[0].SetActive(true);
                speedUpButtons[1].SetActive(false);
                speedUpButtons[2].SetActive(false);
                break;
            case 2:
                speedUpButtons[0].SetActive(false);
                speedUpButtons[1].SetActive(true);
                speedUpButtons[2].SetActive(false);
                break;
            case 3:
                speedUpButtons[0].SetActive(false);
                speedUpButtons[1].SetActive(false);
                speedUpButtons[2].SetActive(true);
                break;
        }
    }

    public void PauseOrStartGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = saveSpeed;
            PauseButton.transform.Find("Text").GetComponent<Text>().text = "暫停";
        }
        else
        {
            saveSpeed = Time.timeScale;
            Time.timeScale = 0;
            PauseButton.transform.Find("Text").GetComponent<Text>().text = "繼續";
        }

    }
    public void EscToPauseOrStartGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = saveSpeed;
        }
        else
        {
            saveSpeed = Time.timeScale;
            Time.timeScale = 0;
        }

    }
}
