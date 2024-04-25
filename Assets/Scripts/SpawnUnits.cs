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
                Vector3 position = SetGoToOnSpawn(clone.transform.position, 5);
                player.AddUnit(clone.GetComponent<Unit>(), position);
            }
        }

        private void AddIndicator(GameObject parent) {
            GameObject clone = Instantiate(indicatorPrefab);
            clone.transform.SetParent(parent.transform, false);
            clone.name = "Indicator";
            parent.GetComponent<UnitData>().FindIndicator();
        }

        private Vector3 SetGoToOnSpawn(Vector3 position, float offset) {
            float x = position.x + Random.Range(-offset, offset);
            float z = position.z - offset;
            return new Vector3(x, position.y, z);
        }
    }
}