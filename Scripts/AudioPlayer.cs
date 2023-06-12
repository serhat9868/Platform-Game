using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Flying")]
    [SerializeField] AudioClip flyingClip;
    [Range(0f, 1f)]
    [SerializeField] float flyingVolume = 0.5f;

    [Header("Diamound")]
    [SerializeField] AudioClip diamoundClip;
    [Range(0f, 1f)]
    [SerializeField] float diamoundVolume = 0.5f;

    [Header("Jump")]
    [SerializeField] AudioClip jumpingClip;
    [Range(0f, 1f)]
    [SerializeField] float jumpingVolume = 0.5f;

    [Header("Enemy Die")]
    [SerializeField] AudioClip enemyDieClip;
    [Range(0f, 1f)]
    [SerializeField] float enemyDieVolume = 0.5f;

    [Header("Door")]
    [SerializeField] AudioClip doorOpenClip;
    [Range(0f, 1f)]
    [SerializeField] float doorVolume = 0.5f;

    [Header("Enemy Hit")]
    [SerializeField] AudioClip enemyHitClip;
    [Range(0f, 1f)]
    [SerializeField] float enemyHitVolume = 0.5f;

    [Header("Player Hit")]
    [SerializeField] AudioClip playerHitClip;
    [Range(0f, 1f)]
    [SerializeField] float playerHitVolume = 0.5f;

    public void PlayFlyingClip()
    {
        if(flyingClip != null)
        {
            AudioSource.PlayClipAtPoint(flyingClip, Camera.main.transform.position, flyingVolume);
        }
    }

    public void PlayCollectingDiamoundClip()
    {
        if (flyingClip != null)
        {
            AudioSource.PlayClipAtPoint(diamoundClip, Camera.main.transform.position, diamoundVolume);
        }
    }
    public void PlayJumpingClip()
    {
        if (jumpingClip != null)
        {
            AudioSource.PlayClipAtPoint(jumpingClip, Camera.main.transform.position, jumpingVolume);
        }
    }

    public void PlayDieClip()
    {
        if (enemyDieClip != null)
        {
            AudioSource.PlayClipAtPoint(enemyDieClip, Camera.main.transform.position, enemyDieVolume);
        }
    }
    public void PlayDoorClip()
    {
        if (doorOpenClip != null)
        {
            AudioSource.PlayClipAtPoint(doorOpenClip, Camera.main.transform.position, doorVolume);
        }
    }

    public void PlayEnemyHitClip()
    {
        if (enemyHitClip != null)
        {
            AudioSource.PlayClipAtPoint(enemyHitClip, Camera.main.transform.position, enemyHitVolume);
        }
    }
    public void PlayPlayerHitClip()
    {
        if (playerHitClip != null)
        {
            AudioSource.PlayClipAtPoint(playerHitClip, Camera.main.transform.position, playerHitVolume);
        }
    }
}
