using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetPlayer : NetworkBehaviour
{
    [SyncVar]//同步数据
    public int mark;
    [SyncVar]
    public bool isWined = false;
    RaycastHit hitInfo;
    NetChessBoard board;
    public ChessType playerTurn;
    GameObject[] players;
    Text winText;
    private void Start()
    {
        winText = GameObject.Find("win_Text").GetComponent<Text>();
        board = GameObject.Find("棋盘").GetComponent<NetChessBoard>();
        board.playerTurn = ChessType.Black;
        players = GameObject.FindGameObjectsWithTag("Player");
        players[0].GetComponent<NetPlayer>().playerTurn = ChessType.Black;

        if (players.Length >= 2)
        {
            players[1].GetComponent<NetPlayer>().playerTurn = ChessType.White;
        }

        GameObject.Find("ReSet").GetComponent<Button>().onClick.AddListener(ResetChess);
        GameObject.Find("ReStart").GetComponent<Button>().onClick.AddListener(RestartChess);

        board.ipText.text = "本机IP:\n" + Network.player.ipAddress;
    }

    void FixedUpdate()
    {
        CmdSetIsWined();
        if (!isWined)
        {            
            CmdMark();
            Play();
            if (mark == 1)
            {
                winText.text = "黑棋下";
            }
            else if (mark == 2)
            {
                winText.text = "白棋下";
            }
        }
        else
        {
            if (mark == 1)
            {
                winText.text = "白棋胜利！";

            }
            else if (mark == 2)
            {
                winText.text = "黑棋胜利！";
            }
        }
    }

    public void Play()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                if (!isLocalPlayer) return;//非本地玩家，不作下棋操作
                CmdPlay((int)playerTurn, (int)(hitInfo.point.x + 0.5f), (int)(hitInfo.point.y + 0.5f));
            }
        }
    }

    void ResetChess()
    {
        if (!isLocalPlayer || playerTurn == ChessType.旁观) return;
        CmdResetChess();
    }
    void RestartChess()
    {
        if (!isLocalPlayer || playerTurn == ChessType.旁观) return;
        CmdReStartGame();
    }

    [Command]//服务器中操作
    void CmdPlay(int playerturn, params int[] pos)
    {
        if (playerturn == (int)board.playerTurn)
        {
            board.PlayChess(pos[0], pos[1]);
        }
    }
    [Command]
    public void CmdMark()
    {
        mark = (int)board.playerTurn;
    }
    [Command]
    void CmdResetChess()
    {
        board.ResetChess();
    }
    [Command]
    void CmdReStartGame()
    {
        board.isWined = false;
        board.ReStartGame();
    }
    [Command]
    void CmdSetIsWined()
    {
        isWined = board.isWined;
    }
}
