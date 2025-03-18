using UnityEngine;
using PixelCrew.Component;

namespace PixelCrew {
    public class Hero : MonoBehaviour {

        [SerializeField] private float speed;
        [SerializeField] private float _jumpImpulse;
        [SerializeField] private float _criticalFallingValue;
        [SerializeField] private float _damageJumpImpulse;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerCheck _layerCheck;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpParticles;
        [SerializeField] private SpawnComponent _fallParticles;
        [SerializeField] private ParticleSystem _hitParticles;

        private float directionX;
        private float directionY;
        private float maxYVeclocity;

        private Collider2D[] _interactionResult = new Collider2D[1];
        private Rigidbody2D rb;
        private Animator animator;
        private static readonly int isGround = Animator.StringToHash("is-ground");
        private static readonly int isRunning = Animator.StringToHash("is-running");
        private static readonly int verticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int hit = Animator.StringToHash("hit");

        private int coins_value = 0;
        private bool _allowDoubleJump = true;
        private bool _isGrounded;
        private bool _inAir = false;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
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
            var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius, Vector2.down, 0, _groundLayer);
            return hit.collider != null;
            //return _layerCheck.isTouchingLayer;
        }

        private void UpgradeDirection() {
            if(directionX > 0) {
                transform.localScale = Vector3.one;
            } else if(directionX < 0) {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        public void Interact() {
            int size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _interactionResult, _interactionLayer);
            for(int i = 0; i < size; i++) {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null) {
                    interactable.Interact();
                }
            }
        }

        public void SpawnFootDust() {
            _footStepParticles.Spawn();
        }

        public void TakeDamage() {
            animator.SetTrigger(hit);
            rb.velocity = new Vector2(rb.velocity.x, _damageJumpImpulse);
            if(coins_value > 0) {
                SpawnParticleCoins();
            }
        }

        public void SpawnParticleCoins() {
            int numberCoinsToDispose = Mathf.Min(coins_value, 5);
            coins_value -= numberCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numberCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        private float CalculateJumpVelocity(float yVelocity) {
            bool isFalling = yVelocity <= 0.01f;
            if(!isFalling) return yVelocity;

            if (_isGrounded) {
                yVelocity += _jumpImpulse;
                _jumpParticles.Spawn();
            } else if (_allowDoubleJump) {
                yVelocity = _jumpImpulse;
                _allowDoubleJump = false;
                _jumpParticles.Spawn();
            }

            return yVelocity;
        }

        private float CalculateYVelocity() {
            float yVelocity = rb.velocity.y;
            bool isJumpPressing = directionY > 0;

            if(_isGrounded) {
                _allowDoubleJump = true;
            }

            if (isJumpPressing && yVelocity <= _jumpImpulse) {
                yVelocity = CalculateJumpVelocity(yVelocity);
            } else if (yVelocity > 0) {
                yVelocity *= 0.5f;
            }
            
            if (maxYVeclocity > yVelocity) {
                maxYVeclocity = yVelocity;
            }

            return yVelocity;
        }

        private void SpawnFallParticles() {
            if(maxYVeclocity < _criticalFallingValue) {
                _fallParticles.Spawn();
                _inAir = false;
                maxYVeclocity = 0;
            }
        }

        private void CheckFalling() {
            if(_isGrounded && _inAir) {
                SpawnFallParticles();
            } else {
                _inAir = true;
            }
        }

        private void FixedUpdate() {
            float xVelocity = directionX * speed;
            float yVelocity = CalculateYVelocity();
            
            rb.velocity = new Vector2(xVelocity, yVelocity);
            
            CheckFalling();
            animator.SetBool(isGround, _isGrounded);
            animator.SetFloat(verticalVelocity, rb.velocity.y);
            animator.SetBool(isRunning, directionX != 0);

            UpgradeDirection();
        }

        private void Update() {
            _isGrounded = IsGrounded();
        }
    }
}
