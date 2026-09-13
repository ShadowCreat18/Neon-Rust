using UnityEngine;
using NeonRust.Nucleo;

namespace NeonRust.Enemigos
{
    // EnemigoBase hereda de Entidad
    public class EnemigoBase : Entidad
    {
        [Header("Recompensas del Enemigo")]
        public GameObject prefabChatarraNeon;
        public int valorXP = 10;
        
        [Header("IA del Enemigo")]
        public float velocidad = 2f;
        protected Transform transformJugador;

        protected override void Start()
        {
            base.Start(); // Llama al Start() de Entidad para inicializar la vida
            // Busca al jugador en la escena (usando la etiqueta Player)
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            if (jugador != null)
            {
                transformJugador = jugador.transform;
            }
        }

        protected virtual void Update()
        {
            // Comportamiento base: Moverse hacia el jugador
            if (transformJugador != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, transformJugador.position, velocidad * Time.deltaTime);
            }
        }

        // Implementación del método abstracto Morir()
        protected override void Morir()
        {
            // Soltar chatarra (XP)
            if (prefabChatarraNeon != null)
            {
                Instantiate(prefabChatarraNeon, transform.position, Quaternion.identity);
            }
            
            // Destruir este objeto
            Destroy(gameObject);
        }
    }
}
