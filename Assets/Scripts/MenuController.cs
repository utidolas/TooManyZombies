using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject LeaveButton;

    private void Start()
    {
        #if UNITY_STANDALONE || UNITY_EDITOR
        LeaveButton.SetActive(true);
        #endif

    }

    IEnumerator ChangeScene(string name)
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(name);
    }

    public void PlayGame()
    {
        StartCoroutine(ChangeScene("MainScene"));
    }

    public void QuitGame()
    {
        StartCoroutine(Leave());
    }

    IEnumerator Leave() 
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
    }
}
