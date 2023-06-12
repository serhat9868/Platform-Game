using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointDisplay : MonoBehaviour
{
    GameObject player;
    bool isIncreasingMode = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (isIncreasingMode) return;
        GetComponent<TextMeshProUGUI>().text =
            String.Format("Points : {0:0}", player.GetComponent<PointControl>().GetPoints());
    }

   public void PointIncreaseEffect()
    {
        StartCoroutine(PointEffect());
    }

    IEnumerator PointEffect()
    {
        isIncreasingMode = true;
        GetComponent<TextMeshProUGUI>().color = Color.green;
        GetComponent<TextMeshProUGUI>().text =
        String.Format("Points : +{0:0}", player.GetComponent<PointControl>().GetCurrentExtraPointAmount());
        yield return new WaitForSeconds(2f);
        GetComponent<TextMeshProUGUI>().color = Color.white;
        isIncreasingMode = false;
    }
}
