using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : Singleton<GameSetting>
{
    public static int Width = 880;
    public static int Height = 1000;

    public float WidthF = Width / 2 / 100f;
    public float HeightF = Height / 2 / 100f;
}
