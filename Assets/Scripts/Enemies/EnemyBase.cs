using UnityEngine;
using NeonRust.Core;

namespace NeonRust.Enemies
{
    // EnemyBase hereda de Entity
    public class EnemyBase : Entity
    {
        [Header("Enemy Rewards")]
        public GameObject neonScrapPrefab;
        public int xpValue = 10;
        
        [Header("Enemy AI")]
        public float speed = 2f;
        protected Transform playerTransform;

        protected override void Start()
        {
            base.Start(); // Llama al Start() de Entity para inicializar la vida
            // Busca al jugador en la escena (usando la etiqueta Player)
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        protected virtual void Update()
        {
            // Comportamiento base: Moverse hacia el jugador
            if (playerTransform != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            }
        }

        // Implementación del método abstracto Die()
        protected override void Die()
        {
            // Soltar chatarra (XP)
            if (neonScrapPrefab != null)
            {
                Instantiate(neonScrapPrefab, transform.position, Quaternion.identity);
            }
            
            // Destruir este objeto
            Destroy(gameObject);
        }
    }
}
