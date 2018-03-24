using UnityEngine;

namespace Vectrosity.Demos.Scripts.Highlight
{
    public class MakeSpheres : MonoBehaviour
    {
        public float area = 4.5f;
        public int numberOfSpheres = 12;

        public GameObject spherePrefab;

        private void Start()
        {
            for (var i = 0; i < numberOfSpheres; i++)
                Instantiate(spherePrefab,
                    new Vector3(Random.Range(-area, area), Random.Range(-area, area), Random.Range(-area, area)),
                    Random.rotation);
        }
    }
}