using UnityEngine;
using PixelCrew.Component;

namespace PixelCrew.Creatures {
    public class Creature : MonoBehaviour {

        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpImpulse;
        [SerializeField] private float _damageJumpImpulse;
        //[SerializeField] private int _damage;

        [Header("Checkers")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerCheck _layerCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent _particles;
        [SerializeField] private Vector3 _groundCheckPositionDelta;
        [SerializeField] private float _groundCheckRadius;

        protected Rigidbody2D Rigidbody;
        protected Animator Animator;
        protected float DirectionX;
        protected float DirectionY;
        protected bool _IsGrounded;
        private bool _isJumping = false;
        protected float MaxYVeclocity;

        private static readonly int isGround = Animator.StringToHash("is-ground");
        private static readonly int isRunning = Animator.StringToHash("is-running");
        private static readonly int verticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int hit = Animator.StringToHash("hit");
        private static readonly int attackKey = Animator.StringToHash("attack");

        protected virtual void Awake() {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        protected virtual void Update() {
            _IsGrounded = _layerCheck.IsTouchingLayer;
            //_IsGrounded = IsGrounded();
        }

        protected virtual void FixedUpdate() {
            float xVelocity = DirectionX * _speed;
            float yVelocity = CalculateYVelocity();

            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            Animator.SetBool(isGround, _IsGrounded);
            Animator.SetFloat(verticalVelocity, Rigidbody.velocity.y);
            Animator.SetBool(isRunning, DirectionX != 0);

            UpgradeDirection(new Vector2(DirectionX, DirectionY));
        }

        private bool IsGrounded() {
            var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, 
                _groundCheckRadius, 
                Vector2.down, 0, _groundLayer);
            return hit.collider != null;
        }

        public void SetDirectionX(float direction) {
            DirectionX = direction;
        }

        public void SetDirectionY(float direction) {
            DirectionY = direction;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity) {
            
            if(_IsGrounded) {
                yVelocity += _jumpImpulse;
                _particles.Spawn("Jump");
            } 

            return yVelocity;
        }

        protected virtual float CalculateYVelocity() {
            float yVelocity = Rigidbody.velocity.y;
            bool isJumpPressing = DirectionY > 0;

            if(_IsGrounded) {
                _isJumping = false;
            }

            if(isJumpPressing) {
                _isJumping = true;
                bool isFalling = yVelocity <= 0.01f;
                if(!isFalling) return yVelocity;
                
                yVelocity = CalculateJumpVelocity(yVelocity);
            } else if(Rigidbody.velocity.y > 0 && _isJumping) {  
                yVelocity *= 0.75f;
            }

            if(MaxYVeclocity > yVelocity) {
                MaxYVeclocity = yVelocity;
            }

            return yVelocity;
        }

        public void UpgradeDirection(Vector2 direction) {
            var multiplier = _invertScale ? -1 : 1;
            if(direction.x > 0) {
                transform.localScale = new Vector3(multiplier, 1, 1);
            } else if(direction.x < 0) {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }

        public virtual void TakeDamage() {
            //Debug.Log("Take Damage: " + gameObject.name);
            _isJumping = false;
            Animator.SetTrigger(hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageJumpImpulse);
        }

        public virtual void Attack() {
            Animator.SetTrigger(attackKey);
        }

        public void OnAttack() {
            _attackRange.Check();
            /*GameObject[] gos = _attackRange.GetObjectsInRange();
            foreach(var go in gos) {
                var healthComponent = go.GetComponent<HealthComponent>();
                if(healthComponent != null && go.CompareTag("Enemy")) {
                    healthComponent.ManageHealthPoints(-_damage);
                }
            }*/
        }

    }
}
