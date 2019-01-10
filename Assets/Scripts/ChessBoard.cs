using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum ChessType
{
    旁观,
    Black,
    White
} //棋子类型枚举
public class ChessBoard : MonoBehaviour
{
    public Text winText;
    public Button Arrow;
    public GameObject restartBtn;
    public GameObject resetBtn;
    public GameObject changeTurnBtn;
    public GameObject[] chessPrefab;

    public int[,] grid = new int[15, 15];//声明一个整型数组存储棋盘逻辑
    public ChessType turn;
    public Stack<GameObject> chessPuth;//生命一个栈存储生成的棋子

    void Start()
    {
        Arrow.onClick.AddListener(ComeOut);
        turn = ChessType.Black;
        chessPuth = new Stack<GameObject>();
    }
    /// <summary>
    /// 下棋方法
    /// </summary>
    /// <param name="pos"></param>
    public void PlayChess(params int[] pos)
    {
        //越界或者下棋位置部位零都不进行下棋操作
        if (pos[0] < 0 || pos[1] < 0 || pos[0] > 14 || pos[1] > 14 || grid[pos[0], pos[1]] != 0 || grid[pos[0], pos[1]] != 0)
            return;

        chessPuth.Push(Instantiate(chessPrefab[(int)turn - 1], new Vector3(pos[0], pos[1], -1f), Quaternion.identity));//入栈
        grid[pos[0], pos[1]] = (int)turn; //给二维数组赋值
        if (CheckWinner(pos))
        {
            resetBtn.SetActive(false);
            changeTurnBtn.SetActive(false);
            winText.enabled = true;
            winText.text = turn + "胜利";
            Time.timeScale = 0f;
        }
        //每次下棋更换棋子种类
        if (turn == ChessType.Black)
        {
            turn = ChessType.White;
        }
        else
        {
            turn = ChessType.Black;
        }
    }
    /// <summary>
    /// 检测所有
    /// </summary>
    /// <param name="pos">棋子位置</param>
    /// <returns></returns>
    bool CheckWinner(int[] pos)
    {
        //传入四个方向
        if (CheckOneline(pos, 1, 0)) return true;
        if (CheckOneline(pos, 1, 1)) return true;
        if (CheckOneline(pos, 1, -1)) return true;
        if (CheckOneline(pos, 0, 1)) return true;
        return false;
    }
    /// <summary>
    /// 检测一行的胜负
    /// </summary>
    /// <param name="pos">棋子位置</param>
    /// <param name="offset">方向</param>
    /// <returns></returns>
    bool CheckOneline(int[] pos, params int[] offset)
    {
        int sum = 1;//计数同类型棋子个数
        for (int x = pos[0] + offset[0], y = pos[1] + offset[1];
            x < 15 && x >= 0 && y < 15 && y >= 0;
            x += offset[0], y += offset[1])//二维循环，检测两个正方向的棋子类型
        {
            if (grid[x, y] == (int)turn)
            {
                sum++;
            }
            else
            {
                break;
            }
        }

        for (int x = pos[0] - offset[0], y = pos[1] - offset[1];
            x < 15 && x >= 0 && y < 15 && y >= 0;
            x -= offset[0], y -= offset[1])//负方向
        {
            if (grid[x, y] == (int)turn)
            {
                sum++;
            }
            else
            {
                break;
            }
        }
        if (sum >= 5) 
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 悔棋方法
    /// </summary>
    public void ResetGame()
    {
        for (int i = 0; i < 2; i++)
        {
            if (chessPuth.Count > 0)
            {
                GameObject temp = chessPuth.Pop();//出栈
                grid[(int)temp.transform.position.x, (int)temp.transform.position.y] = 0;
                Destroy(temp);
            }
        }
        if (chessPuth.Count > 0)
        {
            GameObject temp1 = chessPuth.Pop();
            temp1.transform.Find("Dot").gameObject.SetActive(true);
            chessPuth.Push(temp1);
        }
    }
    /// <summary>
    /// 重开
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(Application.loadedLevel);
    }
    /// <summary>
    /// 返回开始场景
    /// </summary>
    void ComeOut()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("00");
    }
}
