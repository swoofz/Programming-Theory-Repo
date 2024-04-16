using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace WaveSurvivor {
    public enum Faction { Player, Enemy, Neutral }

    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : MonoBehaviour {
        public Faction faction;

        // ENCAPSULATION
        public int Id { get { return id; } set { if (value >= 0) id = value; } }

        public float speed;
        public float attackPower;
        public float health;
        public float dectionRange;
        public float attackRange;
        public GameObject Indicator { get { return indicator; } private set { indicator = value; } }
        public Dictionary<int, GameObject> targets = new Dictionary<int, GameObject>();

        private GameObject target;
        private GameObject indicator;
        private SphereCollider dectectionColider;
        private NavMeshAgent nav_Agent;

        private int id;

        private void Awake() {
            nav_Agent = GetComponent<NavMeshAgent>();
            dectectionColider = GetComponent<SphereCollider>();

            if(gameObject.tag == "Ally")
                indicator = transform.Find("Indicator").gameObject;

            dectectionColider.radius = dectionRange;
        }

        private void Update() {
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
                gameObject.GetComponent<EnemyController>().SwitchTargets(targets.ElementAt(index).Value);
                target = targets.ElementAt(1).Value;
            }
        }

        public virtual void GoTo(Vector3 _target) {
            nav_Agent.speed = speed;
            nav_Agent.SetDestination(_target);
        }

        public virtual void Attack() {
            Debug.Log("Attacking");
        }
    }
}
