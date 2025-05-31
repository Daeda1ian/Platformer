using UnityEngine;
using PixelCrew.Component;
using PixelCrew.Model;
using UnityEditor.Animations;
using PixelCrew.Utils;
using System.Collections;

namespace PixelCrew.Creatures {
    public class Hero : Creature {

        [SerializeField] private float _criticalFallingValue;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private int _throwQueueValue;
        [SerializeField] private float _throwQueueCooldown;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        
        [SerializeField] private SpawnComponent _swordEffect;
        [SerializeField] private ParticleSystem _hitParticles;

        [SerializeField] private Cooldown _throwCooldown;
        
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        private Collider2D[] _interactionResult = new Collider2D[1];
        
        private bool _allowDoubleJump = true;
        private bool _inAir = false;
        private GameSession _session;

        private static readonly int ThrowKey = Animator.StringToHash("throw");

        protected override void Awake() {
            base.Awake();
        }

        private void Start() {
            _session = FindObjectOfType<GameSession>();

            var health = GetComponent<HealthComponent>();
            health.SetHealth(_session.Data.Hp);

            UpdateHeroWeapon();
        }

        protected override void Update() {
            base.Update();
        }

        protected override void FixedUpdate() {
            CheckFalling();
            base.FixedUpdate();
        }

        public void OnHealthChanged(int currentHealth) {
            _session.Data.Hp = currentHealth;
        }

        public void SetCoin(int value, string type) {
            _session.Data.Coins += value;
            Debug.Log(type + " coin, total: " + _session.Data.Coins);
        }

        public void AddSword() {
            _session.Data.Swords++;
        }

        public void Interact() {
            _interactionCheck.Check();
            /*int size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _interactionResult, _interactionLayer);
            for(int i = 0; i < size; i++) {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null) {
                    interactable.Interact();
                }
            }*/
        }

        public override void TakeDamage() {
            base.TakeDamage();
            if(_session.Data.Coins > 0) {
                SpawnParticleCoins();
            }
        }

        public void SpawnParticleCoins() {
            int numberCoinsToDispose = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= numberCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numberCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        protected override float CalculateJumpVelocity(float yVelocity) {

            if (!_IsGrounded && _allowDoubleJump) {
                _allowDoubleJump = false;
                _particles.Spawn("Jump");
                return _jumpImpulse;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        protected override float CalculateYVelocity() {

            if(_IsGrounded) {
                _allowDoubleJump = true;
            }

            return base.CalculateYVelocity();
        }

        private void CheckFalling() {
            if(_IsGrounded && _inAir) {
                SpawnFallParticles();
            } else {
                _inAir = true;
            }
        }

        private void SpawnFallParticles() {
            if(MaxYVeclocity < _criticalFallingValue) {
                _particles.Spawn("Fall");
                _inAir = false;
                MaxYVeclocity = 0;
            }
        }

        public void ArmHero() {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        public void UpdateHeroWeapon() {
            if(_session.Data.IsArmed) {
                Animator.runtimeAnimatorController = _armed;
            } else {
                Animator.runtimeAnimatorController = _unarmed;
            }
        }

        public override void Attack() {
            if (!_session.Data.IsArmed) {
                return;
            }
            _swordEffect.Spawn();
            base.Attack();
        }

        public void OnDoThrow() {
            _particles.Spawn("Throw");
        }

        public void Throw() {
            if(_throwCooldown.IsReady && _session.Data.Swords > 1) {
                Animator.SetTrigger(ThrowKey);
                _throwCooldown.Reset();
                _session.Data.Swords--;
            }
        }

        public void ThrowQueue() {
            if (_throwCooldown.IsReady && _session.Data.Swords > _throwQueueValue) {
                StartCoroutine(QueueSequence(_throwQueueValue));
            } else {
                Throw();
            }
        }

        private IEnumerator QueueSequence(int value) {
            while (value > 0) {
                Animator.SetTrigger(ThrowKey);
                value--;
                _session.Data.Swords--;
                yield return new WaitForSeconds(_throwQueueCooldown);
            }
        }
    }
}
