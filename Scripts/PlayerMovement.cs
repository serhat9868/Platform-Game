using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour,ISaveable
{
    [Header("Movement")]
    [SerializeField] float JumpForce = 10f;
    [SerializeField] float speed = 3f;

    [Header("Ability")]
    [SerializeField] Vector2 rightUpAbilityForce = new Vector2(1,0.5f);
    [SerializeField] Vector2 leftUpAbilityForce = new Vector2(-1,0.5f);
    [SerializeField] Vector2 rightAbilityForce = new Vector2(1, 0);
    [SerializeField] Vector2 leftAbilityForce = new Vector2(-1, 0);
    [SerializeField] float splashForce = 45f;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] GameObject fireball;
    [SerializeField] float rollingTime = 1.4f;

    [Header("Transform Config")]
    [SerializeField] float maxX;
    [SerializeField] float minX;

    bool isInteractable = true;
    bool isFlyingUsable = true; 
    bool onAbilityMode = false;
    bool landingMode = false;
    bool isSplashMode = false;

    float timeSinceLastFire = Mathf.Infinity;
    float defaultGravityScale;
    float jumpingTime = 0;

    Vector3 startingPosition;
    Vector2 zeroVelocity = new Vector2(0, 0);
    Vector2 currentForce;
    Vector3 velocity;
    Rigidbody2D rigidbody;
    BoxCollider2D boxCollider2D;
    CircleCollider2D circleCollider2D;
    Animator playerAnimator;
    Coroutine flyingCoroutine;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        defaultGravityScale = rigidbody.gravityScale;
        playerAnimator = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        startingPosition = transform.position;
    }

    void Update()
    {
        if (GetComponent<Health>().IsDead()) return;
        Respawn();

        if (onAbilityMode) return;

        timeSinceLastFire += Time.deltaTime;
        MovementControl();

        Fight();
        Fire();
    }

    private void FixedUpdate()
    {
        SplashAbility();
        Land();
        RollingAbility();
    }

    private void Fight()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (circleCollider2D.IsTouching(enemy.GetComponent<CapsuleCollider2D>()) && isInteractable &&
                !enemy.GetComponent<EnemyController>().IsGiddy())
            {
                TakeDamage(50f);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        rigidbody.AddForce(new Vector2(0, 2));
        GetComponent<Health>().TakeDamage(damage);
        if (!GetComponent<Health>().IsDead())
        {
            StartCoroutine(DamageEffect());
        }
    }

    private void SplashAbility()
    {
        if (Mathf.Abs(rigidbody.velocity.x) > 0)
        {
            if (isSplashMode)
            {
                GetComponent<Animator>().SetBool("isStrongSplash", false);
            }
            return;
        }

        if (rigidbody.velocity.y < 0)
        {
            GetComponent<Animator>().SetBool("isStrongSplash", false);
            onAbilityMode = false;
            jumpingTime = 0;
            return;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Q))
        {
            GetComponent<Animator>().SetBool("isStrongSplash", true);
            jumpingTime += Time.deltaTime;
           
            if (jumpingTime > 3)
            {
                GetComponent<Animator>().SetBool("isStrongSplash", false);
                jumpingTime = 0;
                rigidbody.AddForce(Vector3.up *splashForce, ForceMode2D.Impulse);
                onAbilityMode = true;
            }           
        }

        else
        {
            GetComponent<Animator>().SetBool("isStrongSplash", false);
            jumpingTime = 0;
        }
    }

    private void Land()
    {
        if (landingMode) return;
        if (Input.GetKey(KeyCode.S) && (Input.GetKey(KeyCode.Q)))
        {
            StartCoroutine(LandingAbility());
        }
    }

    private IEnumerator LandingAbility()
    {
            landingMode = true;
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isLanding", true);
            rigidbody.velocity = zeroVelocity;         
            rigidbody.gravityScale = 0;
            yield return new WaitForSeconds(1.5f);
            rigidbody.gravityScale = defaultGravityScale;
            onAbilityMode = true;
            rigidbody.AddForce(Vector2.down * 50, ForceMode2D.Impulse);
            landingMode = false;
    }

    private void Respawn()
    {
        if (transform.position.x < minX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        }

        if (transform.position.x > maxX)
        {
            transform.position = new Vector2(minX, transform.position.y);
        }

    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (timeSinceLastFire > 1 / fireRate)
            {
                timeSinceLastFire = 0;
             
             GameObject instance = Instantiate(fireball, transform.position, transform.rotation);
                instance.GetComponent<Fireball>().SetScale(gameObject);

            }
        }        
    }

    IEnumerator DamageEffect()
    {
        //Fast Effect
        for (int i = 0; i < 10; i++)
        {
            isInteractable = false;
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
        }

        // Effect is slowing Down here and speed of effect is medium
        for (int i = 0; i < 5; i++)
        {
            isInteractable = false;
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.2f);
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.2f);
        }

        //Slow wink
        for (int i = 0; i < 3; i++)
        {
            isInteractable = false;
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.4f);
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(0.6f);

        GetComponent<SpriteRenderer>().color = Color.white;
        isInteractable = true;
    }

    private void RollingAbility()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Space) && isFlyingUsable)
        {
            if (onAbilityMode)
            {
                StopCoroutine(flyingCoroutine);
            }
            currentForce = leftUpAbilityForce;
            flyingCoroutine = StartCoroutine(Fly());
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Space) && isFlyingUsable)
        {
            if (onAbilityMode)
            {
                StopCoroutine(flyingCoroutine);
            }
            currentForce = rightUpAbilityForce;
            flyingCoroutine = StartCoroutine(Fly());
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Space) && isFlyingUsable)
        {
            if (onAbilityMode)
            {
                StopCoroutine(flyingCoroutine);
            }
            currentForce = rightAbilityForce;
            flyingCoroutine = StartCoroutine(Fly());
        }

        if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space) && isFlyingUsable)
        {
            if (onAbilityMode)
            {
                StopCoroutine(flyingCoroutine);
            }
                currentForce = leftAbilityForce;
            flyingCoroutine = StartCoroutine(Fly());
        }
    }

    private IEnumerator Fly()
    {
        FindObjectOfType<AudioPlayer>().PlayFlyingClip();
        isFlyingUsable = false;
        onAbilityMode = true;
        rigidbody.velocity = new Vector2(0, 0);
        GetComponent<Animator>().SetBool("isJumping", false);
        GetComponent<Animator>().SetBool("isRolling", true);
        rigidbody.AddForce(currentForce, ForceMode2D.Impulse);
        rigidbody.gravityScale = 0;
        rigidbody.drag = 3f;
        yield return new WaitForSeconds(0.4f);
        isFlyingUsable = true;
        yield return new WaitForSeconds(rollingTime);
        rigidbody.drag = 0.05f;
        onAbilityMode = false;
        rigidbody.velocity = new Vector2(0, 0);   
        rigidbody.gravityScale = defaultGravityScale;
        GetComponent<Animator>().SetBool("isRolling", false);

        this.enabled = false;
        yield return new WaitWhile(() =>  Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Space));
        this.enabled = true;

    } 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onAbilityMode && collision.gameObject.CompareTag("Enemy"))
        {
            float fullyHealth = collision.gameObject.GetComponent<Health>().GetFullyHealth();
            collision.gameObject.GetComponent<Health>().TakeDamage(fullyHealth);
            StartCoroutine(TurnOffAbilityMode());
        }

        if (landingMode)
        {
            landingMode = false;
            GetComponent<Animator>().SetBool("isLanding", false);
            StartCoroutine(TurnOffAbilityMode());
        }
    }

    private IEnumerator TurnOffAbilityMode()
    {
        yield return new WaitForSeconds(0.3f);
        onAbilityMode = false;

    }

    private void MovementControl()
    {
        velocity = new Vector3(Input.GetAxis("Horizontal"), 0);
        transform.position += velocity * Time.deltaTime * speed;
        if (Input.GetButtonDown("Jump") && rigidbody.velocity.y == 0)
        {
            FindObjectOfType<AudioPlayer>().PlayJumpingClip();
            rigidbody.AddForce(Vector3.up * JumpForce, ForceMode2D.Impulse);
            playerAnimator.SetBool("isJumping", true);
        }    

        if (playerAnimator.GetBool("isJumping") && rigidbody.velocity.y == 0)
        {
            playerAnimator.SetBool("isJumping", false);
        }
        bool playerHasHorizontalSpeed = Mathf.Abs(velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("IsRunning", playerHasHorizontalSpeed);
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            transform.localScale = new Vector3(1, 1);
        }
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            transform.localScale = new Vector3(-1, 1);
        }   
    }


    public BoxCollider2D GetFeetColliderOfPlayer()
    {
        return boxCollider2D;
    }

    public Vector3 StartingPosition()
    {
        return startingPosition;
    }

    public bool IsPlayerInteractable()
    {
        return isInteractable;
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
