using UnityEngine;
using NeonRust.Nucleo;

namespace NeonRust.Armas
{
    public class Proyectil : MonoBehaviour
    {
        public float velocidad = 25f;
        public float tiempoVida = 3f;
        private float dano;
        private string tagIgnorar;

        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            
            rb.useGravity = false;
            // Bloqueamos posición y rotación en Z para que la bala no se desvíe en 3D
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            Vector3 direccionPlana = new Vector3(transform.forward.x, transform.forward.y, 0).normalized;
            rb.linearVelocity = direccionPlana * velocidad;

            Destroy(gameObject, tiempoVida);
        }

        public void Configurar(float cantidadDano, string ignorarTag)
        {
            dano = cantidadDano;
            tagIgnorar = ignorarTag;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Ignorar choques con quien disparó (ej. "Player" o "Enemy")
            if (other.CompareTag(tagIgnorar)) return;
            // Ignorar otros proyectiles
            if (other.GetComponent<Proyectil>() != null) return;

            Entidad entidad = other.GetComponent<Entidad>();
            if (entidad != null)
            {
                entidad.RecibirDano(dano);
            }
            
            // Destruir proyectil al chocar
            Destroy(gameObject);
        }
    }
}
