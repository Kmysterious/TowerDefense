using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
public class DrawTowerRange : MonoBehaviour
{
    LineRenderer LineDrawer;
    float ThetaScale = 0.01f;//改變形狀
    float radius;
    private int Size;
    private float Theta = 0f;
    protected CircleCollider2D rangeTrigger;
    // Use this for initialization
    void Awake()
    {
        rangeTrigger = transform.GetComponent<CircleCollider2D>();
        LineDrawer = GetComponent<LineRenderer>();
        //使用的Material必須在Project Settings -> Graphics的"Always Included Shaders"中
        //如果沒有，則自行新增
        LineDrawer.material = new Material(Shader.Find("Particles/Additive"));
        LineDrawer.startColor = Color.red;
        LineDrawer.endColor = Color.red;
        LineDrawer.startWidth = 0.1f;
        LineDrawer.endWidth = 0.1f;
        SetRange(transform.GetComponent<Tower>().range);
    }

    public void SetRange(float range)
    {
        radius = range;
        rangeTrigger.radius = range;
    }

    public void DrawRange()
    {//利用畫一個邊非常多的多邊形的方式來畫出圓形範圍
        LineDrawer.enabled = true;
        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        LineDrawer.positionCount = Size;
        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = radius * Mathf.Cos(Theta);
            float y = radius * Mathf.Sin(Theta);
            LineDrawer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    public void StopDrawCircle()
    {
        LineDrawer.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {

    }

}