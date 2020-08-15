using UnityEngine;

public class LearnLoop : MonoBehaviour
{

    public int i = 1;

    public Transform cube;
    // Start is called before the first frame update
    void Start()
    {
        if (true)
        {
            print("我是判斷式~");
        }

        while(i < 7)
        {
            print("我是迴圈 while " + i);
            i++;
        }

        for (int i = 0; i < 7; i++)
        {
            print("我是迴圈for" + i);
        }

        for (int i = 0; i < 10; i++)
        {
            Vector3 pos = new Vector3(i, 0, 0);
            //生成(物件,座標,角度)
            //Quaternion.identity : 零角度
            Instantiate(cube, pos, Quaternion.identity);
        }
    }   
}
