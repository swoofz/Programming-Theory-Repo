using System.Collections;
using UnityEngine;

namespace WaveSurvivor {
    public enum Faction { Player, Enemy, Neutral }

    public abstract class UnitData : MonoBehaviour {
        // ENCAPSULATION
        public int id { get { return gameObject.GetInstanceID(); } }
        public Faction faction;
        public float health;
        public float offsetAttackRange;
    }
}