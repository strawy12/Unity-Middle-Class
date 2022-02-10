using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject escMenu;
    [SerializeField]
    private GameObject settingPanel;
    private bool isEsc = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Inst.gameState != GameState.Clear)
        {
            if (!isEsc)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    escMenu.SetActive(true);
                    GameManager.Inst.SetGameState(GameState.Stop);
                    isEsc = true;
                }
            }
            else if (isEsc)
            {
                Time.timeScale = 0;
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OnClickReturnToGame();
                    settingPanel.SetActive(false);
                }
            }
        }
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
    public void OnClickRestart()
    {
        StageManager.Inst.ReStart();
    }
}
