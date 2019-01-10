using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class testNetworkM : NetworkBehaviour {
    public InputField inField;
    public Button checkBtn;
    public Button createBtn;
    public GameObject resetBtn;
    public GameObject reStartBtn;
    public Button arrowBtn;

    public GameObject board;
    NetworkClient nc;

	void Start () {
        checkBtn.onClick.AddListener(LogIn);
        createBtn.onClick.AddListener(CreateRoom);
        arrowBtn.onClick.AddListener(Back);
    }

    public void LogIn()
    {
        board.SetActive(true);
        nc = gameObject.GetComponent<NetworkManager>().StartClient();
        nc.Connect(inField.text, 7777);
        SetBtnsActive(true);      
    }
    public void CreateRoom()
    {
        board.SetActive(true);
        gameObject.GetComponent<NetworkManager>().StartHost();
        SetBtnsActive(true);      
    }
    void Back()
    { if (reStartBtn.active == false)
        {
            SceneManager.LoadScene("00");
        }
        gameObject.GetComponent<NetworkManager>().StopClient();
        gameObject.GetComponent<NetworkManager>().StopServer();
        SetBtnsActive(false);
        board.SetActive(false);
    }

    void SetBtnsActive(bool active)
    {
        checkBtn.gameObject.SetActive(!active);
        inField.gameObject.SetActive(!active);
        createBtn.gameObject.SetActive(!active);
        resetBtn.SetActive(active);
        reStartBtn.SetActive(active);
    }
}
