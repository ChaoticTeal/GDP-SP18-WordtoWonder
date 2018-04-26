using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
    [Tooltip("Next Scene Name.")]
    [SerializeField]
    string nextScene;

	public void PlayButton()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
