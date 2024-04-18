using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

namespace WaveSurvivor {
    public class PlayerController : MonoBehaviour {

        public class UnitActionData {
            public Unit unit;
            public UnitData target;
            public Vector3 position;
        }

        public Camera GameCam;
        public Selection selector;

        private Dictionary<int, UnitActionData> ourUnits;
        private Dictionary<int, Unit> selectedUnits;

        [SerializeField] private float distanceToTarget;

        private void Awake() {
            ourUnits = new Dictionary<int, UnitActionData>();
            foreach (Unit unit in FindObjectsOfType<Unit>()) {
                UnitActionData unitData = CreateUnitActionData(unit, unit.transform.position);
                ourUnits.Add(unit.id, unitData);
            }
            selectedUnits = new Dictionary<int, Unit>();
        }

        void Update() {
            bool clickedToMove = Input.GetMouseButtonDown(1) && selectedUnits.Count > 0;
            if (clickedToMove) MoveTo(selector.GetPoint(GameCam));

            MoveUnits();

            if (Input.GetMouseButtonUp(0)) HandleSelection();
            if (Input.GetKeyDown(KeyCode.Escape)) ClearSelection();
        }

        private void HandleSelection() {
            if (selector.RaysAreTheSame(GameCam)) { // CLICK
                Unit hitUnit = selector.GetClickedTarget(GameCam).GetComponent<Unit>();

                if (hitUnit == null) { ClearSelection(); return; }
                if (selectedUnits.ContainsKey(hitUnit.id)) return;

                hitUnit.Indicator.SetActive(true);
                selectedUnits.Add(hitUnit.id, hitUnit);
                return;
            }

            // DRAG
            bool foundUnit = false;
            foreach (UnitActionData unitData in ourUnits.Values) {
                if (!selector.Drag(GameCam, unitData.unit.transform.position)) continue;
                if (selectedUnits.ContainsKey(unitData.unit.id)) continue;
                selectedUnits.Add(unitData.unit.id, unitData.unit);
                unitData.unit.Indicator.SetActive(true);
                foundUnit = true;
            }

            float dragDistance = selector.GetDragDistance();
            if (dragDistance < 5f && !foundUnit) ClearSelection();    // Basically click but showed as drag
        }


        private void MoveUnits() {
            foreach (int key in ourUnits.Keys) {
                UnitActionData unitData = ourUnits[key];
                if (!unitData.unit) ourUnits.Remove(key);

                float distance = Vector3.Distance(unitData.unit.transform.position, unitData.position);
                float range = distanceToTarget;
                if (unitData.target) range += unitData.unit.attackRange;

                if (distance > range) unitData.unit.GoTo(unitData.position);
                else if (unitData.target) unitData.unit.Attack(unitData.target);
            }
        }

        private void MoveTo(Vector3 point) {
            foreach (Unit unit in selectedUnits.Values) { ourUnits[unit.id].position = point; }
        }

        private void ClearSelection() {
            foreach (Unit unit in selectedUnits.Values) unit.Indicator.SetActive(false);
            selectedUnits.Clear();
        }

        private UnitActionData CreateUnitActionData(Unit unit, Vector3 position) {
            return new UnitActionData() { unit = unit, target = null, position = position };
        }
    }
}