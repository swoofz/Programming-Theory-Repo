using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace WaveSurvivor {
    public class PlayerController : MonoBehaviour {

        public Camera GameCam;
        public RectTransform selector;

        private Dictionary<int, Unit> ourUnits;
        private Dictionary<int, Unit> selectedUnits;

        private RaycastHit selectionStartRay;
        private RaycastHit selectionEndRay;

        private void Awake() {
            ourUnits = new Dictionary<int, Unit>();
            int index = 0;
            foreach (Unit unit in FindObjectsOfType<Unit>()) { 
                unit.Id = index;
                ourUnits.Add(unit.Id, unit);
                index++;
            }
            selectedUnits = new Dictionary<int, Unit>();
            ResetSelectionPosition();
        }

        void Update() {
            Vector2 intialPosition = Vector3.zero;
            if (Input.GetMouseButtonDown(0)) { 
                selectionStartRay = RayFromMouse();
                intialPosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0)) { selectionEndRay = RayFromMouse(); }

            HandleSelection();

            if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0) {
                Ray ray = GameCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) {
                    foreach (Unit unit in selectedUnits.Values) {
                        unit.GoTo(hit.point);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape)) ClearSelection();
        }


        private RaycastHit RayFromMouse() {
            Ray ray = GameCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) { return hit; }
            return hit;
        }

        private void DragSelect() {
            if (SelectionIsReset()) return;

            bool foundUnit = false;
            foreach (Unit unit in ourUnits.Values) {
                Vector3 unitPosition = unit.transform.position;
                Vector3 startPosition = selectionStartRay.point;
                Vector3 endPosition = selectionEndRay.point;

                if (IsInsideBoundary(startPosition.x, endPosition.x, unitPosition.x) &&
                    IsInsideBoundary(startPosition.z, endPosition.z, unitPosition.z)) {

                    if (selectedUnits.ContainsKey(unit.Id)) continue;
                    selectedUnits.Add(unit.Id, unit);
                    unit.Indicator.SetActive(true);
                    foundUnit = true;
                }
            }

            float dragDistance = Vector3.Distance(selectionStartRay.point, selectionEndRay.point);
            if (dragDistance < 5f && !foundUnit) ClearSelection();    // Basically click but showed as drag

            ResetSelectionPosition();
        }

        private void ClickSelect() {
            if (SelectionIsReset()) return;
            Unit hitUnit = selectionEndRay.collider.GetComponent<Unit>();

            if (hitUnit != null) {
                if (selectedUnits.ContainsKey(hitUnit.Id)) return;
                hitUnit.Indicator.SetActive(true);
                selectedUnits.Add(hitUnit.Id, hitUnit);
            } else ClearSelection();

            ResetSelectionPosition();
        }

        private void ClearSelection() {
            foreach (Unit unit in selectedUnits.Values) unit.Indicator.SetActive(false);
            selectedUnits.Clear();
        }

        private void HandleSelection() {
            if (selectionStartRay.point == selectionEndRay.point) {
                ClickSelect();
                return;
            }

            DragSelect();
        }

        private bool IsInsideBoundary(float start, float end, float middle) {
            if (
                (start > middle && middle > end) ||
                (start < middle && middle < end)
                ) return true;

            return false;
        }

        private void ResetSelectionPosition() {
            selectionStartRay = new RaycastHit() { point = Vector3.down };
            selectionEndRay = new RaycastHit() { point = Vector3.down };
        }

        private bool SelectionIsReset() {
            return (selectionEndRay.point == Vector3.down ||
                selectionStartRay.point == Vector3.down);
        }
    }
}