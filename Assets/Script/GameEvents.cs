using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{


    private BulletPool _bulletPool;
    private EmitterPool _emitterPool;

    void Start()
    {
        //_bulletPool = BulletPool.Instance;
        //_emitterPool = EmitterPool.Instance;

    }

    // Update is called once per frame
    void Update()
    {

        BulletPool.Instance.UpdateBullet();
        EmitterPool.Instance.UpdateEmitter();

    }
    void OnDrawGizmos()
    {
        BulletPool.Instance.DrawDebug();
    }

}
