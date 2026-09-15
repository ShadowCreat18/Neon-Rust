using UnityEngine;

namespace NeonRust.Armas
{
    public class SistemaDisparo : MonoBehaviour
    {
        [Header("Configuración")]
        public GameObject prefabProyectil;
        public Transform puntoDisparo;
        public float cadencia = 0.5f;
        public float dano = 25f;
        
        [Tooltip("Etiqueta de quien NO debe recibir daño por esta bala (ej. Player o Enemy)")]
        public string tagAliado = "Player";

        private float tiempoUltimoDisparo = 0f;

        public void IntentarDisparar()
        {
            if (Time.time >= tiempoUltimoDisparo + cadencia)
            {
                Disparar();
                tiempoUltimoDisparo = Time.time;
            }
        }

        private void Disparar()
        {
            if (prefabProyectil == null) return;

            // Usamos la posición del punto de disparo, pero la rotación del cuerpo principal (para que no salga torcida)
            Vector3 origen = puntoDisparo != null ? puntoDisparo.position : transform.position + Vector3.up;
            Quaternion rotacion = transform.rotation; 

            GameObject bala = Instantiate(prefabProyectil, origen, rotacion);
            
            Proyectil scriptBala = bala.GetComponent<Proyectil>();
            if (scriptBala != null)
            {
                // Le decimos a la bala cuánto daño hace y a quién NO debe hacerle daño
                scriptBala.Configurar(dano, tagAliado);
            }
        }
    }
}
