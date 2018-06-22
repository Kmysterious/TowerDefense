using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour {
    public float baseWidth = 1920;
    public float baseHeight = 1080;
    public float baseOrthographicSize = 5;



    /*
     Projection 投射 – 設定攝影機的投射方式
    Perspective 透視 – 多使用於3D畫面中，物件會因攝影機照射的角度與距離而在畫面上的位置即大小有所偏差
    Orthographic 正投影 – 沒有任何的透視角度，多使用於2D畫面
         */
         //掛在Camera上
    private void Awake()
    {//自動將畫面縮放到適合的比例
        float newOrthographicSize = (float)Screen.height / (float)Screen.width * baseWidth / baseHeight * baseOrthographicSize;
        Camera.main.orthographicSize = Mathf.Max(newOrthographicSize, baseOrthographicSize);
     //   Debug.Log((Screen.height) + "  " + (baseWidth / baseHeight)  + "  "+ Camera.main.orthographicSize);
    }
}
