using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveSurvivor {
    public class SpawnManager : MonoBehaviour {

        public List<Unit> unitPrefabs;

        private SpawnUnits enemySpawner;
        private SpawnUnits playerSpawner;
        [SerializeField] private float xBounds = 20f;
        [SerializeField] private float zBounds = 20f;

        private void Awake() {
            enemySpawner = transform.Find("EnemyUnitSpawner").GetComponent<SpawnUnits>();
            playerSpawner = transform.Find("PlayerUnitSpawner").GetComponent<SpawnUnits>();

            GameObject playerBase = GameObject.Find("Base");
            playerSpawner.spawnLocation = playerBase.transform.Find("SpawnLocation").position;
        }


        public IEnumerator SpawnEnemy(int enemyCount, float minSecond, float maxSecond) {
            for (int i = 0; i < enemyCount; i++) {
                float seconds = Random.Range(minSecond, maxSecond);
                SpawnRandomUnit();
                yield return new WaitForSeconds(seconds);
            }

            StopCoroutine("SpawnEnemy");
        }

        private void SpawnRandomUnit() {
            int u_index = Random.Range(0, unitPrefabs.Count);
            enemySpawner.spawnLocation = GetRandomSpawnLocation();
            enemySpawner.SpawnUnit(unitPrefabs[u_index].gameObject);
        }

        private Vector3 GetRandomSpawnLocation() {
            string direction = GetSpawnDirection();

            float x = 0, z = 0;
            switch (direction) {
                case "North":
                    x = Random.Range(-xBounds, xBounds);
                    z = zBounds;
                    break;
                case "East":
                    x = xBounds;
                    z = Random.Range(-zBounds, zBounds);
                    break;
                case "South":
                    x = Random.Range(-xBounds, xBounds);
                    z = -zBounds;
                    break;
                case "West":
                    x = -xBounds;
                    z = Random.Range(-zBounds, zBounds);
                    break;
                default:
                    Debug.Log("Direction not valid.");
                    break;
            }

            return new Vector3(x, 0.5f, z);
        }

        private string GetSpawnDirection() {
            float ran = Random.Range(0f, 1f);
            if (ran < .25f) return "North";
            if (ran < .5f) return "East";
            if (ran < .75f) return "South";
            return "West";
        }
    }
}