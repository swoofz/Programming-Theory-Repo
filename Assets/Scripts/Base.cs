using System.Collections;
using UnityEngine;

namespace WaveSurvivor {
    public class Base : Building {
        private void Awake() {
            Id = -5;
        }

        private void Update() {
            if(health <= 0) Destroy(gameObject);
        }

    }
}