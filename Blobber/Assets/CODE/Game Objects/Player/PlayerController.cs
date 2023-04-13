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
            public float jumpForce;
            public float dashForce;

            [HideInInspector]
            public bool canMove = true, canJump = false, canDubbleJump = false;

        [Header("Ground Detection")]
            public float checkRadius;
            [HideInInspector] public bool isGrounded;

            [Space, SerializeField] private LayerMask whatIsGround;
            private float inputHorizontal;
            private bool facingRight = true;

        [Header("Stats")]
            public int health;
            public int numOfHearts;

        [Header("Components")]
            [SerializeField] private Rigidbody2D _rb;
            [SerializeField] private Transform _groundCheck;
            [SerializeField] private Animator _anim;

            private GameManager _GM;
            private InputManager _inputControls;

    #endregion
    //=======================================================================================//
    #region InputHandler

        private void Awake() {
            _inputControls = new InputManager();

            _inputControls.Player.Move.performed += ctx => inputHorizontal = ctx.ReadValue<Vector2>().x;
            _inputControls.Player.Move.canceled += ctx => inputHorizontal = 0;

            _inputControls.Player.Jump.performed += ctx => Jump();
            _inputControls.Player.Dash.performed += ctx => Dash();
        }

        private void Jump() {
            if(canJump){
                _rb.velocity = Vector2.up * jumpForce * Time.deltaTime;
            }
        }

        private void Dash() {
            Debug.Log("Dash!");
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
    }

    private void Update() {
        
        canJump = isGrounded;

        // ANIMATIONS
        if(_rb.velocity.x == 0) {
            _anim.SetBool("isMoving", false);
        } else {
            _anim.SetBool("isMoving", true);
        }
        _anim.SetBool("isGrounded", isGrounded);

    }

    private void FixedUpdate() {
        if(canMove) {
            _rb.velocity = new Vector2(inputHorizontal * speed, _rb.velocity.y) * Time.deltaTime;
        }

        if(!facingRight && _rb.velocity.x > 0) {
            Flip();
        } else if(facingRight && _rb.velocity.x < 0) {
            Flip();
        }

        isGrounded = Physics2D.OverlapCircle(_groundCheck.position, checkRadius, whatIsGround);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        
        
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
