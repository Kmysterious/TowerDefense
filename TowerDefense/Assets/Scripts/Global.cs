using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global {

    [Range(0,1f)]
    public static float volume = 0.8f;
    public static int maxLife = 50;
    public static GameMode gameMode = GameMode.Normal;
}
public enum GameMode
{
    Easy,
    Normal,
    Hard
}