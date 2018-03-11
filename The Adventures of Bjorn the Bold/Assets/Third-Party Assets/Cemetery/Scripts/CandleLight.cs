using UnityEngine;

namespace TropicBlocks
{
    [RequireComponent(typeof(Light))]
    public class CandleLight : MonoBehaviour
    {
        public float minIntensity = 0.4f;

        public float maxIntensity = 0.8f; 

        public float speed = 1f;

        private float random = 1f;

        void Start()
        {
            random = Random.Range(0f, 10f);
        }

        void Update()
        {
            float noiseValue = Mathf.PerlinNoise(random, Time.time * speed);
            GetComponent<Light>().intensity = Mathf.Lerp(minIntensity, maxIntensity, noiseValue);
        }
    }
}