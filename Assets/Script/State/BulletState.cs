using System.Text;
using System.Threading.Tasks;

public enum BulletState
{
    Default = 0, //默认：使用速度运动
    Static = 1,   //暂停
    MotionDisp = 2, // 使用位移运动（使用vx、vy运动，angle为计算值）

}
