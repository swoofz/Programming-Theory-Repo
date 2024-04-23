using System.Collections;
using UnityEngine;

namespace WaveSurvivor {
    public class SpawnUnits : MonoBehaviour {

        public Faction faction;
        private GameObject indicatorPrefab;

        private void Awake() { 
            if(faction == Faction.Player)
                indicatorPrefab = transform.Find("Indicator").gameObject; 
        }

        public void SpawnUnit(GameObject unitPrefab) {
            GameObject clone = Instantiate(unitPrefab);
            clone.GetComponent<Unit>().faction = faction;

            if (faction == Faction.Enemy) clone.AddComponent<EnemyController>();
            else if (faction == Faction.Player) { 
                Instantiate(indicatorPrefab).transform.SetParent(clone.transform, false);
                PlayerController player = FindObjectOfType<PlayerController>();
                player.AddUnit(clone.GetComponent<Unit>(), clone.transform.position);
            }
        }
    }
}