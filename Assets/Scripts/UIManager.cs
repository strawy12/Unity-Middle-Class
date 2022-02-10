using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject escMenu;
    [SerializeField]
    private GameObject settingPanel;
    [SerializeField]
    private GameObject titleCanvas;
    private bool isEsc = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        GameManager.Inst.SetGameState(GameState.Stop);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isEsc)
            {
                Time.timeScale = 0f;
                escMenu.SetActive(true);
                GameManager.Inst.SetGameState(GameState.Stop);
                isEsc = true;
            }

            else
            {
                OnClickReturnToGame();
                settingPanel.SetActive(false);
            }
        }
    }

    public void OnClickStartBtn()
    {
        titleCanvas.SetActive(false);
        OnClickReturnToGame();
    }

    public void OnClickReturnToGame()
    {
        isEsc = false;
        escMenu.SetActive(false);
        Time.timeScale = 1;
        GameManager.Inst.SetGameState(GameState.Start);
    }
    public void OnClickSetting()
    {
        escMenu.SetActive(false);
        settingPanel.SetActive(true);
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}
