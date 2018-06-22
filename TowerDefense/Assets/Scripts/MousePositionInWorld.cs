using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MousePositionInWorld  {

	public static Vector2 MousePosition()
    {
        Vector3 location = Input.mousePosition;
        location.z = 10;//與camera的距離，若無設定，轉換的滑鼠座標會有問題
        Vector3 gg = Camera.main.ScreenToWorldPoint(location);
        return gg;
    }
}
