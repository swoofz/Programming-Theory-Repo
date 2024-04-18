using System.Collections;
using UnityEngine;

namespace WaveSurvivor {
    public abstract class Building : UnitData {

        private void Update() {
            ManageBuilding();
        }

        public virtual void ManageBuilding() {
            if (health <= 0) Destroy(gameObject);
        }

    }
}