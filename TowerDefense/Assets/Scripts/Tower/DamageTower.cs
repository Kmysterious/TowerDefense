using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTower : Tower {
    public int[] levelup_Damage = new int[4];
    GameObject targetEnemy;
    GameObject gun;
    public GameObject bullet;
    AudioSource bulletAudio;
    List<GameObject> bulletList = new List<GameObject>();
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        fireCount = fireTimer;//讓塔能在選好目標後直接攻擊一次，而不是還要等時間
        gun = mainBase.transform.Find("Gun").gameObject;
        bullet.GetComponent<Bullet>().damage = damage;
      //  bulletList = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            GameObject objBullet = Instantiate(bullet);
            objBullet.transform.SetParent(transform);
            objBullet.SetActive(false);
            bulletList.Add(objBullet);
        }
        bulletAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.timeScale == 0)//若在遊戲暫停狀態
            return;

        fireCount += Time.deltaTime;
        if (targetEnemy == null)
            return;

        Quaternion diffRotaion = Quaternion.LookRotation(mainBase.transform.position - targetEnemy.transform.position, Vector3.forward);
        mainBase.transform.rotation = Quaternion.RotateTowards(mainBase.transform.rotation, diffRotaion, Time.fixedDeltaTime * 2000);
        mainBase.transform.eulerAngles = new Vector3(0, 0, mainBase.transform.eulerAngles.z);
      

        if (fireCount >= real_FireSpeed)
        {
            Fire();
            fireCount = 0;
        }
    }

    void Fire()
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (!bulletList[i].activeInHierarchy)
            {//bulletList[i]在場景中的active為false時，才執行
                bulletList[i].transform.position = gun.transform.position;
                bulletList[i].transform.rotation = mainBase.transform.rotation;
                bulletList[i].SetActive(true);
                Vector2 dir = targetEnemy.transform.position - gun.transform.position;
                bulletList[i].GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed);
                break;//避免按1次就射好幾發，因此建立1個bullet後，就中斷迴圈
            }
        }
        bulletAudio.Play();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {//使用Stay的原因是因為用Enter的話，
     //如果塔如果把敵人殺掉，而又沒有新敵人進入範圍，塔不會對還留在範圍內的敵人攻擊
        if (targetEnemy == null)
        {
            if (collision.tag == "Enemy")
            {
                targetEnemy = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (targetEnemy != null)
        {
            if (collision.gameObject == targetEnemy)
                targetEnemy = null;
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
      
        int newDamage = (int)(damage * surpport_Damage);
        foreach (GameObject bulletss in bulletList)
            bulletss.GetComponent<Bullet>().damage = newDamage;
        base.RenewTowerData();
    }

    public override void LevelUp()
    {
        damage += levelup_Damage[level - 1];

        base.LevelUp();
    }

   /* public override string information()
    {
        string a ="攻擊："+ (int)(damage * surpport_Damage);
        return a+base.information();
    }*/
}
