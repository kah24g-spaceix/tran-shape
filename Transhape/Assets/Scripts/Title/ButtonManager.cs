using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    [SerializeField] private GameObject exitPopUp;
    [SerializeField] private GameObject titlePopUp;
    [SerializeField] private GameObject escapePopUp;
    private GameObject gameDirector;

    void Start()
    {
        gameDirector = GameObject.Find("GameDirector");
    }
    public void StartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    //Title
    public void TitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void AskTitleButton()
    {
        titlePopUp.SetActive(true);
    }
    public void TitleSayNo()
    {
        titlePopUp.SetActive(false);
    }

    //Resume
    public void ResumeButton()
    {
        gameDirector.GetComponent<GameDirector>().isEscape = false;
        escapePopUp.SetActive(false);
    }

    //Exit
    public void ExitButton()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    public void AskExitButton()
    {
        exitPopUp.SetActive(true);
    }
    public void ExitSayNo()
    {
        exitPopUp.SetActive(false);
    }
    public void ReStartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
