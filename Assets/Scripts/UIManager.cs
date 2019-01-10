using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public Button pvpButton;
    public Button pveButton;
    public Button pvpNetButton;
    public Button arrow;
    public GameObject panel;

    public Button check;
    public Button cancel;
	void Start () {
        pvpButton.onClick.AddListener(PVP);
        pveButton.onClick.AddListener(PVE);
        pvpNetButton.onClick.AddListener(PVPNet);
        arrow.onClick.AddListener(Arrow);
        check.onClick.AddListener(QuitGame);
        cancel.onClick.AddListener(Arrow);
	}

    void PVP()
    {
        SceneManager.LoadScene("01");
    }
    void PVE()
    {
        SceneManager.LoadScene("02");
    }
    void PVPNet()
    {
        SceneManager.LoadScene("03");
    }

    void Arrow()
    {
            panel.SetActive(!panel.active);
    }
    void QuitGame()
    {
        Application.Quit();
    }
}
