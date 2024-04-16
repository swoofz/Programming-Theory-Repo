using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace WaveSurvivor {
    public class EnemyController : MonoBehaviour {

        private Unit unit;
        private Vector3 direction;

        private void Awake() {
            unit = GetComponent<Unit>();
            GameObject _base = GameObject.Find("Base");
            unit.targets.Add(-1, _base.GetComponent<UnitData>());
            direction = _base.transform.position;
        }

        // Update is called once per frame
        void Update() {
            unit.GoTo(direction);
        }

        public void SwitchTargets(UnitData _target) {
            direction = _target.transform.position;
            unit.GoTo(direction);
        }
    }
}