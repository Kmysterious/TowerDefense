using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTower : Tower {
    public int[] levelup_Damage = new int[4];
    GameObject gameManager;
    GameObject showMoney;
    float showTimer = .5f;
    float showCount;
    int real_Damage;
    GameObject enemyManager;
    protected override void Start()
    {
        base.Start();
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        showMoney = transform.Find("GiveMoney").gameObject;
        real_Damage = damage;
    }
    // Update is called once per frame
    void Update () {
        if (Time.timeScale == 0)
            return;

        fireCount += Time.deltaTime;
        if (fireCount >= real_FireSpeed && enemyManager.GetComponent<EnemyManager>().nowSpawn)
        {//如果不加enemyManager.GetComponent<EnemyManager>().nowSpawn的話，
         //打完敵人後，按下下一關之前仍會不斷增加錢
            Moneydoko();
            fireCount = 0;
        }
        if(showMoney.activeInHierarchy)
        {
            showCount += Time.deltaTime;
            showMoney.transform.position = new Vector3(showMoney.transform.position.x,showMoney.transform.position.y +0.01f);
            if(showCount >= showTimer)
            {
                showCount = 0;
                showMoney.SetActive(false);
            }
        }
    }
    public void TowerSurpportToAddDamage(float addDamage)
    {
        if (surpport_Damage > addDamage)//代表已經有效果更強的在運作
            return;

        surpport_Damage = addDamage;
        RenewTowerData();
    }

    protected override void RenewTowerData()
    {
        real_Damage = (int)(damage * surpport_Damage);

        base.RenewTowerData();
    }
    void Moneydoko()
    {//增加錢，並播放特效
        gameManager.GetComponent<GameManager>().CostMoney(real_Damage);
        showMoney.transform.position = transform.position;
        showMoney.SetActive(true);
    }

    public override void LevelUp()
    {
        damage += levelup_Damage[level - 1];
        base.LevelUp();
    }
}
