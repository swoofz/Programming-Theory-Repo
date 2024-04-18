using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace WaveSurvivor {
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : UnitData {     // INHERITANCE

        // ENCAPSULATION
        public GameObject Indicator { get { return indicator; } private set { indicator = value; } }
        public int LastTarget { get { return lastTargetId; } }
        public Dictionary<int, UnitData> targets = new Dictionary<int, UnitData>();

        public float speed;
        public float attackPower;
        public float dectionRange;
        public float attackRange;

        private int lastTargetId = -1;
        private GameObject indicator;
        private SphereCollider dectectionColider;
        private NavMeshAgent nav_Agent;

        private void Awake() {
            nav_Agent = GetComponent<NavMeshAgent>();
            dectectionColider = GetComponent<SphereCollider>();

            if(gameObject.tag == "Ally")
                indicator = transform.Find("Indicator").gameObject;

            dectectionColider.radius = dectionRange;
        }

        private void Update() { if (health <= 0) Destroy(gameObject); }

        private void OnTriggerEnter(Collider other) {
            UnitData otherUnit = other.gameObject.GetComponent<UnitData>();
            if (!otherUnit) return;
            if (targets.ContainsKey(otherUnit.id) || faction == otherUnit.faction) return;
            AddTarget(otherUnit);
        }


        public virtual void GoTo(Vector3 _target) {
            transform.LookAt(_target);
            Vector3 direction = _target - transform.position;
            transform.position += (direction.normalized * speed * Time.deltaTime);
        }

        public virtual void GoTo(UnitData unit) {
            lastTargetId = unit.id;
            transform.LookAt(unit.transform);
            Vector3 direction = unit.transform.position - transform.position;
            transform.position += (direction.normalized * speed * Time.deltaTime);
        }

        public virtual void Attack(UnitData target) {
            target.health -= attackPower;
        }

        public void AddTarget(UnitData unit) {
            targets.Add(unit.id, unit);
        }

        public UnitData FindTarget(int index) {
            if (targets.Count == 0) return null;
            if (targets.Count <= index) index--;

            UnitData target = targets.ElementAt(index).Value;
            if (target) return target;
            targets.Remove(targets.ElementAt(index).Key);
            return FindTarget(index);
        }
    }
}
