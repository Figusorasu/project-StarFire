using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{   
    #region Variables

    [Header("Movement")]
    public float speed;

    [HideInInspector] public bool canMove = true;

    private float inputHorizontal;
    private float inputVertical;
    private bool facingRight = true;

    [Header("Jump")]
    public float jumpforce;

     public bool canJump = false;
    [HideInInspector] public bool canDubbleJump = false;
    
    [Header("Ground Detection")]
    public float checkRadius;

    [SerializeField] private LayerMask whatIsGround;

    [HideInInspector] public bool isGrounded;
    
    [Header("Components")]
    public Rigidbody2D rb;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator anim;

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

        if(other.CompareTag("Coin")) {
            Destroy(other.gameObject);
        }
        
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
