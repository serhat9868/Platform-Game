using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    float time = 0;
  public void StartGame()
    {
       StartCoroutine(StartFunction());
    }
    private IEnumerator  StartFunction()
    {
        yield return new WaitForSeconds(0.5f);
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
