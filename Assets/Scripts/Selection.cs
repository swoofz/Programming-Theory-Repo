using System.Collections;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

namespace WaveSurvivor {
    public class Selection : MonoBehaviour {

        private RectTransform m_RectTransform;
        private Vector2 m_offset;

        private void Awake() {
            m_RectTransform = GetComponent<RectTransform>();
            m_offset = new Vector2(Screen.width, Screen.height) / 2;
            ResetSelection();
        }

        // Update is called once per frame
        void Update() {
            if (IsSelecting()) return;

            if (Input.GetMouseButtonUp(0)) ResetSelection();
            m_RectTransform.anchoredPosition = (Vector2)Input.mousePosition - m_offset;
        }

        // ABSTRACTION
        private bool IsSelecting() {
            if (!Input.GetMouseButton(0)) return false;

            Vector2 startPosition = m_RectTransform.anchoredPosition;
            Vector2 mousePosition = (Vector2)Input.mousePosition - m_offset;
            Vector2 mouseOffset = mousePosition - startPosition;

            m_RectTransform.pivot = GetPivot(mouseOffset);
            m_RectTransform.sizeDelta = new Vector2(Mathf.Abs(mouseOffset.x), Mathf.Abs(mouseOffset.y));
            return true;
        }

        /// <summary>
        /// Determines which quadrant the mouse is in and returns the pivot point
        /// True = Positive(+), False = Negative(-) (++ = 1), (+- = 4), (-+ = 2), (-- = 3) (xy = quadrint)
        /// (True,True = 1), (True,False = 4), (Fasle,True = 2), (False,False = 3)
        /// </summary>
        private Vector2 GetPivot(Vector2 mouseOffset) {
            bool xIsPositive = (mouseOffset.x >= 0);
            bool yIsPositive = (mouseOffset.y >= 0);

            if (xIsPositive && yIsPositive) return new Vector2(0, 0);   // In Quadrint 1
            if (xIsPositive && !yIsPositive) return new Vector2(0, 1);  // In Quadrint 4
            if (!xIsPositive && yIsPositive) return new Vector2(1, 0);  // In Quadrint 2
            if (!xIsPositive && !yIsPositive) return new Vector2(1, 1); // In Quadrint 3

            return new Vector2(0.5f, 0.5f); // Center
        }

        private void ResetSelection() {
            m_RectTransform.pivot = new Vector2(0.5f, 0.5f);
            m_RectTransform.sizeDelta = new Vector2(1f, 1f);
        }
    }
}