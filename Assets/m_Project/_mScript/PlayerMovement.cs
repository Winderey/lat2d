using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] GameObject character;
    [SerializeField] Transform characterSprite;
    [SerializeField] Transform checkGround;
    [SerializeField] SpriteRenderer playerSprite;
    public Rigidbody2D rb;
    public Transform respawnPos;


    public EnemyController ennemy;

    public float speed = 250f;
    public float force = 2f;
    float sprint = 20f;
    private float distanceRaycast = 0.3f;
    public bool isHurt = false;
    public int coinScore = 0;
    public float climbSpeed = 0f;
    public bool canClimb = false;
    public bool isClimbing = false;


    float moveInput;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask groundLayerMask;

    bool canJump = false;
    [SerializeField] bool isGrounded = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (canJump)
        {
            rb.AddForce(new Vector3(0, force, 0), ForceMode2D.Impulse);
            canJump = false;
        }
        rb.velocity = new Vector2(moveInput * Time.fixedDeltaTime * speed, rb.velocity.y);

        if (Input.GetKey(KeyCode.UpArrow) && canClimb == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, climbSpeed * Time.fixedDeltaTime);
            isClimbing = true;
            Debug.Log("HUM MAIS MONTEs");
        }
        else if (Input.GetKey(KeyCode.DownArrow) && canClimb == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, -climbSpeed * Time.fixedDeltaTime);
            isClimbing = true;
        }


    }


    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        Debug.DrawLine(checkGround.transform.position, new Vector3(checkGround.transform.position.x, checkGround.transform.position.y - distanceRaycast, checkGround.transform.position.z), UnityEngine.Color.green);
        isGrounded = Physics2D.Raycast(checkGround.transform.position, Vector3.down, distanceRaycast, groundLayerMask);
        //transform.position += new Vector3(horizontalInput * time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.Space) && (rb.velocity.y <= 0.1 && rb.velocity.y >= -0.1))
        {
            canJump = true;
        } 

        animator.SetFloat("horizontalInput", moveInput);
        animator.SetFloat("verticalInput", rb.velocity.y);
        animator.SetBool("grounded", isGrounded);
        animator.SetBool("hurt", isHurt);
        animator.SetBool("isClimbing", isClimbing);

        //transform.Translate(Vector3.forward * Time.deltaTime * speed);


        if (moveInput < 0)
        {
            characterSprite.localScale = new Vector3(Mathf.Sign(moveInput)*Mathf.Abs(characterSprite.localScale.x), characterSprite.localScale.y, characterSprite.localScale.z);
        }
        if (moveInput > 0)
        {
            characterSprite.localScale = new Vector3(Mathf.Sign(moveInput) * Mathf.Abs(characterSprite.localScale.x), characterSprite.localScale.y, characterSprite.localScale.z);
        }
        if (Physics2D.Raycast(checkGround.transform.position, Vector3.down, distanceRaycast, LayerMask.GetMask("Pente")))
        {
            rb.sharedMaterial.friction = 1f;
        }
        else
        {
            rb.sharedMaterial.friction = 0.6f;
        }




        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 425f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 250f;
        }

        


    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("entrer dans le trigger");
        if (other.tag == "Bunny")
        {

            if (transform.position.y > other.gameObject.transform.position.y + other.gameObject.GetComponent<BoxCollider2D>().bounds.size.y)
            {
                rb.AddForce(new Vector3(0, 5f, 0), ForceMode2D.Impulse);
                //ennemy.LoseHP();
                Debug.Log("souffre, chose");
                if (ennemy.isDead == true)
                {
                    other.gameObject.SetActive(false);
                }
                
            }
            else
            {
                Debug.Log("aïe");
                isHurt = true;
                rb.AddForce(new Vector3(-10, 2, 0), ForceMode2D.Impulse);
                StartCoroutine(Hurt());
            }

        }

        if (other.tag == "Ladder")
        {
            canClimb = true;
        }

        if (other.tag == "EndLadder")
        {
            rb.velocity = Vector2.zero;
            canClimb = false;
        }

        if (other.tag == "Coin")
        {
            GameManager.Instance.GetCoin();
            other.gameObject.SetActive(false);
        }

        if (other.tag == "DeathZone") //respawn au respawn pont + reset velocity
        {
            GameManager.Instance.MyMethod();
            GameManager.Instance.Respawn(gameObject.transform);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Hidden"))
        {
            other.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hidden"))
        {
            other.gameObject.GetComponent<TilemapRenderer>().enabled = true;
            Debug.Log("SORS");
        }
        if (other.tag == "Ladder")
        {
            canClimb = false;
            isClimbing = false;
        }
    }


    void InvicibilityFrame(float alpha)
    {
        playerSprite.color = new Color(1f, 1f, 1f, alpha);
    }

    IEnumerator Hurt()
    {
        for (int i = 0;
            i < 2;
            i++)
        {
            yield return new WaitForSeconds(0.25f);
            InvicibilityFrame(0.2f);
            yield return new WaitForSeconds(0.25f);
            InvicibilityFrame(1f);
        }

        isHurt = false;
    }
}
