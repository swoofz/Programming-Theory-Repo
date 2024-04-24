using System.Collections;
using UnityEngine;

namespace WaveSurvivor {
    public enum Faction { Player, Enemy, Neutral }

    public abstract class UnitData : MonoBehaviour {
        // ENCAPSULATION
        public GameObject Indicator { get { return indicator; } private set { indicator = value; } }
        public int id { get { return gameObject.GetInstanceID(); } }
        public Faction faction;
        public float health;
        public float offsetAttackRange;

        private GameObject indicator;

        private void Awake() { OurAwake(); }

        protected virtual void OurAwake() { }

        public void FindIndicator() {
            if (faction == Faction.Player)
                indicator = transform.Find("Indicator").gameObject;
        }
    }
}