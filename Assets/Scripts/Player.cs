using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float timer = 0f;
    public float temp = 0f;

    RaycastHit hitInfo;
    public ChessBoard board;
    public ChessType playerTurn;

    void FixedUpdate()
    {                                  
            if (playerTurn == board.turn)
            {
                Play();
                timer = Time.time;
            }          
    }
    public virtual void Play() 
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))//获取点击位置
            {
                board.PlayChess((int)(hitInfo.point.x + 0.5f), (int)(hitInfo.point.y + 0.5f));//调用下棋方法
            }
        }
    }
}
