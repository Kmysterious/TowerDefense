using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    //建造好的塔使用
    public string tower_Name;
    public int costMoney;
    public float range = 1f;
    public float bulletSpeed = 350;
    public int damage;
    public float fireTimer = .5f;//原始的攻擊速度
    protected float fireCount;

    
    public float[] levelup_Range = new float[4];
    public float[] levelup_FireSpeed = new float[4];
    [HideInInspector]public int levlup_Price;

    [HideInInspector]public int level = 1;
    [HideInInspector]public int sellMoney;

    //周圍有提昇能力的塔使用
   protected float surpport_Damage = 1f;
   protected float surpport_Range = 1f;
   protected float surpport_FireSpeed = 0f;
    protected float real_FireSpeed;//實際用來計算攻擊速度的值

  
    protected GameObject mainBase;

    private void Awake()
    {
       
        mainBase = transform.Find("MainBase").gameObject;
    }

    // Use this for initialization
    virtual protected  void Start () {
        
       

        real_FireSpeed = fireTimer;
        levlup_Price = costMoney / 2;
        sellMoney = costMoney / 5;
    }
	
   
 

    // Update is called once per frame
    void Update () {
        
    }

    

    public void TowerSurpportToAddRange(float addRange)
    {
        
        if (surpport_Range > addRange)//代表已經有效果更強的在運作
            return;
       
        surpport_Range = addRange;
        RenewTowerData();
    }

    public void TowerSurpportToFireSpeed(float addFireSpeed)
    {
        if (surpport_FireSpeed > addFireSpeed)//代表已經有效果更強的在運作
            return;

        surpport_FireSpeed = addFireSpeed;
        RenewTowerData();
    }

   protected virtual void RenewTowerData()
    {
       
        float newRange = range * surpport_Range;
        transform.GetComponent<DrawTowerRange>().SetRange(newRange);
     //   Debug.Log(transform.name + "  "+ range + "  " + transform.GetComponent<CircleCollider2D>().radius + "   " + newRange + "  " + surpport_Range);
        float newFireSpeed = fireTimer - surpport_FireSpeed;
        real_FireSpeed = newFireSpeed;
    }

    public virtual void LevelUp()
    {
        if (level >= 5)
            return;
        range += levelup_Range[level-1];
        fireTimer -= levelup_FireSpeed[level - 1];

        RenewTowerData();
        level++;
        levlup_Price += levlup_Price / 2;
        sellMoney = (costMoney + levlup_Price) / 5;
    }

 /*   public virtual string information()
    {
        string ssss = "";
        ssss += "範圍" + transform.GetComponent<CircleCollider2D>().radius;
        ssss += "攻速" + real_FireSpeed;
        return ssss;
    }*/
}
