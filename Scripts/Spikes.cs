using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spikes : MonoBehaviour
{
    bool isHazardous = true;
    [SerializeField] float hazardousRange = 0.1f;
    [SerializeField] float spikeDamage = 25f;
    [Range(0, 1)]
    [SerializeField] float alpha = 0.5f;
    GameObject player;
    Tilemap tilemap;
    float time = Mathf.Infinity;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tilemap = GetComponent<Tilemap>();      
    }

    void Update()
    {

        SetSprite(alpha);
        time += Time.deltaTime;

        if (isHazardous)
        {         
            if (time > 1 / hazardousRange)
            {
                time = 0;
                isHazardous = false;
            }
        }

        else
        {          
            if (time > 1 / hazardousRange)
            {            
                time = 0;
                isHazardous = true;
            }
        }   
    }

    private void SetSprite(float alpha)
    {
        if (isHazardous)
        {
            var newColor = tilemap.color;
            newColor.a = 1f;
            tilemap.color = newColor;
        }

        else
        {
            var newColor = tilemap.color;
            newColor.a = alpha;
            tilemap.color = newColor;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHazardous && collision.gameObject == player && player.GetComponent<PlayerMovement>().IsPlayerInteractable())
        {
            Debug.Log("Spike give damage to player");
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            player.GetComponent<PlayerMovement>().TakeDamage(spikeDamage);
        }
    }
}
