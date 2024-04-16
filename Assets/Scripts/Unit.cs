using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace WaveSurvivor {
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : MonoBehaviour {

        // ENCAPSULATION
        public int Id { get { return id; } set { if (value >= 0) id = value; } }

        public float speed;
        public float attackPower;
        public float health;
        public float dectionRange;
        public GameObject Indicator { get { return indicator; } private set { indicator = value; } }

        private GameObject target;
        private GameObject indicator;
        private SphereCollider dectectionColider;
        private NavMeshAgent nav_Agent;

        private int id;

        private void Awake() {
            nav_Agent = GetComponent<NavMeshAgent>();
            dectectionColider = GetComponent<SphereCollider>();
            indicator = transform.Find("Indicator").gameObject;

            dectectionColider.radius = dectionRange;
        }

        // Update is called once per frame
        void Update() {

        }

        private void OnTriggerEnter(Collider other) {
            
        }

        public virtual void GoTo(Vector3 _target) {
            nav_Agent.speed = speed;
            nav_Agent.SetDestination(_target);
        }

        public virtual void Attack() { }
    }
}
