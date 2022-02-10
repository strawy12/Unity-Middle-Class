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
    [SerializeField]
    private GameObject tutorialPanel;
    [SerializeField]
    private Text tutorialText;
    [SerializeField]
    [TextArea]
    private string[] tutorialString;
    private int tutorialTextNum = 0;
    private bool isTutorialed = false;
    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        Time.timeScale = 0f;
        GameManager.Inst.SetGameState(GameState.Stop);
=======
        if(!StageManager.Inst.isTutorial)
        {
            TurnOnTutorialPanel();
        }
>>>>>>> injun
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isEsc)
            {
                SoundManager.Inst.SetEffectSound(0);
                escMenu.SetActive(true);
                GameManager.Inst.SetGameState(GameState.Stop);
                isEsc = true;
            }

            else
=======
        if(isTutorialed)
        {
            if(Input.GetMouseButtonDown(0))
            {
                NextTutorialText();
            }
            
        } 
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
>>>>>>> injun
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

    public void OnClickStartBtn()
    {
        titleCanvas.SetActive(false);
        OnClickReturnToGame();
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

        Text text = null;
    }
    public void OnClickExit()
    {
        SoundManager.Inst.SetEffectSound(0);
        Application.Quit();
    }
    public void OnClickRestart()
    {
        StageManager.Inst.ReStart();
    }
    public void TurnOnTutorialPanel()
    {
        tutorialPanel.SetActive(true);
        tutorialText.text = tutorialString[tutorialTextNum];
        isTutorialed = true;
    }
    public void NextTutorialText()
    {
       if(tutorialTextNum >= 3)
        {
            isTutorialed = false;
            StageManager.Inst.isTutorial = true;
            tutorialPanel.SetActive(false);
        }
       else {
            tutorialText.text = "";
            tutorialTextNum++;
            tutorialText.DOText(tutorialString[tutorialTextNum], tutorialString[tutorialTextNum].Length * 0.03f);
       }
    }
}
