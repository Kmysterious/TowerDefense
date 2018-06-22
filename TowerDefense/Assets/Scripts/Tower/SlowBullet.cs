using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBullet : MonoBehaviour {

    float slowTimer = 0;

    public void SetSlowTimer(float slow)
    {
        slowTimer = slow;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().Slow(slowTimer);
        }
    }
}
