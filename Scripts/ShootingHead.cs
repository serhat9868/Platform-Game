using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingHead : MonoBehaviour
{
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float xTolerance = 1f;
    [SerializeField] Sprite attackingSprite;
    [SerializeField] Sprite idleSprite;

    float time = Mathf.Infinity;
    Vector3 position;
    bool isAttacking = false;

    private void Start()
    {
        xTolerance *= transform.localScale.x;
        position = new Vector3(transform.position.x + xTolerance, transform.position.y,0);     
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > 1 / fireRate && !isAttacking)
        {
            isAttacking = true;
            GetComponent<SpriteRenderer>().sprite = attackingSprite;
            GameObject fireball = Instantiate(fireballPrefab, position, transform.rotation);
            fireball.GetComponent<Fireball>().SetScale(this.gameObject);
            time = 0;
        }

        else if(time < 1 / fireRate && isAttacking)
        {
            GetComponent<SpriteRenderer>().sprite = attackingSprite;
        }

        else if (time > 1 / fireRate && isAttacking)
        {
            GetComponent<SpriteRenderer>().sprite = idleSprite;
            time = 0;
            isAttacking = false;
        }
        
        else
        {
            GetComponent<SpriteRenderer>().sprite = idleSprite;
        }

    }
}
