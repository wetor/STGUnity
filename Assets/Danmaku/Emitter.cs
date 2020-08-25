using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter
{
    private BulletPool _bulletPool;
    public GameObject enemy;
    public Enemy enemyScript;

    public float x, y;
    public EmitterState State = EmitterState.Follow;
    public bool flag = false;
    public int danmakuIndex = 0;
    public int emitterId = 0;

    public bool[] bulletFlag = new bool[BulletPool.BULLET_MAX];
    public int frame = 0;

    public float[] base_angle = new float[2];

    // Start is called before the first frame update
    public void Init(GameObject enemy)
    {
        _bulletPool = BulletPool.Instance;
        
        this.enemy = enemy;
        enemyScript = enemy.GetComponent<Enemy>();
        flag = true;
        frame = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!flag) return;
        if (!_bulletPool.flag) return;
        switch (State)
        {
            case EmitterState.Follow:
                x = enemy.transform.position.x;
                y = enemy.transform.position.y;
                break;
            case EmitterState.AutoMove:
                x = enemy.transform.position.x;
                y = enemy.transform.position.y;
                break;
        }
        //frame++;
    }

}
