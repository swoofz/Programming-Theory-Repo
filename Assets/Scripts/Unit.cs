using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace WaveSurvivor {
    public enum Faction { Player, Enemy, Neutral }

    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : UnitData {     // INHERITANCE
        public GameObject Indicator { get { return indicator; } private set { indicator = value; } }
        public Dictionary<int, GameObject> targets = new Dictionary<int, GameObject>();

        public float speed;
        public float attackPower;
        public float dectionRange;
        public float attackRange;

        private GameObject target;
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

        private void Update() {
            if (health <= 0) Destroy(gameObject);
            if (!target) return;
            float distanceFrom = Vector3.Distance(transform.position, target.transform.position);
            if (distanceFrom < attackRange) Attack();
        }


        private void OnTriggerEnter(Collider other) {
            //Debug.Log(other.gameObject.name);

            Unit OtherUnit = other.gameObject.GetComponent<Unit>();
            if (!OtherUnit) return;
            if (faction == Faction.Enemy && OtherUnit.faction == Faction.Player) {
                if (targets.ContainsKey(OtherUnit.Id)) return;
                targets.Add(OtherUnit.Id, other.gameObject);
                gameObject.GetComponent<EnemyController>().SwitchTargets(targets.ElementAt(1).Value);
                target = targets.ElementAt(1).Value;
            }
        }

        private void OnTriggerExit(Collider other) {
            //Debug.Log(other.gameObject.name);

            Unit OtherUnit = other.gameObject.GetComponent<Unit>();
            if (!OtherUnit) return;
            if (faction == Faction.Enemy && OtherUnit.faction == Faction.Player) {
                targets.Remove(OtherUnit.Id);
                int index = 0;
                if (targets.Count > 1) index = 1;
                GetComponent<EnemyController>().SwitchTargets(targets.ElementAt(index).Value);
                target = targets.ElementAt(index).Value;
            }
        }

        public virtual void GoTo(Vector3 _target) {
            nav_Agent.speed = speed;
            nav_Agent.SetDestination(_target);
        }

        public virtual void Attack() {
            UnitData ourTarget = target.GetComponent<UnitData>();
            ourTarget.health -= attackPower;
            if (ourTarget.health <= 0) {
                targets.Remove(ourTarget.Id);
                int index = 0;
                if (targets.Count > 1) index = 1;
                GetComponent<EnemyController>().SwitchTargets(targets.ElementAt(index).Value);
                target = targets.ElementAt(index).Value;
            }
        }
    }
}
