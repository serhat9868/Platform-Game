using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiamondImageConfig : MonoBehaviour
{
    [SerializeField] Sprite fullSprite;
    [SerializeField] GameObject emptySprite;
    [SerializeField] Transform diamondAmount;

    int firstDiamoundAmount;
    GameObject player;
    public delegate void DiamoundIncreased();
    public event DiamoundIncreased diamoundIncreased;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

       for(int i = 0; i < diamondAmount.childCount; i++)
        {
            Instantiate(emptySprite,transform);
        }

        firstDiamoundAmount = diamondAmount.childCount;

        diamoundIncreased += FillFullySprite;
    }

    public void FillSpriteEvent()
    {
        diamoundIncreased();
    }

    private void FillFullySprite()
    {
        int fullSpriteIndex = player.GetComponent<PointControl>().GetDiamondAmount() - 1;
        transform.GetChild(fullSpriteIndex).GetComponent<Image>().sprite = fullSprite;    
    }

    public int NumberOfDiamoundLeft()
    {
        return diamondAmount.childCount;
    }

    public int GetFirstDiamoundAmount()
    {
        return firstDiamoundAmount;
    }
    
}
