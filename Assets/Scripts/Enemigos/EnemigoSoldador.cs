using UnityEngine;

namespace NeonRust.Enemigos
{
    // EnemigoSoldador hereda de EnemigoBase, el cual hereda de Entidad (Polimorfismo multinivel)
    public class EnemigoSoldador : EnemigoBase
    {
        [Header("Especificaciones del Soldador")]
        public float danoAtaque = 20f;
        public float rangoAtaque = 1.5f;

        protected override void Update()
        {
            base.Update(); // Sigue al jugador usando el código del padre
            
            if (transformJugador != null)
            {
                float distanciaAlJugador = Vector2.Distance(transform.position, transformJugador.position);
                if (distanciaAlJugador <= rangoAtaque)
                {
                    Atacar();
                }
            }
        }

        private void Atacar()
        {
            // Aquí iría la lógica de animación y daño cuerpo a cuerpo
            // Por ejemplo, buscar el script de salud del jugador y hacerle daño
            Debug.Log("¡El Robot Soldador embiste y ataca!");
        }
    }
}
