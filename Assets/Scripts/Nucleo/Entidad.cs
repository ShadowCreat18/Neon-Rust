using UnityEngine;

namespace NeonRust.Nucleo
{
    public class Entidad : MonoBehaviour
    {
        [Header("Estadísticas")]
        public float saludMaxima = 100f;
        public float saludActual;

        private void Start()
        {
            saludActual = saludMaxima;
        }

        public void RecibirDano(float cantidad)
        {
            saludActual -= cantidad;
            Debug.Log(gameObject.name + " recibió " + cantidad + " de daño. Salud restante: " + saludActual);

            // Efecto visual simple de daño (parpadeo rojo temporal)
            StartCoroutine(EfectoDano());

            if (saludActual <= 0)
            {
                Morir();
            }
        }

        private System.Collections.IEnumerator EfectoDano()
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            Color[] coloresOriginales = new Color[renderers.Length];

            // Guardar colores y poner en rojo
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.HasProperty("_Color"))
                {
                    coloresOriginales[i] = renderers[i].material.color;
                    renderers[i].material.color = Color.red;
                }
            }

            yield return new WaitForSeconds(0.15f);

            // Restaurar colores
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].material.HasProperty("_Color"))
                {
                    renderers[i].material.color = coloresOriginales[i];
                }
            }
        }

        protected virtual void Morir()
        {
            Debug.Log(gameObject.name + " ha sido destruido.");
            Destroy(gameObject);
        }
    }
}
