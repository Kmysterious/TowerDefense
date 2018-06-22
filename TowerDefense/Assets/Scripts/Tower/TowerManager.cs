using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TowerManager : MonoBehaviour {

    List<GameObject> allTowers = new List<GameObject>();
    public Transform inMapTowersList;
    public GameObject towerInformation;
    GameObject targetTower;
    GameObject buildManager;
	// Use this for initialization
	void Start () {
        buildManager = GameObject.FindGameObjectWithTag("BuildManager");
      
    }

    public void BuildTowerInMap(GameObject tower)
    {
        //將塔的放在地圖上需要或不需要的功能啟動或關閉，並加入List中
        tower.GetComponent<DrawTowerRange>().StopDrawCircle();
        tower.transform.SetParent(inMapTowersList);
        tower.transform.Find("BuildBase").GetComponent<TowerBuilding>().TowerBuildFinish();
        tower.GetComponent<Tower>().enabled = true;


        allTowers.Add(tower);
        
        RenewAllTowerSurpport();
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.timeScale == 0)
            return;

        if (!buildManager.GetComponent<BuildManager>().nowBuilding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClickToSelectTower();
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelSelect();
            }
        }
    }

    public void ClickToSelectTower()
    {//點選塔，如果塔的資訊還在顯示的話就不執行

        if (towerInformation.activeInHierarchy)
            return;
        
        Collider2D towerCollider = Physics2D.OverlapPoint(MousePositionInWorld.MousePosition(), 1 << LayerMask.NameToLayer("TowerBase"));
        if (towerCollider != null)
        { 
            targetTower = towerCollider.transform.parent.gameObject;
            ShowTowerInformation();
        }
    }

    public void CancelSelect()
    {//取消選取
        if (targetTower == null)
            return;
        targetTower.GetComponent<DrawTowerRange>().StopDrawCircle();
        towerInformation.SetActive(false);
        targetTower = null;
    }

     void ShowTowerInformation()
    {//顯示點選的塔的資訊
        if (targetTower == null)
            return;
       
        towerInformation.transform.position = Camera.main.WorldToScreenPoint(MousePositionInWorld.MousePosition());

        RenewTowerInformation();
        towerInformation.SetActive(true);
    }

    void RenewTowerInformation()
    {
        if (targetTower.GetComponent<Tower>().level == 5)
            towerInformation.transform.Find("LevelUpButton").gameObject.SetActive(false);
        else
            towerInformation.transform.Find("LevelUpButton").gameObject.SetActive(true);

        towerInformation.transform.Find("Name").GetComponent<Text>().text = targetTower.GetComponent<Tower>().tower_Name;
        towerInformation.transform.Find("Level").GetComponent<Text>().text = "等級" + targetTower.GetComponent<Tower>().level;
        towerInformation.transform.Find("LevelUpButton").Find("LevelUp").Find("Cost").Find("Text").GetComponent<Text>().text = "x" + targetTower.GetComponent<Tower>().levlup_Price;
   //     towerInformation.transform.Find("Test").GetComponent<Text>().text = targetTower.GetComponent<Tower>().information();
        towerInformation.transform.Find("Sell").Find("Cost").Find("Text").GetComponent<Text>().text = "x" + targetTower.GetComponent<Tower>().sellMoney;
        targetTower.GetComponent<DrawTowerRange>().DrawRange();
    }

    public void TowerLevelUp()
    {
        if (transform.GetComponent<GameManager>().money >= targetTower.GetComponent<Tower>().levlup_Price)
        {
            transform.GetComponent<GameManager>().CostMoney(-targetTower.GetComponent<Tower>().levlup_Price);
            targetTower.GetComponent<Tower>().LevelUp();
            RenewTowerInformation();
        }
    }

    public void TowerSell()
    {
        foreach(GameObject tower in allTowers)
        {
            if(tower.Equals(targetTower))
            {
                transform.GetComponent<GameManager>().CostMoney(targetTower.GetComponent<Tower>().sellMoney);
                allTowers.Remove(tower);
                targetTower.GetComponent<DrawTowerRange>().StopDrawCircle();
                towerInformation.SetActive(false);
                Destroy(targetTower);
                RenewAllTowerSurpport();
                break;
            }
        }
    }

    void RenewAllTowerSurpport()
    {//重新判定塔所要增加的能力
        foreach (GameObject tower in allTowers)
        {
            if (tower.GetComponent<Tower>() is SurpportTower)
            {
                tower.GetComponent<SurpportTower>().Surpport();
            }
        }
      
    }
}
