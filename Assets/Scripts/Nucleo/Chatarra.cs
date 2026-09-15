using UnityEngine;

namespace NeonRust.Nucleo
{
    public class Chatarra : MonoBehaviour
    {
        public int experiencia = 10;
        public float velocidadAtraccion = 5f;
        private Transform jugador;

        private void Start()
        {
            // Opcional: auto-destrucción en 15 segundos si no se recoge
            Destroy(gameObject, 15f);
        }

        private void Update()
        {
            if (jugador != null)
            {
                // Atraer la chatarra al jugador si está cerca
                transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidadAtraccion * Time.deltaTime);
                
                if (Vector3.Distance(transform.position, jugador.position) < 0.5f)
                {
                    // Dar exp y destruir
                    var controlador = jugador.GetComponent<NeonRust.Jugador.ControladorJugador>();
                    if (controlador != null)
                    {
                        controlador.GanarExperiencia(experiencia);
                    }
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                jugador = other.transform;
            }
        }
    }
}
