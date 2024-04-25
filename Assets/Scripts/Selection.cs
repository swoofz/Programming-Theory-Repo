using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WaveSurvivor {
    public class Selection : MonoBehaviour {

        // View Varibles
        private RectTransform m_RectTransform;
        private Vector2 m_offset;

        // Game Selection Varibles
        private RaycastHit startRay;
        private RaycastHit endRay;
        private Vector3 initalPosition;

        private void Awake() {
            m_RectTransform = GetComponent<RectTransform>();
            m_offset = new Vector2(Screen.width, Screen.height) / 2;

            ResetSelection();
        }

        
        void Update() {
            if(EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject) return;
            if (IsSelecting()) return;

            if (Input.GetMouseButtonUp(0)) ResetSelection();
            m_RectTransform.anchoredPosition = (Vector2)Input.mousePosition - m_offset;
        }


        // ABSTRACTION
        private bool IsSelecting() {
            if (!Input.GetMouseButton(0)) return false;
            if (Input.GetMouseButtonDown(0)) initalPosition = Input.mousePosition;

            Vector2 startPosition = m_RectTransform.anchoredPosition;
            Vector2 mousePosition = (Vector2)Input.mousePosition - m_offset;
            Vector2 mouseOffset = mousePosition - startPosition;

            m_RectTransform.gameObject.GetComponent<Image>().enabled = true;
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
            m_RectTransform.gameObject.GetComponent<Image>().enabled = false;
        }

        private bool IsInsideBoundary(float start, float end, float obj) {
            if ((start > obj && obj > end) || (start < obj && obj < end)) 
                return true;

            return false;
        }

        private RaycastHit GetRay(Camera camera, Vector3 position) {
            Ray ray = camera.ScreenPointToRay(position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) { return hit; }
            return hit;
        }

        public bool Drag(Camera cam, Vector3 objPosition) {
            startRay = GetRay(cam, initalPosition);
            endRay = GetRay(cam, Input.mousePosition);

            bool xInside = IsInsideBoundary(startRay.point.x, endRay.point.x, objPosition.x);
            bool yInside = IsInsideBoundary(startRay.point.z, endRay.point.z, objPosition.z);

            return xInside && yInside;
        }

        public bool RaysAreTheSame(Camera cam) { return GetRay(cam, initalPosition).point == GetRay(cam, Input.mousePosition).point; }
        public GameObject GetClickedTarget(Camera cam) { return GetRay(cam, Input.mousePosition).collider.gameObject; }
        public float GetDragDistance() { return Vector3.Distance(startRay.point, endRay.point); }
        public Vector3 GetPoint(Camera cam) { return GetRay(cam, Input.mousePosition).point; }
    }
}