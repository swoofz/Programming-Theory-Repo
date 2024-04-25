using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace WaveSurvivor {
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : UnitData {     // INHERITANCE

        public Dictionary<int, UnitData> targets = new Dictionary<int, UnitData>();

        public float speed;
        public float attackPower;
        public float dectionRange;
        public float attackRange;

        private SphereCollider dectectionColider;
        private NavMeshAgent nav_Agent;

        // POLYMORPHISM
        protected override void OurAwake() {
            nav_Agent = GetComponent<NavMeshAgent>();
            dectectionColider = GetComponent<SphereCollider>();
            dectectionColider.radius = dectionRange;
            base.OurAwake();
        }

        private void Update() { if (health <= 0) Destroy(gameObject); }

        private void OnTriggerEnter(Collider other) {
            UnitData otherUnit = other.gameObject.GetComponent<UnitData>();
            if (!otherUnit) return;
            if (otherUnit.faction == Faction.Neutral) return;
            if (targets.ContainsKey(otherUnit.id) || faction == otherUnit.faction) return;
            AddTarget(otherUnit);
        }

        private void OnTriggerExit(Collider other) {
            if (faction != Faction.Player) return;
            UnitData otherUnit = other.gameObject.GetComponent<UnitData>();
            if (!otherUnit) return;
            if (faction == otherUnit.faction) return;
            targets.Remove(otherUnit.id);
        }


        // POLYMORPHISM
        public virtual void GoTo(Vector3 target) {
            transform.LookAt(target);
            nav_Agent.speed = speed;
            nav_Agent.SetDestination(target);
        }

        public virtual void GoTo(UnitData unit) {
            transform.LookAt(unit.transform.position);
            nav_Agent.speed = speed;
            nav_Agent.SetDestination(unit.transform.position);
        }

        public virtual void Attack(UnitData target) {
            target.health -= attackPower;
        }

        public bool IsInRange(Vector3 target, float offset) {
            float distance = Vector3.Distance(transform.position, target);
            if (distance < offset) return true;

            return false;
        }

        public bool IsInRange(UnitData target) {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < offsetAttackRange + attackRange) return true;

            return false;
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
