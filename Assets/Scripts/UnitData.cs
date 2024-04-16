using System.Collections;
using UnityEngine;

namespace WaveSurvivor {
    public enum Faction { Player, Enemy, Neutral }

    public abstract class UnitData : MonoBehaviour {
        // ENCAPSULATION
        public int Id { get { return id; } set { if (value >= 0) id = value; } }
        public Faction faction;
        public float health;
        public float offsetAttackRange;

        private int id;
    }
}