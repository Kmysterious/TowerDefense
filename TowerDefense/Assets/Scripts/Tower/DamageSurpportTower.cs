using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSurpportTower : Tower {
    public float[] levelup_SlowTimer = new float[4];
    public float slowTimer =.8f;//緩速敵人的秒數
    GameObject attackEffect;
    GameObject targetEnemy;
    bool nowFire;
    float effectTimer = .3f;
    float effectCount;
    float changeSize = 0f;
    // Use this for initialization
    protected override	void Start () {
        base.Start();
        attackEffect = transform.Find("AttackEffect").gameObject;
        attackEffect.transform.Find("Effect").GetComponent<SlowBullet>().SetSlowTimer(slowTimer);
        nowFire = false;
        fireCount = real_FireSpeed;

    }
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale== 0)//若在遊戲暫停狀態
            return;

        if(nowFire)//已經發出的攻擊動畫，即使原先的目標跑掉了也要做完，因此放在上面
        {
            effectCount += Time.deltaTime;
            Resize();
            if (effectCount >= effectTimer)
            {
                attackEffect.SetActive(false);
                effectCount = 0;
                attackEffect.transform.localScale = new Vector3(.5f,.5f);
                nowFire = false;
            }
        }

        fireCount += Time.deltaTime;
        if (targetEnemy == null)
            return;
      
        if (fireCount >= real_FireSpeed)
        {
            Fire();
            fireCount = 0;
        }
    }

    void Fire()
    {
        nowFire = true;
        attackEffect.SetActive(true);
        changeSize = .5f;//range / effectTimer;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            targetEnemy = collision.gameObject;
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

    void Resize()
    {
        if (attackEffect.transform.localScale.x > transform.GetComponent<CircleCollider2D>().radius + changeSize)
            return;
        attackEffect.transform.localScale = new Vector3(attackEffect.transform.localScale.x + changeSize, attackEffect.transform.localScale.y + changeSize);
    }

    public override void LevelUp()
    {
        slowTimer += levelup_SlowTimer[level - 1];

        base.LevelUp();
    }
}
