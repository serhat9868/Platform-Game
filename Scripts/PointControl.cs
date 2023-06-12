using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointControl : MonoBehaviour,ISaveable
{
    [SerializeField] float diamoundPoints = 25;
    float points = 0;
    int diamondAmount = 0;
    public delegate void PointIncreased();
    public event PointIncreased pointIncreased;
    float currentExtraPoint = 0;

    private void Start()
    {
        pointIncreased += FindObjectOfType<PointDisplay>().PointIncreaseEffect;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Diamond")
        {
            FindObjectOfType<AudioPlayer>().PlayCollectingDiamoundClip();
            diamondAmount++;

            if(diamondAmount > FindObjectOfType<DiamondImageConfig>().GetFirstDiamoundAmount())
            {
                diamondAmount = FindObjectOfType<DiamondImageConfig>().GetFirstDiamoundAmount();
            }

            currentExtraPoint = diamoundPoints;
            points += diamoundPoints;
            pointIncreased();
            FindObjectOfType<DiamondImageConfig>().FillSpriteEvent();
            Destroy(collision.gameObject);                          
        }
    }

    public void StartPointIncreasedEvent()
    {
        pointIncreased();
    }

    public int GetDiamondAmount()
    {
        return diamondAmount;
    }

    public void IncreasePoints(float extraPoints)
    {
        points += extraPoints;
    }

    public float GetPoints()
    {
        return points;
    }
    public float GetCurrentExtraPointAmount()
    {
        return currentExtraPoint;
    }
    public void SetCurrentExtraPointAmount(float newExtraPoint)
    {
        currentExtraPoint = newExtraPoint;
    }

    public object CaptureState()
    {
        return points;
    }

    public void RestoreState(object state)
    {
        points = (float)state;
    }
}
