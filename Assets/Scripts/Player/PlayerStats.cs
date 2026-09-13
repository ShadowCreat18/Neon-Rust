using UnityEngine;
using NeonRust.Core;

namespace NeonRust.Player
{
    // PlayerStats hereda de Entity (comparte salud con los enemigos, pero añade lógica de niveles)
    public class PlayerStats : Entity
    {
        [Header("Progression")]
        public int currentLevel = 1;
        public int currentXP = 0;
        public int xpToNextLevel = 100;

        protected override void Start()
        {
            base.Start(); // Inicializa salud
            Debug.Log("Volt inicializado. Nivel: " + currentLevel);
        }

        public void AddXP(int amount)
        {
            currentXP += amount;
            Debug.Log("XP Obtenida: " + amount + " | Total: " + currentXP + "/" + xpToNextLevel);

            if (currentXP >= xpToNextLevel)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            currentLevel++;
            currentXP -= xpToNextLevel; // Guarda el remanente de XP
            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f); // Escala el requerimiento para el siguiente nivel
            
            // Restaura la salud al subir de nivel
            currentHealth = maxHealth;
            
            Debug.Log("¡SUBIDA DE NIVEL! Volt es ahora nivel " + currentLevel);
            // Aquí se llamaría a la UI para ofrecer las mejoras de armas
        }

        protected override void Die()
        {
            Debug.Log("¡VOLT HA SIDO DESTRUIDO! FIN DEL JUEGO.");
            // Lógica de Game Over
        }
    }
}
