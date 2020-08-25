using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BulletPool _bulletPool;
    private PlayerInput _playerInput;

    public float Speed = 5.2f;

    private PlayerState State = PlayerState.Default;
    private int frame = 0;
    // Start is called before the first frame update

    private void Start()
    {
        _bulletPool = BulletPool.Instance;
        _playerInput = PlayerInput.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        State = PlayerState.Default;
        float hInput = _playerInput.HorizontalInput;
        float vInput = _playerInput.VerticalInput;

        float horizontalMove = hInput * Speed;
        float VerticalMove = vInput * Speed;
        //Debug.LogFormat("{0} {1}", hInput, vInput);
        if (vInput * hInput != 0)
        {
            horizontalMove /= 1.414f; // Sqrt(2)
            VerticalMove /= 1.414f;
        }
        if (_playerInput.SlowButton)
        {
            horizontalMove /= 2.5f;
            VerticalMove /= 3.0f;
            State = PlayerState.SlowMove;
        }
        Vector2 playerPos = (Vector2)transform.position;
        Vector2 tempPos = (Vector2)transform.position +
            new Vector2(horizontalMove, VerticalMove) * Time.deltaTime;
        if(!(tempPos.x < - GameSetting.Instance.WidthF || tempPos.x > GameSetting.Instance.WidthF))
        {
            playerPos.x += horizontalMove * Time.deltaTime;
        }
        if (!(tempPos.y < -GameSetting.Instance.HeightF || tempPos.y > GameSetting.Instance.HeightF))
        {
            playerPos.y += VerticalMove * Time.deltaTime;
        }
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);


        if (_playerInput.ShotButton)
        {
            //Debug.Log("Shot");
            if(frame%2==0)
            {
                _bulletPool.AddBullet(true, 0, 0, playerPos.x - 0.25f, playerPos.y, Mathf.PI / 8, 0.15f);
                _bulletPool.AddBullet(true, 0, 0, playerPos.x - 0.1f, playerPos.y, 0, 0.3f);
                _bulletPool.AddBullet(true, 0, 0, playerPos.x + 0.1f, playerPos.y, 0, 0.3f);
                _bulletPool.AddBullet(true, 0, 0, playerPos.x + 0.25f, playerPos.y, -Mathf.PI / 8, 0.15f);
            }
           
        }
        frame++;
    }
}
