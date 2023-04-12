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
    public float jumpForce;
    public float dashForce;
    public float checkRadius;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canJump = false;
    [HideInInspector] public bool canDubbleJump = false;
    [HideInInspector] public bool isGrounded;

    [SerializeField] private LayerMask whatIsGround;

    private float inputHorizontal;
    private bool facingRight = true;

    [Header("Stats")]
    public int health;
    public int numOfHearts;

    [Header("Components")]
    [InspectorName("Rigidbody2D")] public Rigidbody2D rb;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerInput input;

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

        // ANIMATIONS
        if(rb.velocity.x == 0) {
            anim.SetBool("isMoving", false);
        } else {
            anim.SetBool("isMoving", true);
        }
        anim.SetBool("isGrounded", isGrounded);

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
    #region Input System

    private void OnEnable() {
        input.onActionTriggered += PlayerInputOnActionTriggered;
    }

    private void PlayerInputOnActionTriggered(InputAction.CallbackContext ctx) {

        if(ctx.action.name == "Move") {
            inputHorizontal = ctx.ReadValue<Vector2>().x;
        }
        
        if(ctx.action.name == "Jump") {
            rb.velocity = Vector2.up * jumpForce;
        }

        if(ctx.action.name == "Dash") {
            rb.velocity = Vector2.right * dashForce;
        }
    }


    /* 
    public void OnMove(InputAction.CallbackContext ctx) {
        inputHorizontal = ctx.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        if(ctx.performed && canJump) {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    public void OnDash(InputAction.CallbackContext ctx) {
        if(ctx.performed) {
            rb.velocity = Vector2.right * dashForce;
        }
    }*/

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
