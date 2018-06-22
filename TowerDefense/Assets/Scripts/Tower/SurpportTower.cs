using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpportTower : Tower {
    List<GameObject> inRangeTower = new List<GameObject>();
    bool nowSurpport = true;
    public int surpportType = 0;
    public float addDamage;
    public float addRange;
    public float addFireSpeed;
    public float[] levelup_SurpportToAddDamage     = new float[4];
    public float[] levelup_SurpportToAddRange      = new float[4];
    public float[] levelup_SurpportToAddFireSpeed = new float[4];
    Collider2D[] firstResult;
    Collider2D[] trueResult;
    // Use this for initialization
    protected override	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
 
	}

    public override void LevelUp()
    {
        addDamage += levelup_SurpportToAddDamage[level-1];
        addRange += levelup_SurpportToAddRange[level - 1];
        addFireSpeed += levelup_SurpportToAddFireSpeed[level - 1];

        base.LevelUp();
        Surpport();
    }



    public void Surpport()
    {
        nowSurpport = true;
        inRangeTower.Clear();
        //先用Physics2D.OverlapCircleAll取出CircleCollider跟多少Collider重疊
        //接著將取得的數字作為OverlapCollider需要的陣列trueResult的長度
        firstResult = Physics2D.OverlapCircleAll(transform.position, transform.GetComponent<CircleCollider2D>().radius/*, 1 << LayerMask.NameToLayer("TowerBase")*/);
        trueResult = new Collider2D[firstResult.Length];
        transform.GetComponent<CircleCollider2D>().OverlapCollider(new ContactFilter2D(), trueResult);

        foreach (var r in trueResult)
        {
            if (r != null)
            {
                if (r is BoxCollider2D && r.transform.parent != null)
                {//將trueResult取得的Collider中屬於Box且父物件不為null又有"Tower"Script中的加進List中
                    if (r.transform.parent.GetComponent<Tower>() != null)
                        inRangeTower.Add(r.transform.parent.gameObject);
                }
            }
        }

        // Debug.Log("重疊=" + a + "  " + result.Length);

        if (surpportType == 0)
        {//0為攻擊強化 1為攻速範圍強化
            foreach (GameObject tower in inRangeTower)
            {//加強在範圍內的塔的能力
                if (tower.GetComponent<Tower>() is DamageTower)
                {
                    tower.GetComponent<DamageTower>().TowerSurpportToAddDamage(addDamage);
                }
                else if (tower.GetComponent<Tower>() is MoneyTower)
                {
                    tower.GetComponent<MoneyTower>().TowerSurpportToAddDamage(addDamage);
                }
            }
        }
        else
        {
            foreach (GameObject tower in inRangeTower)
            {
                tower.GetComponent<Tower>().TowerSurpportToAddRange(addRange);
                tower.GetComponent<Tower>().TowerSurpportToFireSpeed(addFireSpeed);
            }
        }

    }
}
