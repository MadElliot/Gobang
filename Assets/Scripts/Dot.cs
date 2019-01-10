using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{

    public ChessType dotType = ChessType.Black;
    ChessBoard CB;
    void Start()
    {
        CB = GameObject.Find("棋盘").GetComponent<ChessBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dotType == CB.turn)
        {
            gameObject.SetActive(false);
        }
    }
}
