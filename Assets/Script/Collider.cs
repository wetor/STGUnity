using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Collider
{
	public static bool Judge(BulletBase bullet, float x1, float y1, float radius)
	{
		int j;
		if (bullet.frame > 0)
		{ //如果射击的子弹的轨道至少计算过1次
			float x = bullet.pos.x - x1; //敌人和自机射击的子弹的距离
			float y = bullet.pos.y - y1;
			//防止溢出

			/*if (bullet.knd >= bullet_RANGE_MAX || unit.knd >= unit_RANGE_MAX)
				printfDx("out_judge_bullet溢出");*/

			//敌人的碰撞判定和自机射击的子弹的碰撞判定的合计范围
			float r = bullet.radius + radius;
			//如果有必要计算中间的话
			if (bullet.speed > r)
			{
				float t_angle = bullet.angle + Mathf.PI / 2f;
				//保存1帧之前的位置
				//float pre_x = bullet.pos.x + Mathf.Cos(bullet.angle + Mathf.PI) * bullet.speed;
				//float pre_y = bullet.pos.y + Mathf.Sin(bullet.angle + Mathf.PI) * bullet.speed;


				float pre_x = bullet.pos.x - Mathf.Cos(t_angle) * bullet.speed;
				float pre_y = bullet.pos.y - Mathf.Sin(t_angle) * bullet.speed;

				//Debug.LogFormat("{0} {1}    {2} {3}", bullet.pos.x, bullet.pos.y,pre_x,pre_y);
				float px, py;
				for (j = 0; j < bullet.speed / r; j++)
				{//前进部分÷碰撞判定部分次循环
					px = pre_x - x1;
					py = pre_y - y1;
					if (px * px + py * py < r * r)
						return true;
					pre_x += Mathf.Cos(t_angle) * r;
					pre_y += Mathf.Sin(t_angle) * r;
				}
			}
			if (x * x + y * y < r * r)//如果在碰撞判定范围内
				return true;//碰撞
		}
		return false;
	}
}