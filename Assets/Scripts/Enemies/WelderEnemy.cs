using UnityEngine;

namespace NeonRust.Enemies
{
    // WelderEnemy hereda de EnemyBase, el cual hereda de Entity (Polimorfismo multinivel)
    public class WelderEnemy : EnemyBase
    {
        [Header("Welder Specifics")]
        public float attackDamage = 20f;
        public float attackRange = 1.5f;

        protected override void Update()
        {
            base.Update(); // Sigue al jugador usando el código del padre
            
            if (playerTransform != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
                if (distanceToPlayer <= attackRange)
                {
                    Attack();
                }
            }
        }

        private void Attack()
        {
            // Aquí iría la lógica de animación y daño cuerpo a cuerpo
            // Por ejemplo, buscar el script de salud del jugador y hacerle daño
            Debug.Log("El Robot Soldador embiste y ataca!");
        }
    }
}
