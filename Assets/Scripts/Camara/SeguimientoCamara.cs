using UnityEngine;

namespace NeonRust.Camara
{
    public class SeguimientoCamara : MonoBehaviour
    {
        public Transform objetivo;
        public float suavizado = 5f;
        public Vector3 offset = new Vector3(0, 3f, -12f); // Vista lateral Contra
        public float minY = 0f; // Evitar que la cámara baje demasiado

        private void LateUpdate()
        {
            if (objetivo == null) return;

            // Mantenerse en 2D, seguir en X e Y (pero limitado)
            Vector3 posicionDeseada = new Vector3(objetivo.position.x + offset.x, objetivo.position.y + offset.y, offset.z);
            
            // Límite inferior para que no vea debajo del piso
            if (posicionDeseada.y < minY)
            {
                posicionDeseada.y = minY;
            }

            transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
            
            // Siempre mirar al frente (Z = 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
