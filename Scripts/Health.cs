using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

public class Health : MonoBehaviour, ISaveable
{
    [SerializeField] float healthPoints = 100f;
    [SerializeField] Vector2 deathKick = new Vector2(10, 120);
    [SerializeField] float givenPoints = 100f;
    [SerializeField] bool isPlayer = false;

    private bool isDead = false;
    private Rigidbody2D rigidbody;
    private float maxHealth;
    private GameObject player;
    public delegate void DamageForPlayer();
    public event DamageForPlayer damageForPlayer;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        maxHealth = healthPoints;
        damageForPlayer += FindObjectOfType<HealthDisplay>().DamageEffect;
    }

    public void TakeDamage(float damage)
    {
        if (isPlayer)
        {
            FindObjectOfType<HealthDisplay>().SetDamageAmount(damage);
            damageForPlayer();
            FindObjectOfType<AudioPlayer>().PlayPlayerHitClip();
        }
        else
        {
            FindObjectOfType<AudioPlayer>().PlayEnemyHitClip();
            StartCoroutine(EnemyDamageEffect());
        }

        healthPoints = Mathf.Max(healthPoints - damage, 0);
        if (healthPoints == 0)
        {
            StartCoroutine(Die());
            isDead = true;
        }
    }

    private IEnumerator EnemyDamageEffect()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private IEnumerator Die()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        rigidbody.gravityScale = 5;

        GetComponent<BoxCollider2D>().enabled = false;

        if (isPlayer)
        {
            FindObjectOfType<SoulDisplay>().AddNumberToSoulCount(-1);
            int soulAmount = FindObjectOfType<SoulDisplay>().GetSoulAmount();
            if (soulAmount <= 0)
            {
                FindObjectOfType<TextControl>().StartGameOverText();
            }

            GetComponent<CircleCollider2D>().enabled = false;
            yield return new WaitForSeconds(3f);
            GetComponent<SpriteRenderer>().color = Color.white;

            if (soulAmount > 0)
            {
                transform.position = FindObjectOfType<PlayerMovement>().StartingPosition();
                rigidbody.gravityScale = 2f;
                GetComponent<BoxCollider2D>().enabled = true;
                GetComponent<CircleCollider2D>().enabled = true;
                isDead = false;
                healthPoints = maxHealth;
            }
        }

        else
        {
            FindObjectOfType<AudioPlayer>().PlayDieClip();
            GetComponent<CapsuleCollider2D>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            rigidbody.velocity = deathKick;
            player.GetComponent<PointControl>().IncreasePoints(givenPoints);
            player.GetComponent<PointControl>().StartPointIncreasedEvent();
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetFullyHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealthPoints()
    {
        return healthPoints;
    }

    public object CaptureState()
    {
        return healthPoints;
    }

    public void RestoreState(object state)
    {
        healthPoints = (float)state;
    }
}
