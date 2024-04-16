using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace WaveSurvivor {
    public class EnemyController : MonoBehaviour {

        private Unit unit;
        private Vector3 direction;

        private void Awake() {
            unit = GetComponent<Unit>();
            unit.targets.Add(-1, GameObject.Find("Base"));
            direction = GameObject.Find("Base").transform.position;
        }

        // Update is called once per frame
        void Update() {
            unit.GoTo(direction);
        }

        public void SwitchTargets(GameObject _target) {
            direction = _target.transform.position;
            unit.GoTo(direction);
        }
    }
}