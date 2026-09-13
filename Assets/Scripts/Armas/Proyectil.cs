using UnityEngine;
using NeonRust.Nucleo;

namespace NeonRust.Armas
{
    public class Proyectil : MonoBehaviour
    {
        public float velocidad = 10f;
        public float tiempoVida = 3f;
        
        private float dano;
        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            Destroy(gameObject, tiempoVida); // Destruye el proyectil después de un tiempo
        }

        void FixedUpdate()
        {
            // Mueve el proyectil hacia adelante basado en su rotación
            rb.MovePosition(rb.position + (Vector2)transform.up * velocidad * Time.fixedDeltaTime);
        }

        public void EstablecerDano(float cantidad)
        {
            dano = cantidad;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Verifica si golpeó una entidad
            Entidad entidad = collision.GetComponent<Entidad>();
            if (entidad != null)
            {
                // Si es un jugador golpeando un enemigo, o viceversa (dependiendo de la configuración de capas)
                entidad.RecibirDano(dano);
            }
            
            // Destruye el proyectil al impactar
            Destroy(gameObject);
        }
    }
}
