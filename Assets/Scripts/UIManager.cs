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
        if (!isEsc)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SoundManager.Inst.SetEffectSound(0);
                escMenu.SetActive(true);
                GameManager.Inst.SetGameState(GameState.Stop);
                isEsc = true;
            }
        }
        else if(isEsc)
        {
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClickReturnToGame();
                settingPanel.SetActive(false);
            }
        }
    }

    public void OnClickReturnToGame()
    {
        SoundManager.Inst.SetEffectSound(0);
        isEsc = false;
        escMenu.SetActive(false);
        Time.timeScale = 1;
        GameManager.Inst.SetGameState(GameState.Start);
    }
    public void OnClickSetting()
    {
        SoundManager.Inst.SetEffectSound(0);
        escMenu.SetActive(false);
        settingPanel.SetActive(true);
    }
    public void OnClickExit()
    {
        SoundManager.Inst.SetEffectSound(0);
        Application.Quit();
    }
}
