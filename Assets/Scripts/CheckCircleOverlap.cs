﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PixelCrew.Utils;

namespace PixelCrew {
    public class CheckCircleOverlap : MonoBehaviour {

        [SerializeField] private float _radius = 1f;

        private Collider2D[] _interactionResult = new Collider2D[5];

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

    }
}
