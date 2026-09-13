using UnityEngine;

namespace NeonRust.Nucleo
{
    // Clase base abstracta que demuestra Programación Orientada a Objetos
    public abstract class Entidad : MonoBehaviour
    {
        [Header("Estadísticas de Entidad")]
        public float saludMaxima = 100f;
        protected float saludActual;

        protected virtual void Start()
        {
            saludActual = saludMaxima;
        }

        // Método polimórfico para recibir daño
        public virtual void RecibirDano(float cantidad)
        {
            saludActual -= cantidad;
            if (saludActual <= 0)
            {
                Morir();
            }
        }

        // Método abstracto que obliga a las clases hijas a definir cómo mueren
        protected abstract void Morir();
    }
}
