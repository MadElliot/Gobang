using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAI0 : Player//继承自Player
{
    //声明字典集合，存储棋型及其对应的分数
    Dictionary<string, int> sign = new Dictionary<string, int>();
    Button changeTurnBtn;
    Text btnText;
    GameObject[] gos;
    void Start()
    {
        changeTurnBtn = GameObject.Find("ChangeTurn_Btn").GetComponent<Button>();
        changeTurnBtn.onClick.AddListener(ChangeTurn);
        btnText = changeTurnBtn.GetComponentInChildren<Text>();
        //存入棋型及对应分数
        sign.Add("_*_", 10);
        sign.Add("_**", 50);
        sign.Add("**_", 50);
        sign.Add("_**_", 100);

        sign.Add("_***", 100);
        sign.Add("***_", 100);
        sign.Add("_***_", 500);

        sign.Add("_****", 500);
        sign.Add("****_", 500);
        sign.Add("_****_", 1000);
        sign.Add("*****", 5000);

        sign.Add("*****_", 10000);
        sign.Add("_*****", 10000);
        sign.Add("_*****_", 10000);
    }

    private void FixedUpdate()
    {
        if (playerTurn == ChessType.Black)
        {
            temp = Time.time - timer;
            if (temp >= 1 && playerTurn == board.turn)
            {
                Play();
                timer = Time.time;
            }
        }
    }

    int CheckOneline(int[] pos, int turn, params int[] offset)
    {
        string temp = "*";//预下棋
        for (int x = pos[0] + offset[0], y = pos[1] + offset[1];
             x < 14 && x >= 0 && y < 14 && y >= 0;
             x += offset[0], y += offset[1]) //类型判断胜利的二维循环检测
        {
            if (board.grid[x, y] == turn)
            {
                temp = temp + "*";
            }
            else if (board.grid[x, y] == 0)
            {
                temp = temp + "_";
                break;
            }
            else
            {
                break;
            }
        }

        for (int x = pos[0] - offset[0], y = pos[1] - offset[1];
            x < 14 && x >= 0 && y < 14 && y >= 0;
            x -= offset[0], y -= offset[1])
        {
            if (board.grid[x, y] == turn)
            {
                temp = "*" + temp;
            }
            else if (board.grid[x, y] == 0)
            {
                temp = "_" + temp;
                break;
            }
            else
            {
                break;
            }
        }
        int score = 0;
        foreach (var key in sign.Keys)
        {
            if (temp == key)//遍历，看最终得到的字符串是否与字典中的一致，一致则将相应分数赋值
            {
                score = sign[key];
            }
        }
        return score;
    }
    int SetScore(params int[] pos)
    {
        int Sum = 0;
        Sum += CheckOneline(pos, 1, 1, 1);
        Sum += CheckOneline(pos, 1, 1, -1);
        Sum += CheckOneline(pos, 1, 0, 1);
        Sum += CheckOneline(pos, 1, 1, 0);

        Sum += CheckOneline(pos, 2, 1, 1);
        Sum += CheckOneline(pos, 2, 1, -1);
        Sum += CheckOneline(pos, 2, 0, 1);
        Sum += CheckOneline(pos, 2, 1, 0);
        return Sum;
    }

    public override void Play()//重写Player中的下棋方法
    {
        AIPlay();
    }

    void AIPlay()
    {
        int maxX = 7, maxY = 7;
        int maxScore = 80;
        //遍历整个棋盘，找到最高分的坐标
        for (int x = 0; x < 14; x++)
        {
            for (int y = 0; y < 14; y++)
            {
                if (board.grid[x, y] != 0)
                {
                    continue;
                }
                if (SetScore(x, y) > maxScore)
                {
                    maxScore = SetScore(x, y);
                    maxX = x;
                    maxY = y;
                }
            }
        }
        board.PlayChess(maxX, maxY); //进行下棋操作       
    }
    /// <summary>
    /// 换先后手
    /// </summary>
    public void ChangeTurn()
    {
        if (btnText.text == "先手")
        {
            gos = GameObject.FindGameObjectsWithTag("Chess");
            for (int i = 0; i < gos.Length; i++)
            {
                Destroy(gos[i]);
                board.grid[(int)gos[i].transform.position.x, (int)gos[i].transform.position.y] = 0;
            }
            board.turn = ChessType.White;
            btnText.text = "后手";
        }
        else
        {
            gos = GameObject.FindGameObjectsWithTag("Chess");
            for (int i = 0; i < gos.Length; i++)
            {
                Destroy(gos[i]);
                board.grid[(int)gos[i].transform.position.x, (int)gos[i].transform.position.y] = 0;
            }
            board.turn = ChessType.Black;
            btnText.text = "先手";
        }
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
}
