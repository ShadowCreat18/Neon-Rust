using UnityEngine;
using NeonRust.Core;

namespace NeonRust.Weapons
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 10f;
        public float lifetime = 3f;
        
        private float damage;
        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            Destroy(gameObject, lifetime); // Destruye el proyectil después de un tiempo
        }

        void FixedUpdate()
        {
            // Mueve el proyectil hacia adelante basado en su rotación
            rb.MovePosition(rb.position + (Vector2)transform.up * speed * Time.fixedDeltaTime);
        }

        public void SetDamage(float amount)
        {
            damage = amount;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Verifica si golpeó una entidad
            Entity entity = collision.GetComponent<Entity>();
            if (entity != null)
            {
                // Si es un jugador golpeando un enemigo, o viceversa (dependiendo de la configuración de capas)
                entity.TakeDamage(damage);
            }
            
            // Destruye el proyectil al impactar
            Destroy(gameObject);
        }
    }
}
