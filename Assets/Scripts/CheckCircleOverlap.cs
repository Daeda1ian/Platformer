﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PixelCrew.Utils;
using UnityEngine.Events;
using System;
using System.Linq;

namespace PixelCrew {
    public class CheckCircleOverlap : MonoBehaviour {

        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlap;

        private Collider2D[] _interactionResult = new Collider2D[10];

        public GameObject[] GetObjectsInRange() {
            int size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactionResult);

            List<GameObject> overlaps = new List<GameObject>();
            for (int i = 0; i < size; i++) {
                overlaps.Add(_interactionResult[i].gameObject);
            }

            return overlaps.ToArray();
        }

        private void OnDrawGizmosSelected() {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }

        public void Check() {
            int size = Physics2D.OverlapCircleNonAlloc(transform.position, 
                _radius, 
                _interactionResult, _mask);

            for(int i = 0; i < size; i++) {
                var overlapResult = _interactionResult[i];
                var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
                if(isInTags) {
                    _onOverlap?.Invoke(_interactionResult[i].gameObject);
                }
            }
        }

        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject> {

        }

    }
}
