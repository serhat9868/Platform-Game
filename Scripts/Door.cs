using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Door : MonoBehaviour
{
   [SerializeField] bool isDoorOpen = false;
    bool isPlayed = false;

    void Update()
    {      
        if(FindObjectOfType<DiamondImageConfig>().NumberOfDiamoundLeft() == 0)
        {
            GetComponent<Animator>().SetBool("isDoorOpen", true);
            isDoorOpen = true;
            DoorOpenSound();
        }
    }

    private void DoorOpenSound()
    {
        if (!isPlayed)
        {
            FindObjectOfType<AudioPlayer>().PlayDoorClip();
            isPlayed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDoorOpen)
        {
            if (collision.gameObject.tag == "Player")
            {
                StartCoroutine(LoadNextScene());              
            }
        }
    }

    private IEnumerator LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        Fader fader = FindObjectOfType<Fader>();
        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
        fader.FadeOut(2f);
        savingWrapper.Save();

        yield return SceneManager.LoadSceneAsync(nextSceneIndex);

        savingWrapper.Load();
        fader.FadeIn(2f);
        savingWrapper.Save();
    }
}
