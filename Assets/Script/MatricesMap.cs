using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//放弃使用

// 子弹坐标管理
// 由于DrawMeshInstanced只能渲染1024个单位，故需要分组

public class MatricesMap
{
    public List<List<List<Matrix4x4>>> bulletMatrices = new List<List<List<Matrix4x4>>>();
    public List<int> group = new List<int>(); //组号 [材质]

    public void clear(int kndNum)
    {
        bulletMatrices.Clear();
        group.Clear();
        for (int i = 0; i < kndNum; i++)
        {
            group.Add(0); //每个材质都是0组
            bulletMatrices.Add(new List<List<Matrix4x4>>());//每个材质
            bulletMatrices[i].Add(new List<Matrix4x4>()); // 每个材质 加入一组
        }
    }

    private void addGroup(int knd)
    {
        bulletMatrices[knd].Add(new List<Matrix4x4>());
    }
    public List<Matrix4x4> get(int knd, int group)
    {
        return bulletMatrices[knd][group];
    }
    public int getGroup(int knd)
    {
        return group[knd] + 1;
    }
    public void add(int knd, Matrix4x4 matrices)
    {
        if (bulletMatrices[knd][group[knd]].Count >= 1023)
        {
            addGroup(knd);
            group[knd]++;
        }
        bulletMatrices[knd][group[knd]].Add(matrices);
    }
}