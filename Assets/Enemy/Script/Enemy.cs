using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//用于物理计算的结构体
public struct phy_t
{
	public int flag, cnt, set_t;
	public float ax, v0x, ay, v0y, vx, vy, prex, prey;
};
public class Enemy : MonoBehaviour
{
	const int BOSS_POS_X = 0;
	const int BOSS_POS_Y = -1080 / 4;

	private EmitterPool _emitterPool;

	public float Speed = 4.0f;
    public int Hp = 2000;
    public EnemyState State;
	public int DanmakuType = 0;


	public float x, y;

    private int frame = 0;

	private int emitterId;

	public bool flag = false;

	private phy_t phy;
	// Start is called before the first frame update
	void Start()
    {

		_emitterPool = EmitterPool.Instance;
		flag = true;
        frame = 0;
		x = transform.position.x;
		y = transform.position.y;
		emitterId = _emitterPool.AddEmitter(gameObject, DanmakuType);
		//input_phy_pos(0,350/100,60);

	}

    // Update is called once per frame
    void Update()
    {
		if (phy.flag == 1)
		{//如果物理移动计算有效
		 //calc_phy();//进行物理计算
			float t = phy.cnt;
			x = phy.prex - ((phy.v0x * t) - 0.5f * phy.ax * t * t);//计算当前应当所在的x坐标
			y = phy.prey - ((phy.v0y * t) - 0.5f * phy.ay * t * t);//计算当前应当所在的y坐标
			phy.cnt++;
			if (phy.cnt >= phy.set_t)//如果超过附加移动的时间的话
				phy.flag = 0;//オフ 设置移动为无效
		}

		
		transform.position = new Vector3(x, y, transform.position.z);
        frame++;

	}

	public void input_phy(int t)
	{//t=附加在移动上的时间
		float ymax_x, ymax_y;
		if (t == 0)
			t = 1;
		phy.flag = 1;//登录有效
		phy.cnt = 0;//计数器初始化
		phy.set_t = t;//设置附加移动时间
		ymax_x = x - BOSS_POS_X;//想要移动的水平距离
		phy.v0x = 2 * ymax_x / t;//水平分量的初速度
		phy.ax = 2 * ymax_x / (t * t);//水平分量的加速度
		phy.prex = x;//初始x坐标
		ymax_y = y - BOSS_POS_Y;//想要移动的竖直距离
		phy.v0y = 2 * ymax_y / t;//竖直分量的初速度
		phy.ay = 2 * ymax_y / (t * t);//数值分量的加速度
		phy.prey = y;//初始y坐标
	}
	//进行与某点的指定距离的物理计算的登录（在指定时间t内回到固定位置）
	public void input_phy_pos(float x1, float y1, int t)
	{
		float ymax_x, ymax_y;
		if (t == 0) t = 1;
		phy.flag = 1;//登录有效
		phy.cnt = 0;//计时器初始化
		phy.set_t = t;//设置移动附加时间
		ymax_x = x - x1;//想要移动的水平距离
		phy.v0x = 2 * ymax_x / t;//水平分量的初速度
		phy.ax = 2 * ymax_x / (t * t);//水平分量的加速度
		phy.prex = x;//初始x坐标
		ymax_y = y - y1;//想要移动的水平距离
		phy.v0y = 2 * ymax_y / t;//水平分量的初速度
		phy.ay = 2 * ymax_y / (t * t);//水平分量的加速度
		phy.prey = y;//初始y坐标
	}
	public int move_boss_pos(float x1, float y1, float x2, float y2, float dist, int t)
	{
		int i = 0;
		float t_x, t_y, angle;
		for (i = 0; i < 1000; i++)
		{
			t_x = x;
			t_y = y;//设置当前Boss的位置
			angle = Random.Range(0, Mathf.PI * 2);//適当地决定前进方向
			t_x += Mathf.Cos(angle) * dist;//向着那个地方移动
			t_y += Mathf.Sin(angle) * dist;

			
			if (x1 <= t_x && t_x <= x2 && y1 <= t_y && t_y <= y2)
			{//如果那个点在移动可能的范围内的话
				
				input_phy_pos(t_x, t_y, t);
				//Debug.LogFormat("{0} {1}  {2} {3} {4} {5}", t_x, t_y, x1, y1, x2, y2);
				return 0;
			}
		}
		return -1;//1000如果1000次尝试都不能的话就返回错误
	}
}
