using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour {

    public GameObject[] gameModeButton = new GameObject[3];
    string[] gameModeString = new string[3] {"Easy","Normal","Hard" };
    public int stageNumber { get; private set; }
    public GameObject[] stageImage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeStage(int stage)
    {
        stageNumber = stage;
        for (int i =0;i< 3; i++)
        {//判斷地圖的三種難度是否已經過關
            if(PlayerPrefs.HasKey("Stage" + stage + " " + gameModeString[i]))
            {
                if(PlayerPrefs.GetInt("Stage" + stage + " " + gameModeString[i]) == 1)
                {
                    gameModeButton[i].transform.Find("VictoryImage").gameObject.SetActive(true);
                }
            }
            else
            {
                gameModeButton[i].transform.Find("VictoryImage").gameObject.SetActive(false);
            }
        }

        for(int i =0;i<stageImage.Length;i++)
        {
            if ((i + 1) == stage)
                stageImage[i].SetActive(true);
            else
                stageImage[i].SetActive(false);
        }

    }

    private void OnEnable()
    {
        ChangeStage(1);
    }
}
