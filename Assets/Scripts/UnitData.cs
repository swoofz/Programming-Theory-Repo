using System.Collections;
using UnityEngine;

namespace WaveSurvivor {
    public abstract class UnitData : MonoBehaviour {
        // ENCAPSULATION
        public int Id { get { return id; } set { if (value >= 0) id = value; } }
        public Faction faction;
        public float health;

        private int id;
    }
}