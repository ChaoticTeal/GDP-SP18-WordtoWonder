using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour 
{
    [Tooltip("Fade out panel")]
    [SerializeField]
    Image fadePanel;
    [Tooltip("Fade out time")]
    [SerializeField]
    float fadeTime;
    [Tooltip("End scene name")]
    [SerializeField]
    string endScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        Color finalColor = fadePanel.color;
        finalColor.a = 1f;
        while(fadePanel.color != finalColor)
        {
            fadePanel.color = Color.Lerp(fadePanel.color, finalColor, fadeTime * Time.deltaTime);
            yield return null;
        }
        Cursor.visible = true;
        SceneManager.LoadScene(endScene);
    }
}
