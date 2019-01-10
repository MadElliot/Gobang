using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetChessBoard : NetworkBehaviour
{
    public bool isWined;

    public ChessType playerTurn;
    public Text ipText;
    public GameObject restartBtn;
    public GameObject resetBtn;
    public GameObject[] chessPrefab;
    public int[,] grid = new int[15, 15];
   
    public Stack<GameObject> chessPuth;
    void Update()
    {
        if (chessPuth.Count == 0)
        {
            playerTurn = ChessType.Black;
        }                
    }
    void Start()
    {       
        isWined = false;
        Time.timeScale = 1f;
        playerTurn = ChessType.Black;
        chessPuth = new Stack<GameObject>();
    }

    public void PlayChess(params int[] pos)
    {
        if (pos[0] < 0 || pos[1] < 0 || pos[0] > 14 || pos[1] > 14 || grid[pos[0], pos[1]] != 0 || grid[pos[0], pos[1]] != 0)
            return;

        GameObject spawnChess = GameObject.Instantiate(chessPrefab[(int)playerTurn - 1], new Vector3(pos[0], pos[1], -1f), Quaternion.identity);
        chessPuth.Push(spawnChess);
        NetworkServer.Spawn(spawnChess);

        grid[pos[0], pos[1]] = (int)playerTurn; //给二维数组赋值
        isWined = CheckWinner(pos);      

        if (playerTurn == ChessType.Black)
        {
            playerTurn = ChessType.White;
        }
        else if (playerTurn == ChessType.White)
        {
            playerTurn = ChessType.Black;
        }
        else
        {
            return;
        }
    }

    public bool CheckWinner(int[] pos)
    {
        if (CheckOneline(pos, 1, 0)) return true;
        if (CheckOneline(pos, 1, 1)) return true;
        if (CheckOneline(pos, 1, -1)) return true;
        if (CheckOneline(pos, 0, 1)) return true;
        return false;
    }

    bool CheckOneline(int[] pos, params int[] offset)
    {
        int sum = 1;
        for (int x = pos[0] + offset[0], y = pos[1] + offset[1];
            x < 15 && x >= 0 && y < 15 && y >= 0;
            x += offset[0], y += offset[1])
        {
            if (grid[x, y] == (int)playerTurn)
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
            x -= offset[0], y -= offset[1])
        {
            if (grid[x, y] == (int)playerTurn)
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

    public void ResetChess()
    {
        for (int i = 0; i < 2; i++)
        {
            if (chessPuth.Count > 0)
            {
                GameObject temp = chessPuth.Pop();
                grid[(int)temp.transform.position.x, (int)temp.transform.position.y] = 0;
                Destroy(temp);
            }
        }
    }
    public void ReStartGame()
    {        
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Chess");
        for (int i = 0; i < gos.Length; i++)
        {
            Destroy(gos[i]);
            grid[(int)gos[i].transform.position.x, (int)gos[i].transform.position.y] = 0;
        }
    }
}
