using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Component {
    public class TeleportComponent : MonoBehaviour {

        [SerializeField] private Transform _destination;
        [SerializeField] private float _alphaTime;
        [SerializeField] private float _moveTime;

        public void Teleport(GameObject target) {
            //target.transform.position = _destination.position;
            StartCoroutine(AnimateTeleport(target));
        }

        private IEnumerator AnimateTeleport(GameObject target) {
            SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
            PlayerInput input = target.GetComponent<PlayerInput>();
            SetLockInput(input, true);

            yield return AlphaAnimation(sprite, 0);
            target.SetActive(false);

            yield return MoveAnimation(target);

            target.SetActive(true);
            yield return AlphaAnimation(sprite, 1);
            SetLockInput(input, false);
        }

        private void SetLockInput(PlayerInput input, bool isLocked) {
            if (input != null) {
                input.enabled = !isLocked;
            }
        }

        private IEnumerator MoveAnimation(GameObject target) {
            float moveTime = 0;
            while(moveTime < _moveTime) {
                moveTime += Time.deltaTime;
                target.transform.position = Vector3.Lerp(target.transform.position, _destination.position, _moveTime * Time.deltaTime);

                yield return null;
            }
        }

        private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha) {
            float alphaTime = 0;
            var spriteAlpha = sprite.color.a;
            while(alphaTime < _alphaTime) {
                alphaTime += Time.deltaTime;
                float tmpAlpha = Mathf.Lerp(spriteAlpha, destAlpha, alphaTime / _alphaTime);
                Color color = sprite.color;
                color.a = tmpAlpha;
                sprite.color = color;

                yield return null;
            }
        }

    }
}
