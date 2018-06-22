using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildTowerButton : Button {

    public int inListIndex;
    public GameObject towerList;
	
	// Update is called once per frame
	void Update () {
        var eventData = new UnityEngine.EventSystems.BaseEventData(UnityEngine.EventSystems.EventSystem.current);
        if (IsHighlighted(eventData))
        {//偵測是否為反白狀態
            towerList.GetComponent<BuildManager>().SetTarget(inListIndex);
        }
    }
}
