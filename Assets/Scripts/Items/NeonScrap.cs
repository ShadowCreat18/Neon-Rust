using UnityEngine;

namespace NeonRust.Items
{
    public class NeonScrap : MonoBehaviour
    {
        public int xpAmount = 10;
        public float pickupRadius = 1.5f;
        
        private Transform playerTransform;
        public float magnetSpeed = 5f;
        private bool isMagnetized = false;

        void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        void Update()
        {
            // Efecto magnético: la chatarra vuela hacia el jugador si está cerca
            if (playerTransform != null)
            {
                float distance = Vector2.Distance(transform.position, playerTransform.position);
                if (distance <= pickupRadius)
                {
                    isMagnetized = true;
                }

                if (isMagnetized)
                {
                    transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, magnetSpeed * Time.deltaTime);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Verifica si quien recogió la chatarra fue el jugador
            if (collision.CompareTag("Player"))
            {
                Player.PlayerStats stats = collision.GetComponent<Player.PlayerStats>();
                if (stats != null)
                {
                    stats.AddXP(xpAmount);
                }
                
                Destroy(gameObject);
            }
        }
    }
}
