using UnityEngine;

namespace NeonRust.Objetos
{
    public class ChatarraNeon : MonoBehaviour
    {
        public int cantidadXP = 10;
        public float radioRecoleccion = 1.5f;
        
        private Transform transformJugador;
        public float velocidadMagnetica = 5f;
        private bool estaMagnetizado = false;

        void Start()
        {
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            if (jugador != null)
            {
                transformJugador = jugador.transform;
            }
        }

        void Update()
        {
            // Efecto magnético: la chatarra vuela hacia el jugador si está cerca
            if (transformJugador != null)
            {
                float distancia = Vector2.Distance(transform.position, transformJugador.position);
                if (distancia <= radioRecoleccion)
                {
                    estaMagnetizado = true;
                }

                if (estaMagnetizado)
                {
                    transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidadMagnetica * Time.deltaTime);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Verifica si quien recogió la chatarra fue el jugador
            if (collision.CompareTag("Player"))
            {
                Jugador.EstadisticasJugador estadisticas = collision.GetComponent<Jugador.EstadisticasJugador>();
                if (estadisticas != null)
                {
                    estadisticas.AgregarExperiencia(cantidadXP);
                }
                
                Destroy(gameObject);
            }
        }
    }
}
