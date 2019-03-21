using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField]
        private ObjectPooling pool;

        private List<GameObject> objects = new List<GameObject>();

        public GameObject Spawn(Vector3 position)
        {
            var obj = pool.GetObject();
            obj.transform.position = position;
            objects.Add(obj);

            var reuser = obj.GetComponent<ParticleReuser>();
            reuser?.Initalize();

            return obj;
        }

        public void Free()
        {
            foreach (var obj in objects)
            {
                obj.SetActive(false);
            }
        }
    }
}
