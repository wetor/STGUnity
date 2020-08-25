using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : Singleton<BulletPool>
{
    public static int BULLET_MAX = 5000;
    public BulletBase[] bullet = new BulletBase[BULLET_MAX];


    public List<List<Material>> bulletMaterial = new List<List<Material>>(); //材质

    //private MatricesMap bulletMap = new MatricesMap(); //位置 [材质][组号][序号]
    
    

    private Mesh mesh;

    public bool flag = false;

    private CircleCollider2D playerCollider;
    private List<CircleCollider2D> enemyCollider = new List<CircleCollider2D>();


    //private ComputeBuffer positionBuffer;
    //private ComputeBuffer argsBuffer;
    //private ComputeBuffer colorBuffer;

    //private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

    public int BulletNum = 0;

    public void Init(List<List<Material>> bulletMaterial, Mesh mesh)
    {
        playerCollider = GameObject.Find("PlayerCollider").GetComponent<CircleCollider2D>();
        

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("EnemyCollider"))
        {
            //Debug.Log(go.GetComponent<CircleCollider2D>());
          
            enemyCollider.Add(go.GetComponent<CircleCollider2D>());
        }
        this.bulletMaterial = bulletMaterial;
        this.mesh = mesh;
        //argsBuffer = new ComputeBuffer(5, sizeof(uint), ComputeBufferType.IndirectArguments);
        
        //bulletMap.clear(bulletMaterial.Count);

        for (int i = 0; i < BULLET_MAX; i++)
            bullet[i] = new BulletBase();

        Debug.Log("BulletPool Init");
        flag = true;
        //UpdateBuffers();
    }
    private int SearchBullet()
    {
        for(int i = 0; i < BULLET_MAX; i++)
        {
            if (!bullet[i].flag)
                return i;
        }
        return -1;
    }

    public int AddBullet(bool isPlayer, int type, int color, float x, float y, float angle, float speed, int emitterId = -1)
    {
        if (!flag) return -1;
        int id = SearchBullet();
        if (id >= 0)
        {
            bullet[id].type = type;
            bullet[id].pos.x = x;
            bullet[id].pos.y = y;
            bullet[id].angle = angle;
            bullet[id].speed = speed;
            bullet[id].radius = 0.05f;
            bullet[id].color = color;
            //bullet[id].material = bulletMaterial[type][color];

            bullet[id].emitterId = emitterId;
            bullet[id].bulletId = id;
            bullet[id].Init(isPlayer);

        }

        return id;
    }
    public void DrawDebug()
    {
        if (!flag) return;
        Gizmos.color = Color.red;
        for (int i = 0; i < BULLET_MAX; i++)
        {
            if (bullet[i].flag)
            {
                  Gizmos.DrawSphere(new Vector3(bullet[i].pos.x, bullet[i].pos.y, -1f), bullet[i].radius);
            }
        }
    }
    public void UpdateBullet()
    {
        if (!flag) return;
        BulletNum = 0;
        //bulletMap.clear(bulletMaterial.Count); // 清理

        for (int i = 0; i < BULLET_MAX; i++)
        {
            if (bullet[i].flag)
            {
                BulletNum++;
                bullet[i].Update();
                
                if (!bullet[i].isPlayer) //敌方子弹击中玩家
                {
                    if (Collider.Judge(bullet[i], playerCollider.transform.position.x, playerCollider.transform.position.y, playerCollider.radius))
                    {

                        //Debug.Log("被击中");
                        bullet[i].Destroy();
                        continue;
                    }
                }
                else  // 玩家子弹击中敌方
                {
                    for (int j = 0; j < enemyCollider.Count; j++)
                    {
                        if (Collider.Judge(bullet[i], enemyCollider[j].transform.position.x, enemyCollider[j].transform.position.y, enemyCollider[j].radius))
                        {
                            //Debug.Log("被击中");
                            bullet[i].Destroy();
                            continue;
                        }
                    }
                    
                }
                Graphics.DrawMesh(mesh, bullet[i].matrices, bulletMaterial[bullet[i].type][bullet[i].color], 0);

                //bulletMap.add(bullet[i].knd, bullet[i].matrices); // 储存每个子弹的坐标信息
            }  
        }

        /*for (int i = 0; i < bulletMaterial.Count; i++)
            for (int j = 0; j < bulletMap.getGroup(i); j++) 
                Graphics.DrawMeshInstanced(mesh, 0, bulletMaterial[i], bulletMap.bulletMatrices[i][j]); //显示*/



        // Update starting position buffer
        //UpdateBuffers();
        //Graphics.DrawMeshInstancedIndirect(mesh, 0, bulletMaterial[0], new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), argsBuffer);
    }
/*    void UpdateBuffers()
    {
        BulletNum = 0;
        // Positions & Colors
        if (positionBuffer != null) positionBuffer.Release();

        positionBuffer = new ComputeBuffer(BULLET_MAX, 16);

        Vector4[] positions = new Vector4[BULLET_MAX];

        for (int i = 0; i < BULLET_MAX; i++)
        {
            if (bullet[i].flag)
            {
                BulletNum++;
                bullet[i].Update();
                positions[i] = new Vector4(bullet[i].x, bullet[i].y, 1, 30);
            }
        }

        positionBuffer.SetData(positions);

        bulletMaterial[0].SetBuffer("positionBuffer", positionBuffer);

        // indirect args
        uint numIndices = (mesh != null) ? (uint)mesh.GetIndexCount(0) : 0;
        args[0] = numIndices;
        args[1] = (uint)BULLET_MAX;
        argsBuffer.SetData(args);

        cachedInstanceCount = BULLET_MAX;
    }*/
}
