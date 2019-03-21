using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class ObjectPooling : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Object;
        [SerializeField]
        private int initCount;
        [SerializeField]
        private bool setParent;

        private List<GameObject> gameObjects;

        private void Start()
        {
            gameObjects = new List<GameObject>();
            for (int i = 0; i < initCount; i++)
                CreateNew(false);
        }

        private GameObject CreateNew(bool active)
        {
            var go = Instantiate(m_Object);
            if (setParent) go.transform.parent = gameObject.transform;
            go.SetActive(active);
            gameObjects.Add(go);
            return go;
        }

        public GameObject GetObject()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (!gameObjects[i].activeSelf)
                {
                    gameObjects[i].SetActive(true);
                    return gameObjects[i];
                }
            }
            return CreateNew(true);
        }

        public void ReturnObject(GameObject go)
        {
            if (gameObjects.Contains(go)) go.SetActive(false);
            else throw new System.ArgumentException("Not found the game object.");
        }
    }
}
