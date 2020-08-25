using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Instance : MonoBehaviour
{
    /*Red = 0,
Yellow = 54,
Green = 108,
Cyan = 162,
Blue = 216,
Purple = 270,
Rosein = 305*/

    /* 显示层次     对应Z轴
     * 1 背景       5  
     * 2 背景特效   4
     * 3 敌人单位   3
     * 4 玩家       2
     * 5 单位特效   1
     * 6 玩家子弹   0.1
     * 7 敌方碰撞盒 0.1
     * 8 敌方子弹   0
     * 9 玩家碰撞盒 0
     * 10 子弹特效  -1
     * 11 玩家判定点-2
     * 12 全屏特效  -3
     * 13 UI        -4
     */

    private static int[] color_DH = { 0, 54, 108, 162, 216, 270, 305 };
    private static List<List<Material>> materials = new List<List<Material>>();
    private static Mesh mesh;
    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        Screen.SetResolution(1920, 1080, false);

        Camera camera = Camera.main;
        float orthographicSize = camera.orthographicSize;
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        float cameraHeight = orthographicSize * 2;
        float cameraWidth = cameraHeight * aspectRatio;
        Debug.Log("camera size in unit=" + cameraWidth + "," + cameraHeight);


        Debug.Log("start");
        LoadResources();
        BulletPool.Instance.Init(materials, mesh);
        EmitterPool.Instance.Init();
    }

    static void LoadResources()
    {
        materials.Clear();
        for (int i = 0; i <= 27; i++)
        {
            materials.Add(new List<Material>());
            Material mt = Resources.Load<Material>("Bullets/Materials/bullet" + i.ToString());
            for(int j = 0; j < color_DH.Length; j++)
            {
                Material mt_color = new Material(mt);
                mt_color.SetInt("_DH", color_DH[j]);
                materials[i].Add(mt_color);

            }
            
            
        }
        mesh = GameObject.Find("GameSetting").GetComponent<MeshFilter>().mesh;
        
    }
}