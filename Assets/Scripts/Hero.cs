﻿using System.Collections;
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

        private Rigidbody2D rb;

        private int coins_value = 0;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
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

        private void FixedUpdate() {
            rb.velocity = new Vector2(directionX * speed, rb.velocity.y);

            if(directionY > 0) {
                if(IsGrounded() && rb.velocity.y <= 0) {
                    rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
                }
            } else if(rb.velocity.y > 0) {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }

    }
}
