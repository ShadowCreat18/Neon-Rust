using UnityEngine;
using NeonRust.Nucleo;

namespace NeonRust.Jugador
{
    // EstadisticasJugador hereda de Entidad (comparte salud con los enemigos, pero añade lógica de niveles)
    public class EstadisticasJugador : Entidad
    {
        [Header("Progresión")]
        public int nivelActual = 1;
        public int experienciaActual = 0;
        public int experienciaParaSiguienteNivel = 100;

        protected override void Start()
        {
            base.Start(); // Inicializa salud
            Debug.Log("Volt inicializado. Nivel: " + nivelActual);
        }

        public void AgregarExperiencia(int cantidad)
        {
            experienciaActual += cantidad;
            Debug.Log("XP Obtenida: " + cantidad + " | Total: " + experienciaActual + "/" + experienciaParaSiguienteNivel);

            if (experienciaActual >= experienciaParaSiguienteNivel)
            {
                SubirNivel();
            }
        }

        private void SubirNivel()
        {
            nivelActual++;
            experienciaActual -= experienciaParaSiguienteNivel; // Guarda el remanente de XP
            experienciaParaSiguienteNivel = Mathf.RoundToInt(experienciaParaSiguienteNivel * 1.5f); // Escala el requerimiento para el siguiente nivel
            
            // Restaura la salud al subir de nivel
            saludActual = saludMaxima;
            
            Debug.Log("¡SUBIDA DE NIVEL! Volt es ahora nivel " + nivelActual);
            // Aquí se llamaría a la UI para ofrecer las mejoras de armas
        }

        protected override void Morir()
        {
            Debug.Log("¡VOLT HA SIDO DESTRUIDO! FIN DEL JUEGO.");
            // Lógica de Game Over
        }
    }
}
