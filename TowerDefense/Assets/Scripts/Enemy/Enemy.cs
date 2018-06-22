using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    GameObject[] targets;
    GameObject enemyManager;
    public int health;
    public float speed;
    int nowTarget;
    float moveSpeed;//紀錄第一次移動的距離
    float nowSpeed;//目前移動的速度
    public int haveMoney;
    GameObject gameManager;
    float damageTimer = .2f;
    float damageTimerCount;
    bool nowDamage = false;
    bool nowDead = false;//避免子彈重複觸發導致EnemyManager的deadCount重複計算
    SpriteRenderer image;
    bool slow = false;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager");
       // targets = enemyManager.GetComponent<EnemyManager>().targers;
        nowTarget = 0;
        nowSpeed = speed;
        moveSpeed = speed * 15;
            /*Vector2.Distance(transform.position, targets[1].transform.position) * speed * 5;*/
        image = transform.GetComponent<SpriteRenderer>();
    }

    public void SetPath(GameObject[] targetPaths)
    {
        targets = targetPaths;
    }
	
	// Update is called once per frame
	void Update () {
        Movement();
        if(nowDamage)
        {//根據是否受到傷害，切換顏色
            image.color = Color.red;
            damageTimerCount += Time.deltaTime;
            if(damageTimerCount >= damageTimer)
            {
                damageTimerCount = 0;
                image.color = Color.white;
                nowDamage = false;
            }
        }
    }

    void Movement()
    {
        if (nowTarget >= targets.Length)
            return;

        if (Time.timeScale == 0)//若在遊戲暫停狀態
            return;

        float slowSpeed = 1f;
        if (slow)
        {
            slowSpeed = .5f;
        }
        else
            slowSpeed = 1f;
        


        //自動面向目標物，另外因為Enemy的Sprite都是朝下，因此使用Vector3.back而不是Vector3.forward
        Quaternion diffRotaion = Quaternion.LookRotation(transform.position - targets[nowTarget].transform.position, Vector3.back);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, diffRotaion, Time.fixedDeltaTime * 2000);
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);


        transform.position = Vector2.Lerp(transform.position, targets[nowTarget].transform.position, nowSpeed * Time.fixedDeltaTime * Time.timeScale * slowSpeed);
        nowSpeed = calculateNewSpeed();


    }

    public void Damage(int damage)
    {if (nowDead)
            return;
        nowDamage = true;
        health -= damage;
        if(health <= 0)
        {
            nowDead = true;
            health = 0;
            gameManager.GetComponent<GameManager>().CostMoney(haveMoney);
            enemyManager.GetComponent<EnemyManager>().PlayBoomSound(transform.position);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //自動移動到目標點，並在最後一個目標點死亡
        if (collision.transform == targets[nowTarget].transform)
        {
            nowTarget++;
            if (nowTarget >= targets.Length)
            {
                nowTarget = targets.Length - 1;
                gameManager.GetComponent<GameManager>().Damage();
                enemyManager.GetComponent<EnemyManager>().EnemyDeadCount();
                Destroy(gameObject);
            }
            nowSpeed = calculateNewSpeed();
        }
    }

    private float calculateNewSpeed()
    {//因為Lerp會不斷繼續與目標的距離並改變速度，因此為了等速移動必須另外處理
        float tmp = Vector2.Distance(transform.position, targets[nowTarget].transform.position);

        if (tmp == 0)
            return tmp;
        else
            return (moveSpeed / tmp);
    }

    public void Slow(float slowTimer)
    {
        slow = true;
        StartCoroutine(SlowCount(slowTimer));//執行指定函式
    }

    IEnumerator SlowCount(float slowcount)//計時器
    {//執行此段程式x秒，x為脫離迴圈的值
        for (float i =0;i<= slowcount; i+=Time.deltaTime)
        {
            //碰到 yield return 0;會先跳離介面，
            //同步執行StartCoroutine之後的程式，
            //然後再回去執行IEnumerator介面直到迴圈結束，
            //沒碰到yield return 0;，就執行迴圈之後的程式碼，並結束StartCoroutine
            yield return 0;
        }

        slow = false;
    }
}
