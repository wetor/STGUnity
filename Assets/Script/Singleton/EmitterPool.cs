using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterPool : Singleton<EmitterPool>
{
    public static int EMITTER_MAX = 30;
    public Emitter[] emitter = new Emitter[EMITTER_MAX];
    private GameObject player;

    private BulletPool _bulletPool;
    public bool flag = false;

    private const float PI2 = Mathf.PI * 2;
    private const float PI = Mathf.PI;

    delegate void DanmakuDelegate(Emitter emitter);
    DanmakuDelegate[] Danmakus;

    public void Init()
    {
        _bulletPool = BulletPool.Instance;
        player = GameObject.Find("Player");
        Danmakus = new DanmakuDelegate[7] {
            new DanmakuDelegate(Danmaku01) ,
            new DanmakuDelegate(Danmaku01) ,
            new DanmakuDelegate(Danmaku02) ,
            new DanmakuDelegate(Danmaku03) ,
            new DanmakuDelegate(Danmaku04) ,
            new DanmakuDelegate(Danmaku05) ,
            new DanmakuDelegate(Danmaku06)};

        for (int i = 0; i < EMITTER_MAX; i++)
            emitter[i] = new Emitter();

        flag = true;
    }

    private int SearchEmitter()
    {
        for (int i = 0; i < EMITTER_MAX; i++)
        {
            if (!emitter[i].flag)
                return i;
        }
        return -1;
    }

    public int AddEmitter(GameObject enemy, int danmaku)
    {
        int id = SearchEmitter();
        if (id >= 0)
        {
            emitter[id].danmakuIndex = danmaku;
            emitter[id].emitterId = id;
            emitter[id].Init(enemy);

        }

        return id;
    }

    public void UpdateEmitter()
    {
        for (int i = 0; i < EMITTER_MAX; i++)
        {
            if (emitter[i].flag)
            {
                emitter[i].Update();
                Danmakus[emitter[i].danmakuIndex](emitter[i]);
                emitter[i].frame++;
            }
        }
    }

    private float emitterAtan2(float x, float y)
    {
        return Mathf.Atan2(player.transform.position.y - y, player.transform.position.x - x) - PI / 2;
    }
    private float rang(float max)
    {
        return Random.Range(0, max);
    }
    private float rang(float min, float max)
    {
        return Random.Range(min, max);
    }
    private int rang(int max)
    {
        return Random.Range(0, max);
    }

    public void Danmaku01(Emitter emitter)
    {
        const int TM000 = 120;
        int i, t = emitter.frame % TM000;

        int bullet_id = 0;
        float angle;

        if (t < 60 && t % 10 == 0)
        {
            angle = emitterAtan2(emitter.x, emitter.y);
            for (i = 0; i < 30; i++)
            {
                bullet_id = _bulletPool.AddBullet(false, 1, 0,
                    emitter.x, emitter.y,
                    angle + PI2 / 30 * i,
                    3 / 100f, emitter.emitterId);
                emitter.bulletFlag[bullet_id] = true;
            }
        }
    }
    public void Danmaku02(Emitter emitter)
    {
        const int TM000 = 120;
        int i, t = emitter.frame % TM000;

        int bullet_id = 0;
        float angle;

        if (t < 60 && t % 10 == 0)
        {
            angle = emitterAtan2(emitter.x, emitter.y);
            for (i = 0; i < 30; i++)
            {
                bullet_id = _bulletPool.AddBullet(false, 1, 0,
                    emitter.x, emitter.y,
                    angle + PI2 / 30 * i,
                    3 / 100f, emitter.emitterId);
                emitter.bulletFlag[bullet_id] = true;
            }
        }
        if (t == 80)
        {
            for (i = 0; i < BulletPool.BULLET_MAX; i++)
            {
                if (emitter.bulletFlag[i])
                {
                    _bulletPool.bullet[i].angle += PI / 2;
                }
            }
        }
    }
    //沉默的圣奈
    static int cnum03 = 0;
    public void Danmaku03(Emitter emitter)
    {
        const int TM001 = 60;
        int i, k, t = emitter.frame % TM001, t2 = emitter.frame;

        int bullet_id = 0;
        float angle;

        if (t2 == 0)//最开始的初始化
            cnum03 = 0;
        if (t == 0)
        {//每1次弹幕最开始的初始化
            emitter.base_angle[0] = emitterAtan2(emitter.x, emitter.y);//自机与Boss的角度
            if (cnum03 % 4 == 3)
            {// 4次弹幕移动一次
                // -4 <x< 4 y:2 <y< 4.7 区域移动
                emitter.enemyScript.move_boss_pos(
                    -GameSetting.Instance.WidthF + 0.4f,
                    GameSetting.Instance.HeightF - 3f,
                    GameSetting.Instance.WidthF - 0.4f,
                    GameSetting.Instance.HeightF - 0.3f,
                    100 / 100f, 60);
                //boss->move_boss_pos(FX + 40, FY + 30, FMX - 40, FX + 120, 60, 60);
            }
        }
        //1次弹幕的最开始是自机狙，到了一半之后从自机狙错开
        if (t == TM001 / 2 - 1)
            emitter.base_angle[0] += PI2 / 20 / 2;
        //1次弹幕发射10次圆形子弹
        if (t % (TM001 / 10) == 0)
        {
            angle = emitterAtan2(emitter.x, emitter.y);//自机-Boss之间的角度
            for (i = 0; i < 20; i++)
            {//20个
                //从基本角度开始旋转20次并发射
                bullet_id = _bulletPool.AddBullet(false, 9, 0,
                    emitter.x, emitter.y,
                    emitter.base_angle[0] + PI2 / 20 * i,
                    2.7f / 100f, emitter.emitterId);
                emitter.bulletFlag[bullet_id] = true;

            }
        }
        //4次计数掉落一次的子弹的登录
        if (t % 4 == 0)
        {

            bullet_id = _bulletPool.AddBullet(false, 9, 3,
                Random.Range(-GameSetting.Instance.WidthF, GameSetting.Instance.WidthF),
                Random.Range(GameSetting.Instance.HeightF, GameSetting.Instance.HeightF - 2f),
                PI,
                (1 + rang(0.5f)) / 100f, emitter.emitterId);
            emitter.bulletFlag[bullet_id] = true;
        }
        if (t == TM001 - 1)
            cnum03++;
    }
    //完美冻结
    public void Danmaku04(Emitter emitter)
    {
        const int TM002 = 650;
        int i, k, t = emitter.frame % TM002;

        int bullet_id = 0;
        float angle;
        if (t == 0 || t == 210)
        {
            // -4 <x< 4 y:2 <y< 4.7 区域移动
            emitter.enemyScript.move_boss_pos(
                -GameSetting.Instance.WidthF + 0.4f,
                GameSetting.Instance.HeightF - 3f,
                GameSetting.Instance.WidthF - 0.4f,
                GameSetting.Instance.HeightF - 0.3f,
                100 / 100f, 60);
        }
        //最开始的随机发射
        if (t < 180)
        {
            for (i = 0; i < 2; i++)
            {//1次计数发射2次
                bullet_id = _bulletPool.AddBullet(false, 21, rang(7),
                    emitter.x, emitter.y,
                    rang(PI2 / 15) + PI2 / 10 * t,   //rang(PI2/15) 15，越大越聚合
                    (3.2f + rang(2.1f)) / 100f, emitter.emitterId);
                emitter.bulletFlag[bullet_id] = true;
                _bulletPool.bullet[bullet_id].autoRotate = true;
                _bulletPool.bullet[bullet_id].danmakuFlag = 0;

            }
        }
        //根据自机的位置往8个方向发射
        if (210 < t && t < 270 && t % 3 == 0)
        {
            angle = emitterAtan2(emitter.x, emitter.y);
            for (i = 0; i < 8; i++)
            {
                bullet_id = _bulletPool.AddBullet(false, 5, 4,
                    emitter.x, emitter.y,
                    angle - PI / 2 * 0.8f + PI * 0.8f / 7f * i + rang(-PI / 25f, PI / 25f), //rang(-PI/25f, PI/25f) 25，越大越聚合
                    (3.0f + rang(0.3f)) / 100f, emitter.emitterId);
                emitter.bulletFlag[bullet_id] = true;
                _bulletPool.bullet[bullet_id].danmakuFlag = 2;

            }

        }

        // 静止子弹
        for (i = 0; i < BulletPool.BULLET_MAX; i++)
        {
            if (emitter.bulletFlag[i])
            {
                //t在190的时候将所有的子弹都停止下来，然后变白色，重置计数器
                if (_bulletPool.bullet[i].danmakuFlag == 0)
                {
                    if (t == 190)
                    {
                        _bulletPool.bullet[i].autoRotate = false;//停止子弹的旋转
                        _bulletPool.bullet[i].state = BulletState.Static;
                        _bulletPool.bullet[i].speed = 0;
                        _bulletPool.bullet[i].color = 3;
                        _bulletPool.bullet[i].frame = 0;
                        _bulletPool.bullet[i].danmakuFlag = 1;//将状态设置为1
                    }
                }
                //开始往随机方向移动
                if (_bulletPool.bullet[i].danmakuFlag == 1)
                {
                    if (_bulletPool.bullet[i].frame == 200)
                    {
                        _bulletPool.bullet[i].angle = rang(PI2);//全方向随机
                        _bulletPool.bullet[i].autoRotate = true;//设置旋转flag为有效
                    }
                    if (_bulletPool.bullet[i].frame > 200)
                    {
                        _bulletPool.bullet[i].state = BulletState.Default;
                        _bulletPool.bullet[i].speed += 0.01f / 100;//逐渐加速
                    }

                }
            }
        }
    }

    //恋之迷路
    static int tcnt05, cnt05, cnum05;
    public void Danmaku05(Emitter emitter)
    {
        const int TM003 = 600;
        const int DF003 = 20;
        int i, j, k, t = emitter.frame % TM003, t2 = emitter.frame;
        

        int bullet_id = 0;
        float angle;
        if (t2 == 0)
        {
            emitter.enemyScript.input_phy_pos(0, 0, 50);

            cnum05 = 0;
        }
        if (t == 0)
        {
            emitter.base_angle[0] = emitterAtan2(emitter.x, emitter.y);
            cnt05 = 0;
            tcnt05 = 2;
        }
        if (t < 540 && t % 3 !=0)
        {
            angle = emitterAtan2(emitter.x, emitter.y);
            if (tcnt05 - 2 == cnt05 || tcnt05 - 1 == cnt05)
            {
                if (tcnt05 - 1 == cnt05)
                {
                    emitter.base_angle[1] = emitter.base_angle[0] + PI2 / DF003 * cnt05 * (cnum05%2!=0 ? -1 : 1) - PI2 / (DF003 * 6) * 3;
                    tcnt05 += DF003 - 2;
                }
            }
            else
            {
                for (i = 0; i < 6; i++)
                {

                    bullet_id = _bulletPool.AddBullet(false, 9, cnum05%2 != 0 ? 1 : 4,
                        emitter.x, emitter.y,
                        emitter.base_angle[0] + PI2 / DF003 * cnt05 * (cnum05 % 2 != 0 ? -1 : 1) + PI2 / (DF003 * 6) * i * (cnum05 % 2 != 0 ? -1 : 1),
                        2 / 100f, emitter.emitterId);
                    emitter.bulletFlag[bullet_id] = true;
                }
            }
            cnt05++;
        }
        if (40 < t && t < 540 && t % 30 == 0)
        {
            for (j = 0; j < 3; j++)
            {
                angle = emitter.base_angle[1] - PI2 / 36 * 4;
                for (i = 0; i < 27; i++)
                {
                    bullet_id = _bulletPool.AddBullet(false, 5, cnum05 % 2 != 0 ? 6 : 0,
                        emitter.x, emitter.y,
                        angle,
                        (4 - 1.6f / 3 * j) / 100f, emitter.emitterId);
                    emitter.bulletFlag[bullet_id] = true;

                    angle -= PI2 / 36;
                }
            }
        }
        if (t == TM003 - 1)
            cnum05++;

    }
    //小小青蛙不畏风雨
    static int tm06;
    public void Danmaku06(Emitter emitter)
    {
        const int TM004 = 200;
        int i, j, k, n, t = emitter.frame % TM004, t2 = emitter.frame;
        float angle;
        int bullet_id = 0;
        
        if (t2 == 0)
            emitter.enemyScript.input_phy_pos(0, 3.8f, 50);
        //周期的最开始设置tm
        if (t == 0)
            tm06 = 190 + rang(30);
        angle = PI * 1.5f + PI / 4 * Mathf.Sin(PI2 / tm06 * t2);
        //每4次计数往12路射出子弹
        if (t2 % 4 == 0)
        {
            for (n = 0; n < 12; n++)
            {
                bullet_id = _bulletPool.AddBullet(false, 23, n==0 || n==11 ? 3:0,
                       emitter.x, emitter.y,
                       0,
                       0, emitter.emitterId);
                emitter.bulletFlag[bullet_id] = true;
                _bulletPool.bullet[bullet_id].state = BulletState.MotionDisp;
                _bulletPool.bullet[bullet_id].vPos.x = (Mathf.Cos(angle - PI / 8 * 4 + PI / 12 * n + PI / 16) * 3) / 50f;
                _bulletPool.bullet[bullet_id].vPos.y = -(Mathf.Sin(angle - PI / 8 * 4 + PI / 12 * n + PI / 16) * 3) / 80f;
                _bulletPool.bullet[bullet_id].danmakuFlag = 0;



            }
        }
        if (t % 1 == 0 && t2 > 80)
        {
            int num = 1;
            if (t % 2!=0)
                num = 2;
            for (n = 0; n < num; n++)
            {
                angle = PI * 1.5f - PI / 2 + PI / 12 * (t2 % 13) + rang(PI / 15);
                bullet_id = _bulletPool.AddBullet(false, 9, 4,
                    emitter.x, emitter.y,
                    0,
                    0, emitter.emitterId);
                emitter.bulletFlag[bullet_id] = true;
                _bulletPool.bullet[bullet_id].state = BulletState.MotionDisp;
                _bulletPool.bullet[bullet_id].vPos.x = (Mathf.Cos(angle) * 1.4f * 1.2f) /70f;
                _bulletPool.bullet[bullet_id].vPos.y = -(Mathf.Sin(angle) * 1.4f) /100f;
                _bulletPool.bullet[bullet_id].danmakuFlag = 1;

            }
        }
        for (i = 0; i < BulletPool.BULLET_MAX; i++)
        {
            if (emitter.bulletFlag[i])
            {
                if (_bulletPool.bullet[i].danmakuFlag == 0)
                {
                    if (_bulletPool.bullet[i].frame < 60)
                        _bulletPool.bullet[i].vPos.y -= 0.03f / 100f;
                    else if (_bulletPool.bullet[i].frame < 150)
                        _bulletPool.bullet[i].vPos.y -= 0.04f / 100f;
                    //bullet[i].x += bullet[i].vx;
                    //bullet[i].y += bullet[i].vy;
                }
                if (_bulletPool.bullet[i].danmakuFlag == 1)
                {
                    if (_bulletPool.bullet[i].frame < 160)
                        _bulletPool.bullet[i].vPos.y -= 0.03f / 100f;
                    //bullet[i].x += bullet[i].vx;
                    //bullet[i].y += bullet[i].vy;
                    //bullet[i].angle = atan2(bullet[i].vy, bullet[i].vx);
                }
            }
        }
    }
}