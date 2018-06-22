using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    public int money = 100;
    public int life = 50;
    public GameObject moneyLabel;
    public GameObject lifeLabel;
    public GameObject exitMenu;
    public GameObject levelLabel;
    GameObject enemyManager;
    public int level = 0;
    public int maxLevel = 50;
    bool gameOver;
    bool gameEnd;
    public GameObject gameOverText;
    public GameObject gameEndText;
    public int stageNumber = 1;
	// Use this for initialization
	void Start () {
        life = Global.maxLife;
        moneyLabel.transform.Find("Text").GetComponent<Text>().text = "x" + money;
        lifeLabel.transform.Find("Text").GetComponent<Text>().text = "x" + life;
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager");
        gameOver = false;
        gameEnd = false;
        gameOverText.SetActive(false);
        Time.timeScale = 1;//回到標題會設為0，如果少了這行，會導致回到標題後再進關卡，讓遊戲處於暫停狀態
    }
	
    public void CostMoney(int cost)
    {
        money += cost;
        moneyLabel.transform.Find("Text").GetComponent<Text>().text = "x" + money;
    }

    public void Damage()
    {
        life--;
        if (life <= 0)
        {
            life = 0;
            GameOver();
        }
          

        lifeLabel.transform.Find("Text").GetComponent<Text>().text = "x" + life;
    }

    public void StartLevel()
    {
        level++;
        levelLabel.GetComponent<Text>().text = "Level:" + level;
        enemyManager.GetComponent<EnemyManager>().StartLevel(level);
    }


	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
            {
                transform.GetComponent<SpeedManager>().EscToPauseOrStartGame();
                exitMenu.SetActive(true);
            }
            else
            {
                Resume();
            }
        }
        if (gameOver || gameEnd)
            if (Input.GetMouseButtonDown(0))
            {
              MusicManager.instance.PlayTitleMusic();
                SceneManager.LoadScene("Title");
            }
                
    }

    public void SetVolume()
    {
        MusicManager.instance.SetVolume();
    }

    public void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void Resume() //繼續遊戲
    {
        exitMenu.transform.parent.Find("GameSetting").gameObject.SetActive(false);
        exitMenu.SetActive(false);
        transform.GetComponent<SpeedManager>().PauseOrStartGame();
    }

    void GameOver()
    {
        MusicManager.instance.GameOverMusic();
        Time.timeScale = 0;
        gameOver = true;
        gameOverText.SetActive(true);
    }

    public void GameEnd()
    {
        Time.timeScale = 0;
        if (life <= 0)
        {//避免最後一隻敵人死亡的同時，玩家也死了，卻還判斷成贏
            GameOver();
        }
        else
        {
            gameEnd = true;
            gameEndText.SetActive(true);
            SaveData.SaveClearData(stageNumber);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
