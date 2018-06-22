using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData  {

	public static void SaveClearData(int stageNumber)
    {
        PlayerPrefs.SetInt("Stage"+stageNumber + " " + Global.gameMode.ToString(),1);
    }
}
