using UnityEngine;

namespace NeonRust.Entorno
{
    public class Chatarra : MonoBehaviour
    {
        public int valorExperiencia = 10;
        public float velocidadRotacion = 100f;

        private void Update()
        {
            // Hacer que la chatarra gire para verse bonita
            transform.Rotate(Vector3.up * velocidadRotacion * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("¡Volt recogió " + valorExperiencia + " de Chatarra!");
                // Aquí en el futuro sumaríamos la experiencia al jugador
                Destroy(gameObject);
            }
        }
    }
}
