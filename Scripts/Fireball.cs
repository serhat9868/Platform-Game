using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] float fireballSpeed = 5f;
    [SerializeField] float lifetime = 5f;
    [SerializeField] bool isPlayer = false;
    ShootingHead shootingHead;
    float xSpeed;
    Vector2 velocity;
    float time = 0;

    public void SetScale(GameObject shootingHead)
    {
        xSpeed = shootingHead.transform.localScale.x * fireballSpeed;
        velocity = new Vector2(xSpeed, 0);
        transform.localScale = new Vector2(shootingHead.transform.localScale.x, 1);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(time > lifetime)
        {
            Destroy(gameObject);
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(xSpeed, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && FindObjectOfType<PlayerMovement>().IsPlayerInteractable() && !isPlayer)
        {
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(50f);
            Destroy(gameObject);
        }

        if (isPlayer)
        {
            if(collision.gameObject.GetComponent<Health>() != null && collision.gameObject.tag != "Player")
            {
                collision.gameObject.GetComponent<Health>().TakeDamage(50f);
                Destroy(gameObject);
            }
            else if(collision.gameObject.GetComponent<Health>() == null)
            {
                Destroy(gameObject);
            }
        }

        if(collision.gameObject.tag != "ShootingHead" && !isPlayer)
        {
            Destroy(gameObject);
        }
    }
}
