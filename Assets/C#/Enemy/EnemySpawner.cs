using C_.CharacterController;
using UnityEngine;
using UnityEngine.Serialization;

namespace C_.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab1; 
        public GameObject enemyPrefab2; 
        public float spawnRate; 
        public int offset = 1;
        

        private void Start()
        {
            InvokeRepeating(nameof(SpawnEnemy), 0.1f, spawnRate);
        }

        private void SpawnEnemy()
        {

            for (int i = 0; i < TopDownMovement.Instance._level; i++)
            {
                // spawn limit
            
                Vector3 spawnPosition = GetSpawnPositionOutsideScreen();
            
                GameObject enemyPrefab = GetRandomEnemyPrefab();
            
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
            
            
        }

        private GameObject GetRandomEnemyPrefab()
        {
            // Random
            return Random.value > 0.5f ? enemyPrefab1 : enemyPrefab2;
        }

        private Vector3 GetSpawnPositionOutsideScreen()
        {
            //screen view
            Vector3 screenPosition = Camera.main.ViewportToWorldPoint(new Vector3(1.5f, 1.5f, 0f));
            Vector3 spawnPosition;

            // Random
            int side = Random.Range(0, 4);
            
            switch (side)
            {
                case 0: // Top
                    spawnPosition = new Vector3(Random.Range(-screenPosition.x / 2, screenPosition.x / 2) + offset, screenPosition.y / 2 + offset, 0f);
                    break;
                case 1: // Bottom
                    spawnPosition = new Vector3(Random.Range(-screenPosition.x / 2, screenPosition.x / 2) + offset, -screenPosition.y / 2 + offset, 0f);
                    break;
                case 2: // Left
                    spawnPosition = new Vector3(-screenPosition.x / 2 + offset, Random.Range(-screenPosition.y / 2, screenPosition.y / 2) + offset, 0f);
                    break;
                case 3: // Right
                    spawnPosition = new Vector3(screenPosition.x / 2 + offset, Random.Range(-screenPosition.y / 2, screenPosition.y / 2) + offset, 0f);
                    break;
                default:
                    spawnPosition = Vector3.zero;
                    break;
            }

            return spawnPosition;
        }
    }
}
