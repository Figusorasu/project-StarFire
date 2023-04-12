using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{   
    #region Variables

    [Header("Movement, Jump & Ground Detection")]
    public float speed;
    public float jumpforce;
    public float checkRadius;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canJump = false;
    [HideInInspector] public bool canDubbleJump = false;
    [HideInInspector] public bool isGrounded;

    [SerializeField] private LayerMask whatIsGround;

    private float inputHorizontal;
    private float inputVertical;
    private bool facingRight = true;

    [Header("Health System")]
    public int health;
    public int numOfHearts;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHearth;
    [SerializeField] private Sprite emptyHearth;

    [Header("Components")]
    [InspectorName("Rigidbody2D")] public Rigidbody2D rb;

    [SerializeField] private Transform groundCheck;
    [SerializeField, InspectorName("Animator")] private Animator anim;

    private GameManager GM;

    #endregion
    //=======================================================================================//
    #region Unity Functions

    private void Start() {
        GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }

    private void Update() {
        if(isGrounded) {
            canJump = true;
        } else {
            canJump = false;
        }

        if(health > numOfHearts) {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health) {
                hearts[i].sprite = fullHearth;
            } else {
                hearts[i].sprite = emptyHearth;
            }

            if(i < numOfHearts) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }

    }

    private void FixedUpdate() {
        if(canMove) {
            rb.velocity = new Vector2(inputHorizontal * speed, rb.velocity.y);
        }

        if(!facingRight && rb.velocity.x > 0) {
            Flip();
        } else if(facingRight && rb.velocity.x < 0) {
            Flip();
        }



        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        
        
    }

    private void OnDrawGizmosSelected() {
        // Ground Check Radius Display
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    #endregion
    //=======================================================================================//
    #region Input System Methods

    public void CalculateMovement(InputAction.CallbackContext ctx) {
        inputVertical = ctx.ReadValue<Vector2>().y;
        inputHorizontal = ctx.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext ctx) {
        if(ctx.performed && canJump) {
            rb.velocity = Vector2.up * jumpforce;
        }
    }


    #endregion
    //=======================================================================================//
    #region Custom Methods

    private void Flip() {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }



    #endregion
}
