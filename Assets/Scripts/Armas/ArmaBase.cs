using UnityEngine;

namespace NeonRust.Armas
{
    public abstract class ArmaBase : MonoBehaviour
    {
        [Header("Estadísticas del Arma")]
        public float cadenciaDisparo = 0.5f;
        public float dano = 10f;
        public GameObject prefabProyectil;
        public Transform puntoDisparo;

        protected float tiempoSiguienteDisparo = 0f;

        public virtual void IntentarDisparar()
        {
            if (Time.time >= tiempoSiguienteDisparo)
            {
                Disparar();
                tiempoSiguienteDisparo = Time.time + cadenciaDisparo;
            }
        }

        // Método abstracto que define CÓMO dispara el arma
        protected abstract void Disparar();
    }
}
