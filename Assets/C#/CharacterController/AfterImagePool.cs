using System;
using System.Collections.Generic;
using UnityEngine;


namespace C_.CharacterController
{
    public class AfterImagePool : MonoBehaviour
    {
        [SerializeField] private GameObject afterImagePrefab;
        private readonly Queue<GameObject> _availableObjects = new Queue<GameObject>();
        
        public static AfterImagePool Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            GrowPool();
        }

        private void GrowPool()
        {
            for (var i = 0; i < 10; i++)
            {
                var instanceToAdd = Instantiate(afterImagePrefab, transform, true);
                AddToPool(instanceToAdd);
            }
        }

        public void AddToPool(GameObject instanceToAdd)
        {
            instanceToAdd.SetActive(false);
            _availableObjects.Enqueue(instanceToAdd);
        }

        public GameObject GetFromPool()
        {
            Debug.Log("GetFromPool");
            if (_availableObjects.Count == 0)
            {
                GrowPool();
            }

            var instance = _availableObjects.Dequeue();
            instance.SetActive(true);
            return instance;
        }
    }
}