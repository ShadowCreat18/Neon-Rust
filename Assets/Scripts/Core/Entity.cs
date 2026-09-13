using UnityEngine;

namespace NeonRust.Core
{
    // Clase base abstracta que demuestra Programación Orientada a Objetos
    public abstract class Entity : MonoBehaviour
    {
        [Header("Entity Stats")]
        public float maxHealth = 100f;
        protected float currentHealth;

        protected virtual void Start()
        {
            currentHealth = maxHealth;
        }

        // Método polimórfico para recibir daño
        public virtual void TakeDamage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        // Método abstracto que obliga a las clases hijas a definir cómo mueren
        protected abstract void Die();
    }
}
