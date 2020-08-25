using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class BulletBase
{

	public Material material;

	public Matrix4x4 matrices;


	public int type;
	public int color;
	public float angle;
	public float speed;
	public float radius;
	public Vector3 pos = new Vector3(0, 0, 0);
	public Vector3 vPos = new Vector3(0, 0, 0);

	public BulletState state = BulletState.Default;

	public bool autoRotate = false;
	private Quaternion rotate = Quaternion.Euler(0, 0, 0);
	private Vector3 scale = new Vector3(0.5f,0.5f,0);
	public bool isPlayer = false;

	public int emitterId;
	public int bulletId;
	public bool flag = false;
	public int frame = 0;
	public int danmakuFlag = 0;

	public void Init(bool isPlayer = false)
	{
		flag = true;
		autoRotate = false;
		state = BulletState.Default;
		danmakuFlag = 0;
		frame = 0;
		this.isPlayer = isPlayer;

		if (isPlayer)
			pos.z = 0.1f;
		else
			pos.z = 0;
		rotate = Quaternion.Euler(0, 0, angle);

	}

	// Update is called once per frame
	public void Update()
	{
		if (!flag) return;
		if(state == BulletState.MotionDisp)
		{
			pos.x += vPos.x;
			pos.y += vPos.y;
			angle = Mathf.Atan2(vPos.x, -vPos.y);
		}

		float t_angle = angle + Mathf.PI / 2f;

		if(state == BulletState.Default)
		{
			pos.x += Mathf.Cos(t_angle) * speed;
			pos.y += Mathf.Sin(t_angle) * speed;
		}
		if(state != BulletState.Static)
		{
			if (autoRotate)
			{
				t_angle = Mathf.PI * frame * speed / 2;
			}

			rotate = Quaternion.Euler(0, 0, t_angle * Mathf.Rad2Deg);

			
			//matrices = Matrix4x4.Translate(pos);
			//matrices = Matrix4x4.Rotate(new Quaternion(x,y,0,0));
			matrices = Matrix4x4.TRS(pos, rotate, scale);
		}

		if (pos.x <  - GameSetting.Instance.WidthF - 1f || 
			pos.x > GameSetting.Instance.WidthF + 1f || 
			pos.y < - GameSetting.Instance.HeightF - 1f || 
			pos.y > GameSetting.Instance.HeightF + 1f)
		{//如果跑到画面外面的话
			Destroy();
		}

		frame++;
	}

	public void Destroy()
	{
		flag = false;
		if (!isPlayer && emitterId >= 0 && bulletId >= 0)   // 同步emitter中的flag
		{
			EmitterPool.Instance.emitter[emitterId].bulletFlag[bulletId] = false;
		}
	}



}
