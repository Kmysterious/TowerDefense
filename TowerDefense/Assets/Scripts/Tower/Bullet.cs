using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage;
    GameObject targetEnemy;
    private void OnEnable()
    {
        transform.GetComponent<Rigidbody2D>().WakeUp();
        Invoke("HideBullet", 1f);//1秒後執行HideBullet函式
    }

    void HideBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {//因為gameObject的Active在擊中敵人時被設為false
     //為了避免在重複設定，因此要取消Invoke
     //且為了減少效能負擔在Disable的同時，讓Rigidbody Sleep
        transform.GetComponent<Rigidbody2D>().Sleep();
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetEnemy != null)//避免一個子彈同時殺好幾個進入範圍的敵人
            return;
        if (collision.transform.tag == "Enemy")
        {
            targetEnemy = collision.gameObject;    
        }
    }

    private void Update()
    {
        if (targetEnemy != null)
        {//避免一個子彈同時殺好幾個進入範圍的敵人
            gameObject.SetActive(false);
            targetEnemy.GetComponent<Enemy>().Damage(damage);
            targetEnemy = null;//避免下次子彈啟動時無法攻擊敵人
        }
    }
}
