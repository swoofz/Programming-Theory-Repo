using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace WaveSurvivor {
    public class EnemyController : MonoBehaviour {

        [SerializeField] private GameObject target;
        private Unit unit;
        private Vector3 direction;

        private void Awake() {
            unit = GetComponent<Unit>();
            direction = Vector3.zero;
            target = GameObject.Find("Base");
            unit.targets.Add(-1, target);
        }

        // Update is called once per frame
        void Update() {
            if (!target) return;
            if (!IsMoving()) unit.GoTo(direction);
        }

        private bool IsMoving() {
            if (direction == Vector3.zero) return false;

            direction = target.transform.position;
            unit.GoTo(direction);
            return true;
        }

        public void SwitchTargets(GameObject _target) {
            target = _target;
            direction = target.transform.position;
            unit.GoTo(direction);
        }
    }
}