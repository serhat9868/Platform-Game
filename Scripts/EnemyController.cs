using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour,ISaveable
{
    [SerializeField] float velocity = 5f;
    [SerializeField] bool isBird = false;

    Rigidbody2D rigidbody;
    CapsuleCollider2D capsuleCollider2D;
    bool isGiddy = false;
    int health = 2;
    float timeSinceLastAttack = Mathf.Infinity;
    GameObject player;
    float pushingForce = 18.5f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (GetComponent<Health>().IsDead()) return;
            if (capsuleCollider2D.IsTouching(FindObjectOfType<PlayerMovement>().GetFeetColliderOfPlayer())
            && timeSinceLastAttack > 0.3f )    
        {
            timeSinceLastAttack = 0;
            player.GetComponent<Rigidbody2D>().AddForce(Vector3.up*pushingForce,ForceMode2D.Impulse);
            GetComponent<Health>().TakeDamage(50);
            StartCoroutine(BeGiddy());
        
        }       
        timeSinceLastAttack += Time.deltaTime;
        if (!isGiddy) 
        {
            rigidbody.velocity = new Vector2(velocity, 0);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isBird) return;
        transform.localScale = new Vector2(-(Mathf.Sign(rigidbody.velocity.x)), 1);
        velocity = -velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBird)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(rigidbody.velocity.x)), 1);
            velocity = -velocity;
        }
    }

    public void GiddyFunc()
    {
        StartCoroutine(BeGiddy());
    }

    public IEnumerator BeGiddy()
    {
        isGiddy = true;
        GetComponent<Animator>().SetBool("isStunt", true);
        rigidbody.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(2f);

        if (velocity > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        GetComponent<Animator>().SetBool("isStunt", false);
        isGiddy = false;
    }
    public bool IsGiddy()
    {
        return isGiddy;
    }

    public void TakeDamage()
    {
        health--;
        if(health == 0)
        {
            Destroy(gameObject);
        }
    }
    public object CaptureState()
    {
        return new SerializeableVector3(transform.position);
    }

    public void RestoreState(object state)
    {
        SerializeableVector3 position = (SerializeableVector3)state;
        transform.position = position.ToVector();
    }
}
