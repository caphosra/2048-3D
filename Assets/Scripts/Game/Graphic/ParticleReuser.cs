using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleReuser : MonoBehaviour
    {
        private ParticleSystem particle;

        private float finishTime;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void Initalize()
        {
            if (particle == null)
            {
                particle = GetComponent<ParticleSystem>();
            }

            finishTime = particle.main.duration + particle.main.startLifetime.constantMax;
            particle.Play();

            StartCoroutine(WaitForFinishCorutine());
        }

        IEnumerator WaitForFinishCorutine()
        {
            yield return new WaitForSeconds(finishTime);
            gameObject.SetActive(false);
        }
    }
}
