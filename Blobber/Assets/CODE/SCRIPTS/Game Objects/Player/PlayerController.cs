using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{   
    #region Variables

        [Header("Movement")]
            public float speed;
            [Space]
            public float jumpForce;
            public float jumpTime;
            public float gravityForce;

            [Space]
            public float dashForce;
            public float dashTime;
            public float dashCooldown;

            [HideInInspector] public bool canMove = true; 

            private bool canJump;
            private bool canDubbleJump = false;
            private float jumpTimeCounter;
            private bool isJumping;

            private bool canDash = true;
            private bool isDashing;

        [Header("Ground Detection")]
            public float checkRadius;
            
            [HideInInspector] public bool isGrounded;
            [HideInInspector] public float inputHorizontal;

            [Space, SerializeField] private LayerMask whatIsGround;
            [SerializeField] private LayerMask whatIsLava;

            private bool facingRight = true;

        [Header("Stats")]
            public int health;
            public int numOfHearts;

        [Header("Components")]
            public Rigidbody2D _rb;
            [SerializeField] private Transform _groundCheck;
            [SerializeField] private Animator _anim;
            [SerializeField] private TrailRenderer _trail;

            private GameManager _GM;
            private InputManager _inputControls;

    #endregion
    
    public delegate void LavaCollision(bool i);
    private LavaCollision lavaCollision;


    #region InputHandler

        private void Awake() {
            _inputControls = new InputManager();

            _inputControls.Player.Move.performed += ctx => inputHorizontal = ctx.ReadValue<Vector2>().x;
            _inputControls.Player.Move.canceled += ctx => inputHorizontal = 0;

            _inputControls.Player.Jump.performed += ctx => {
                if(canJump) {
                    isJumping = true;
                    jumpTimeCounter = jumpTime;
                    _rb.velocity = Vector2.up * jumpForce;
                }
            };

            _inputControls.Player.Dash.performed += ctx => StartCoroutine("Dash");
        }

        private void Jump() {
            if(_inputControls.Player.Jump.WasPressedThisFrame()) {
                if(canJump) {
                    isJumping = true;
                    jumpTimeCounter = jumpTime;
                    _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                }
            }

            if(_inputControls.Player.Jump.IsPressed()) {
                if(isJumping) {
                    if(jumpTimeCounter > 0) {
                        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                        jumpTimeCounter -= Time.deltaTime;
                    } else {
                        isJumping = false;
                    }
                }
            }

            if(_inputControls.Player.Jump.WasReleasedThisFrame()) {
                isJumping = false;
            }
        }

        private IEnumerator Dash() {
            canDash = false;
            isDashing = true;
            _rb.gravityScale = 0f;
            _rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
            _trail.emitting = true;
            yield return new WaitForSeconds(dashTime);
            _trail.emitting = false;
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

        private void OnEnable() {
            _inputControls.Player.Enable();
        }
        private void OnDisable() {
            _inputControls.Player.Disable();
        }

    #endregion

    private void Start() {
        _GM = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();

        lavaCollision = (bool isInLava) => {
            if(isInLava) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        };
    }

    private void Update() {
        _anim.SetBool("isDashing", isDashing);
        if(isDashing) {
            _anim.SetTrigger("Dash");
            canMove = false;
            canJump = false;

        } else {
            canMove = true;
            canJump = isGrounded;
        }
        
        // ANIMATIONS
        if(_rb.velocity.x == 0) {
            _anim.SetBool("isMoving", false);
        } else {
            _anim.SetBool("isMoving", true);
        }
        _anim.SetBool("isGrounded", isGrounded);

    }

    private void FixedUpdate() {
        if(isDashing) {
            return;
        }
        _rb.gravityScale = gravityForce;

        if(canMove) {
            _rb.velocity = new Vector2(inputHorizontal * speed, _rb.velocity.y);
        }

        //Jump();

        if(!facingRight && _rb.velocity.x > 0) {
            Flip();
        } else if(facingRight && _rb.velocity.x < 0) {
            Flip();
        }

        isGrounded = Physics2D.OverlapCircle(_groundCheck.position, checkRadius, whatIsGround);

        bool isInLava = Physics2D.OverlapCircle(_groundCheck.position, checkRadius, whatIsLava); 
        lavaCollision(isInLava);
    }

    private void OnDrawGizmosSelected() {
        // Ground Check Radius Display
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, checkRadius);
    }

    private void Flip() {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

}