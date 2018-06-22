using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildManager : MonoBehaviour {
    public GameObject[] Towers;
    public GameObject[] TowerIconButtons;
    int nowTarget = 0;
    public GameObject explainPanel;
    GameObject willBuildingTower;
    GameObject gameManager;
    public bool nowBuilding { get; private set; }
    // Use this for initialization
    void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        nowBuilding = false;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Time.timeScale == 0)
            return;

        if (!nowBuilding)
            return;
        if (Input.GetMouseButtonDown(1))
        {
            CancelBuild();//取消建造
            return;
        }

        Vector3 mouseInWhere = MousePositionInWorld.MousePosition();
        willBuildingTower.transform.position = mouseInWhere;//取得游標座標
        //判斷游標目前座標是否有跟Collider2D重疊
        Collider2D[] allCollider = Physics2D.OverlapPointAll(mouseInWhere);
        willBuildingTower.GetComponent<DrawTowerRange>().DrawRange();//畫出射程範圍
        
        if (allCollider.Length == 0)//為0 代表目前位置沒有任何塔或可建造區域 因此不做後續判斷
            return;
        bool canBuild = false;
        foreach (var r in allCollider)
        {//取出重疊的Collider2D
            if (r != null)
            {
                if (r.tag == "BuildArea" && willBuildingTower.transform.Find("BuildBase").GetComponent<TowerBuilding>().overlapCount == 0)
                {//游標的的位置位於BuildArea中，並且重疊到的塔的數量為0
                    canBuild = true;
                    break;
                }
            }
        }
        if (canBuild)//依據可否建造改變顏色
            willBuildingTower.transform.Find("BuildBase").GetComponent<TowerBuilding>().CanBuildToSetColor();
        else
            willBuildingTower.transform.Find("BuildBase").GetComponent<TowerBuilding>().CanNotBuildToSetColor();


        if (Input.GetMouseButtonDown(0))
        {
            if (canBuild)
                BuildTower();
            else
                Debug.Log("can't build");
        }
    }

    public void SelectTower(int towerIndex)
    {if (Time.timeScale == 0)
            return;
      
        //避免點了塔顯示資訊後，又沒取消選取導致塔資訊的視窗阻礙視線
        gameManager.GetComponent<TowerManager>().CancelSelect();

        if (!nowBuilding)
        {
            if (gameManager.GetComponent<GameManager>().money >= Towers[towerIndex].GetComponent<Tower>().costMoney)
            {
                willBuildingTower = Instantiate(Towers[towerIndex], Input.mousePosition, Towers[towerIndex].transform.rotation);
                nowBuilding = true;
            }
        }
        else
        {
            CancelBuild();
        }
    }


    public void SetTarget(int index)
    {
        nowTarget = index;
        SetExplain();
    }

    void CancelBuild()
    {
        nowBuilding = false;
        Destroy(willBuildingTower);
        willBuildingTower = null;
    }

    public void BuildTower()
    {
        //扣除掉建塔需要消耗的錢
        gameManager.GetComponent<GameManager>().CostMoney(-willBuildingTower.GetComponent<Tower>().costMoney);
        willBuildingTower.transform.position = MousePositionInWorld.MousePosition();
      //  willBuildingTower.SetActive(false);
      //  GameObject newTower = Instantiate(willBuildingTower, willBuildingTower.transform.position, willBuildingTower.transform.rotation);
        gameManager.GetComponent<TowerManager>().BuildTowerInMap(willBuildingTower);

    //    Destroy(willBuildingTower);
        willBuildingTower = null;
        nowBuilding = false;
    }

    void SetExplain()
    {
        explainPanel.transform.Find("Name").GetComponent<Text>().text = Towers[nowTarget].GetComponent<Tower>().tower_Name;
        explainPanel.transform.Find("Cost").Find("Text").GetComponent<Text>().text = "x" + Towers[nowTarget].GetComponent<Tower>().costMoney;
    }
}
