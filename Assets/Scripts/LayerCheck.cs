﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew {
    public class LayerCheck : MonoBehaviour {

        [SerializeField] private LayerMask _layer;
        [SerializeField] private bool _isTouchingLayer;
        private Collider2D _collider;          // берем базовый класс для всех двумерных коллайдеров, чтобы была возможность проверять взаимодействие

        public bool IsTouchingLayer => _isTouchingLayer;

        private void Awake() {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerStay2D(Collider2D collision) {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
        }

        private void OnTriggerExit2D(Collider2D collision) {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
        }

    }
}
