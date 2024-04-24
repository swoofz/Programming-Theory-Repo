using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace WaveSurvivor {
    public class SpawnUnits : MonoBehaviour {

        public Faction faction;
        public Vector3 spawnLocation { get; set; }
        private GameObject indicatorPrefab;

        private void Awake() { 
            if(faction == Faction.Player)
                indicatorPrefab = transform.Find("Indicator").gameObject; 
        }

        public void SpawnUnit(GameObject unitPrefab) {
            GameObject clone = Instantiate(unitPrefab, spawnLocation, Quaternion.identity);
            clone.GetComponent<Unit>().faction = faction;

            if (faction == Faction.Enemy) { clone.AddComponent<EnemyController>(); }
            else if (faction == Faction.Player) {
                AddIndicator(clone);
                PlayerController player = FindObjectOfType<PlayerController>();
                player.AddUnit(clone.GetComponent<Unit>(), clone.transform.position);
            }
        }

        private void AddIndicator(GameObject parent) {
            GameObject clone = Instantiate(indicatorPrefab);
            clone.transform.SetParent(parent.transform, false);
            clone.name = "Indicator";
            parent.GetComponent<UnitData>().FindIndicator();
        }
    }
}