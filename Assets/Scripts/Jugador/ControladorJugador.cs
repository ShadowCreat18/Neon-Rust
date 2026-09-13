using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeonRust.Armas; // Add reference to weapons namespace

namespace NeonRust.Jugador
{
    public class ControladorJugador : MonoBehaviour
    {
        [Header("Configuración de Movimiento")]
        public float velocidadMovimiento = 5f;
        
        [Header("Configuración de Combate")]
        public ArmaBase armaActual;
        
        private Rigidbody2D rb;
        private Vector2 movimiento;
        private Vector2 posicionMouse;
        private Camera camaraPrincipal;

        void Start()
        {
            // Obtener el componente Rigidbody2D adherido a Volt
            rb = GetComponent<Rigidbody2D>();
            camaraPrincipal = Camera.main; // Caché de la cámara principal para calcular posición del mouse
        }

        void Update()
        {
            // Procesamiento de entrada para movimiento
            movimiento.x = Input.GetAxisRaw("Horizontal");
            movimiento.y = Input.GetAxisRaw("Vertical");

            // Procesamiento de entrada para apuntar con el mouse
            posicionMouse = camaraPrincipal.ScreenToWorldPoint(Input.mousePosition);

            // Entrada de disparo
            if (Input.GetButton("Fire1") && armaActual != null)
            {
                armaActual.IntentarDisparar();
            }
        }

        void FixedUpdate()
        {
            // Cálculos físicos para el movimiento
            rb.MovePosition(rb.position + movimiento.normalized * velocidadMovimiento * Time.fixedDeltaTime);

            // Cálculos físicos para la rotación (mirar hacia el mouse)
            Vector2 direccionMirada = posicionMouse - rb.position;
            float angulo = Mathf.Atan2(direccionMirada.y, direccionMirada.x) * Mathf.Rad2Deg - 90f; // Offset de -90 asumiendo que el sprite mira hacia arriba
            rb.rotation = angulo;
        }
    }
}
