using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilding : MonoBehaviour {
    //建立塔使用
    //剛體要使用Kinematic 才能避免塔被擠走
    public SpriteRenderer[] towerImage;
    Collider2D[] result;//取得重疊的collider
    public int overlapCount { get; private set; }//重疊的個數
    bool buildFinish;//判斷是否建好
    public Color normalColor = Color.white;
	void Start () {
        buildFinish = false;
   
        overlapCount = 0;

    }
    // Update is called once per frame
    void Update()
    {
        if (buildFinish)
            return;

        result = Physics2D.OverlapBoxAll(transform.position, transform.GetComponent<BoxCollider2D>().size, LayerMask.NameToLayer("TowerBase"));

        overlapCount = transform.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(), result);
     //   Debug.Log("重疊=" + overlapCount);

     /*   foreach (var r in result)
        {
            if (r != null)
                Debug.Log(r.name);
        }*/
    }
   
    public void CanNotBuildToSetColor()
    {//不能建造的話，顯示紅色
        foreach (SpriteRenderer image in towerImage)
            image.color = Color.red;
    }
    public void CanBuildToSetColor()
    {//可以建造，顯示原來的顏色
        foreach (SpriteRenderer image in towerImage)
            image.color = normalColor;
    }

   /* private void OnTriggerExit2D(Collider2D collision)
    {
        overlapCount = 0;
    }*/
    public int ReturnOverlapCount()
    {
        return overlapCount;
    }
    //建好塔後把TowerBuilding的enabled及物件上的box collider的is trigger為false
    public void TowerBuildFinish()
    {
        buildFinish = true;
        transform.GetComponent<Collider2D>().isTrigger = false;
     //   transform.GetComponent<Rigidbody2D>().Sleep();
        transform.GetComponent<TowerBuilding>().enabled = false;
    }
}
