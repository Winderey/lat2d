using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] GameObject character;
    [SerializeField] Transform characterSprite;
    [SerializeField] Transform checkGround;
    [SerializeField] SpriteRenderer playerSprite;
    public Rigidbody2D rb;
    public Transform respawnPos;


    public float speed = 250f;
    public float force = 2f;
    float sprint = 20f;
    private float distanceRaycast = 0.3f;
    public bool isHurt = false;
    public int coinScore = 0;


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
        if (canJump)
        {
            rb.AddForce(new Vector3(0, force, 0), ForceMode2D.Impulse);
            canJump = false;
        }
        rb.velocity = new Vector2(moveInput * Time.fixedDeltaTime * speed, rb.velocity.y);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "obstacle")
        {
            Destroy(other.gameObject);

        }
    }*/
    // Update is called once per frame
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
                other.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("aïe");
                isHurt = true;
                rb.AddForce(new Vector3(-10, 2, 0), ForceMode2D.Impulse);
                StartCoroutine(Hurt());
            }

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
            other.gameObject.SetActive(false); //  ACCORRIGER)
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
