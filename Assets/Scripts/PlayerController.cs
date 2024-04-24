using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveSurvivor {
    public class PlayerController : MonoBehaviour {

        public class UnitActionData {
            public UnitData unit;
            public UnitData target;
            public Vector3 position;
        }

        public Camera GameCam;
        public Selection selector;
        // Delect just for testing rn
        public SpawnManager spawnManager;

        private Dictionary<int, UnitActionData> ourUnits;
        private Dictionary<int, Unit> selectedUnits;
        private UnitData selectedBuilding;

        [SerializeField] private float distanceToTarget;

        private void Awake() {
            ourUnits = new Dictionary<int, UnitActionData>();
            foreach (Unit unit in FindObjectsOfType<Unit>()) {
                if (unit.faction != Faction.Player) continue;
                AddUnit(unit, unit.transform.position);
            }
            selectedUnits = new Dictionary<int, Unit>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(spawnManager.SpawnEnemy(3, 0, 1));
        }

        private void LateUpdate() {
            bool clickedToMove = Input.GetMouseButtonDown(1) && selectedUnits.Count > 0;
            if (clickedToMove) MoveTo(selector.GetPoint(GameCam));

            MoveUnits();

            if (Input.GetMouseButtonUp(0)) HandleSelection();
            if (Input.GetKeyDown(KeyCode.Escape)) ClearSelection();
        }

        private void HandleSelection() {
            if (selector.RaysAreTheSame(GameCam)) { // CLICK
                ClearSelection();
                UnitData hitUnit = selector.GetClickedTarget(GameCam).GetComponent<UnitData>();
                if (hitUnit == null) return;

                Unit unit = hitUnit.GetComponent<Unit>();
                if(unit) selectedUnits.Add(hitUnit.id, unit);
                else selectedBuilding = hitUnit;
                hitUnit.Indicator.SetActive(true);
                return;
            }

            // DRAG
            bool foundUnit = false;
            foreach (UnitActionData unitData in ourUnits.Values) {
                if (!unitData.unit.GetComponent<Unit>()) continue;
                if (!selector.Drag(GameCam, unitData.unit.transform.position)) continue;
                if (selectedUnits.ContainsKey(unitData.unit.id)) continue;

                Unit unit = unitData.unit.GetComponent<Unit>();
                selectedUnits.Add(unitData.unit.id, unit);
                unit.Indicator.SetActive(true);
                foundUnit = true;
            }

            float dragDistance = selector.GetDragDistance();
            if (dragDistance < 5f && !foundUnit) ClearSelection();    // Basically click but showed as drag
        }


        private void MoveUnits() {
            List<int> ids = new List<int>();
            foreach (int key in ourUnits.Keys) {
                UnitActionData unitData = ourUnits[key];
                if (!unitData.unit) { ids.Add(key); continue; }

                Unit unit = unitData.unit.GetComponent<Unit>();
                if (!unit) continue;

                bool isMoving = !unit.IsInRange(unitData.position, distanceToTarget);
                if (isMoving) unit.GoTo(unitData.position);
                else {
                    unitData.position = unit.transform.position;
                    TryAttacking(unit.FindTarget(0), unit);
                }
            }

            foreach (int id in ids) {
                ourUnits.Remove(id);
                if(selectedUnits.ContainsKey(id)) selectedUnits.Remove(id);
            }
        }

        private void MoveTo(Vector3 point) {
            foreach (Unit unit in selectedUnits.Values) { ourUnits[unit.id].position = point; }
        }

        private void TryAttacking(UnitData target, Unit unit) {
            if (!target) return;

            bool targetInRange = unit.IsInRange(target);
            if (!targetInRange) { unit.GoTo(target); }
            if (targetInRange) unit.Attack(target);
        }

        private void ClearSelection() {
            foreach (Unit unit in selectedUnits.Values) unit.Indicator.SetActive(false);
            if(selectedBuilding) selectedBuilding.Indicator.SetActive(false);
            selectedBuilding = null;
            selectedUnits.Clear();
        }

        public void SpawnUnit(GameObject unitPrefab) {
            GameObject clone = Instantiate(unitPrefab);
            clone.GetComponent<Unit>().faction = Faction.Player;
        }

        public void AddUnit(Unit unit, Vector3 position) {
            UnitActionData data = new UnitActionData() { unit = unit, target = null, position = position };
            ourUnits.Add(unit.id, data);
        }
    }
}