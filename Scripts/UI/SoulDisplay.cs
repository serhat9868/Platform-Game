using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulDisplay : MonoBehaviour, ISaveable
{
    [SerializeField] int soul = 1;

    void Update()
    {      
        GetComponent<TextMeshProUGUI>().text = String.Format("Soul : {0:0} ",soul);
    }
    public object CaptureState()
    {      
        return soul;
    }
    public void RestoreState(object state)
    {
        soul = (int)state;
    }

    public void AddNumberToSoulCount(int soulAmount)
    {
        soul += soulAmount;
    }

    public int GetSoulAmount()
    {
        return soul;
    }
}
