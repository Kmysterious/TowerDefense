using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleMenu : MonoBehaviour {

    public GameObject SelectStage;
    public void StartGame(int life)
    {
        switch(life)
        {
            case 100:
                Global.gameMode = GameMode.Easy;
                break;
            case 50:
                Global.gameMode = GameMode.Normal;
                break;
            case 1:
                Global.gameMode = GameMode.Hard;
                break;
        }

        Global.maxLife = life;
        MusicManager.instance.StartGameMusic();
        SceneManager.LoadScene("stage0"+ SelectStage.GetComponent<SelectStage>().stageNumber);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
