using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace PixelCrew {
    public class Hero : MonoBehaviour {

        [SerializeField] private float speed;
        [SerializeField] private float jumpImpulse;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerCheck _layerCheck;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;

        private float directionX;
        private float directionY;

        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private Animator animator;
        private static readonly int isGround = Animator.StringToHash("is-ground");
        private static readonly int isRunning = Animator.StringToHash("is-running");
        private static readonly int verticalVelocity = Animator.StringToHash("vertical-velocity");

        private int coins_value = 0;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetCoin(int value, string type) {
            coins_value += value;
            Debug.Log(type + " coin, total: " + coins_value);
        }

        public void SetDirectionX(float direction) {
            directionX = direction;
        }

        public void SetDirectionY(float direction) {
            directionY = direction;
        }

        public void SaySomething() {
            Debug.Log("Hello!");
        }

        private bool IsGrounded() {
            return _layerCheck.isTouchingLayer;
        }

        private void UpgradeSpriteDirection() {
            if(directionX > 0) {
                spriteRenderer.flipX = false;
            } else if(directionX < 0) {
                spriteRenderer.flipX = true;
            }
        }

        private void FixedUpdate() {
            rb.velocity = new Vector2(directionX * speed, rb.velocity.y);

            bool isGrounded = IsGrounded();
            if(directionY > 0) {
                if(IsGrounded() && rb.velocity.y <= 0) {
                    rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
                }
            } else if(rb.velocity.y > 0) {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }

            animator.SetBool(isGround, isGrounded);
            animator.SetFloat(verticalVelocity, rb.velocity.y);
            animator.SetBool(isRunning, directionX != 0);

            UpgradeSpriteDirection();
        }

    }
}
