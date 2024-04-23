using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveSurvivor {
    public class EnemyController : MonoBehaviour {

        private Unit unit;
        [SerializeField] private UnitData target;
        [SerializeField] private string playerBase = "Base";

        private void Awake() {
            unit = GetComponent<Unit>();
            GameObject _base = GameObject.Find(playerBase);
            unit.AddTarget(_base.GetComponent<UnitData>());
        }

        // Update is called once per frame
        private void Update() {
            target = unit.FindTarget(1);
            if (!target) return;

            float distanceFrom = Vector3.Distance(transform.position, target.transform.position);
            if (distanceFrom < unit.attackRange + target.offsetAttackRange) unit.Attack(target); 
            else unit.GoTo(target);
        }

        private void OnTriggerExit(Collider other) {
            UnitData otherUnit = other.gameObject.GetComponent<UnitData>();
            if (!otherUnit) return;
            if (unit.faction == otherUnit.faction || otherUnit.name == playerBase) return;
            unit.targets.Remove(otherUnit.id);
        }
    }
}