using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.ParticleSystemJobs;

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
            [HideInInspector] public float inputHorizontal;

            private bool canJump;
            private float jumpTimeCounter;
            private bool isJumping;

            private bool canDash = true;
            private bool isDashing;

        [Header("Ground Detection")]
            public float checkRadius;
            
            [HideInInspector] public bool isGrounded;
            [HideInInspector] public bool isOnSolidGround;
            [HideInInspector] public bool isInLava;

            [Space, SerializeField] private Transform _groundCheck;
            [SerializeField] private Transform _solidGroundCheck;
            [SerializeField] private Transform _lavaCheck;

            [Space, SerializeField] private LayerMask whatIsGround;
            [SerializeField] private LayerMask whatIsSolidGround;
            [SerializeField] private LayerMask whatIsLava;

            private bool facingRight = true;

        [Header("Respawn system")]
            private Vector2 lastSolidGroundedPos;

        [Header("Stats")]
            public int health;
            public int numOfHearts;

        [Header("Components")]
            public Rigidbody2D _playerRB;
            public Transform _playerTR;
            [SerializeField] private Animator _anim;
            [SerializeField] private TrailRenderer _trail;
            [SerializeField] private GameObject headParticles;

            private GameManager _GM;
            private InputManager _inputControls;

    #endregion
    
    private Action lavaCollision;

    #region InputHandler

        private void Awake() {
            _inputControls = new InputManager();

            _inputControls.Player.Move.performed += ctx => inputHorizontal = ctx.ReadValue<Vector2>().x;
            _inputControls.Player.Move.canceled += ctx => inputHorizontal = 0;

            _inputControls.Player.Jump.performed += ctx => {
                if(canJump) {
                    isJumping = true;
                    jumpTimeCounter = jumpTime;
                    _playerRB.velocity = Vector2.up * jumpForce;
                }
            };

            _inputControls.Player.Dash.performed += ctx => StartCoroutine(Dash());
        }

        private void Move() {
            if(canMove) {
                _playerRB.velocity = new Vector2(inputHorizontal * speed, _playerRB.velocity.y);
            }
        }

        private void Jump() {
            if(_inputControls.Player.Jump.WasPressedThisFrame()) {
                if(canJump) {
                    isJumping = true;
                    jumpTimeCounter = jumpTime;
                    _playerRB.velocity = new Vector2(_playerRB.velocity.x, jumpForce);
                }
            }

            if(_inputControls.Player.Jump.IsPressed()) {
                if(isJumping) {
                    if(jumpTimeCounter > 0) {
                        _playerRB.velocity = new Vector2(_playerRB.velocity.x, jumpForce);
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
            if(canDash) {
                canDash = false;
                isDashing = true;
                _playerRB.gravityScale = 0f;
                _playerRB.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
                _trail.emitting = true;
                yield return new WaitForSeconds(dashTime);
                _trail.emitting = false;
                isDashing = false;
                yield return new WaitForSeconds(dashCooldown);
                canDash = true;
                StartCoroutine(Blink());
            }
        }

        private IEnumerator Blink() {
            GameObject gfx = gameObject.GetComponent<Transform>().GetChild(0).gameObject;

            gfx.SetActive(false);
            yield return new WaitForSeconds(.1f);
            gfx.SetActive(true);
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

        lavaCollision = () => {
            health -= 1;
            _playerTR.position = lastSolidGroundedPos;
        };
    }

    private void Update() {
        StartCoroutine(SetAnimations());

        if(isDashing) {
            canMove = false;
            canJump = false;
        } else {
            canMove = true;
            canJump = isGrounded;
        }

        if(health < 1) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FixedUpdate() {
        if(isDashing) {
            return;
        }

        StartCoroutine(CheckGroundCollisions());
        Move();
        //Jump();

        _playerRB.gravityScale = gravityForce;

        if(!facingRight && _playerRB.velocity.x > 0) {
            Flip();
        } else if(facingRight && _playerRB.velocity.x < 0) {
            Flip();
        }

        if(isOnSolidGround) {
            lastSolidGroundedPos = _playerTR.position;
        }

        if(isInLava) {
            lavaCollision();
        }
    }

    private void OnDrawGizmosSelected() {
        // Ground Check Radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_groundCheck.position, checkRadius);
        // Lava Collider Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_lavaCheck.position, checkRadius * 1.5f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_solidGroundCheck.position, checkRadius * .5f);
    }

    private IEnumerator CheckGroundCollisions() {
        isGrounded = Physics2D.OverlapCircle(_groundCheck.position, checkRadius, whatIsGround);
        isOnSolidGround = Physics2D.OverlapCircle(_solidGroundCheck.position, checkRadius * .5f, whatIsSolidGround);
        isInLava = Physics2D.OverlapCircle(_lavaCheck.position, checkRadius * 1.5f, whatIsLava);
        yield return null;
    }

    private IEnumerator SetAnimations() {
        _anim.SetBool("isDashing", isDashing);
        _anim.SetBool("isGrounded", isGrounded);

        if(isDashing) {
            _anim.SetTrigger("Dash");
        }

        if(inputHorizontal != 0) {
            _anim.SetBool("isMoving", true);
        } else {
            _anim.SetBool("isMoving", false);
        }
        
        yield return null;
    }
    
    private void Flip() {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }



}