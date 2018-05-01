using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour 
{
    [Tooltip("Next Scene Name.")]
    [SerializeField]
    string nextScene;
    [Tooltip("Menu Scene.")]
    [SerializeField]
    string menuScene;
    [Tooltip("Main object")]
    [SerializeField]
    GameObject mainMenu;
    [Tooltip("Controls object")]
    [SerializeField]
    GameObject controls;
    [Tooltip("Start button")]
    [SerializeField]
    GameObject startButton;


    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void ControlsButton(int type)
    {
        mainMenu.SetActive(false);
        controls.SetActive(true);
        if (type == 1)
            startButton.SetActive(true);
    }

    public void BackControlsButton()
    {
        controls.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BacktoMenuButton()
    {
        SceneManager.LoadScene(menuScene);
    }
}
