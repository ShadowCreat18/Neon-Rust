using UnityEngine;

namespace NeonRust.Jugador
{
    [RequireComponent(typeof(Rigidbody))]
    public class ControladorJugador : MonoBehaviour
    {
        [Header("Movimiento")]
        [Header("Configuración 2.5D (Contra)")]
        public float velocidad = 8f;
        public float fuerzaSalto = 6f;
        public Transform pies;
        public float radioSuelo = 0.3f;
        public LayerMask capaSuelo;
        
        [Header("RPG")]
        public int nivel = 1;
        public int expActual = 0;
        public int expParaSubir = 50;

        private Rigidbody rb;
        private Camera camaraPrincipal;
        private NeonRust.Armas.SistemaDisparo sistemaDisparo;
        private Animator anim;
        private bool enSuelo;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            camaraPrincipal = Camera.main;
            sistemaDisparo = GetComponent<NeonRust.Armas.SistemaDisparo>();
            anim = GetComponent<Animator>();

            if (capaSuelo.value == 0) capaSuelo = ~0; // Todo por defecto si no se configura

            // Crear un objeto en los pies para detectar el suelo
            GameObject objPies = new GameObject("Pies");
            objPies.transform.SetParent(transform);
            objPies.transform.localPosition = new Vector3(0, 0.1f, 0);
            pies = objPies.transform;
        }

        private float horizontalInput;

        private void Update()
        {
            // Detectar suelo comprobando que tocamos algo que NO sea nosotros mismos
            bool enSuelo = false;
            if (pies != null)
            {
                Collider[] hits = Physics.OverlapSphere(pies.position, radioSuelo);
                foreach (var hit in hits)
                {
                    if (hit.gameObject != gameObject && !hit.isTrigger)
                    {
                        enSuelo = true;
                        break;
                    }
                }
            }

            horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D
            
            // Salto
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (enSuelo)
                {
                    Debug.Log("Volt: SALTO EJECUTADO (Sí detectó el suelo)");
                    rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
                }
                else
                {
                    Debug.Log("Volt: SALTO DENEGADO (No detectó el suelo o el aire)");
                }
            }

            if (anim != null)
            {
                anim.SetFloat("velocidad", Mathf.Abs(horizontalInput));
            }

            ApuntarHaciaMouse2D();

            if (Input.GetMouseButton(0) && sistemaDisparo != null)
            {
                sistemaDisparo.IntentarDisparar();
            }
        }

        private void FixedUpdate()
        {
            // Movimiento lateral (solo X)
            rb.linearVelocity = new Vector3(horizontalInput * velocidad, rb.linearVelocity.y, 0);
        }

        private void ApuntarHaciaMouse2D()
        {
            // Apuntar en el plano 2D (Z=0)
            Ray ray = camaraPrincipal.ScreenPointToRay(Input.mousePosition);
            Plane planoJuego = new Plane(Vector3.forward, Vector3.zero); // Un plano en Z=0 mirando hacia -Z
            float distancia;
            
            if (planoJuego.Raycast(ray, out distancia))
            {
                Vector3 puntoMouse = ray.GetPoint(distancia);
                
                // Rotar para mirar al mouse, pero solo en el eje Y (izquierda/derecha)
                if (puntoMouse.x > transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0); // Mirar derecha
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, -90, 0); // Mirar izquierda
                }

                // Aquí idealmente el arma o los brazos rotarían hacia arriba/abajo (Aim IK)
                // Para mantenerlo arcade básico, dispararemos directo al punto
                if (sistemaDisparo != null && sistemaDisparo.puntoDisparo != null)
                {
                    sistemaDisparo.puntoDisparo.LookAt(puntoMouse);
                }
            }
        }

        public void GanarExperiencia(int cantidad)
        {
            expActual += cantidad;
            Debug.Log($"<color=cyan>Chatarra recogida!</color> EXP: {expActual}/{expParaSubir}");
            if (expActual >= expParaSubir)
            {
                SubirNivel();
            }
        }

        private void SubirNivel()
        {
            nivel++;
            expActual -= expParaSubir;
            expParaSubir = Mathf.RoundToInt(expParaSubir * 1.5f);
            
            // Subir daño y salud
            var entidad = GetComponent<NeonRust.Nucleo.Entidad>();
            if (entidad != null)
            {
                entidad.saludMaxima += 20;
                entidad.saludActual = entidad.saludMaxima;
            }
            if (sistemaDisparo != null)
            {
                sistemaDisparo.cadencia = Mathf.Max(0.1f, sistemaDisparo.cadencia - 0.05f);
            }
            
            Debug.Log($"<color=yellow>¡LEVEL UP!</color> Nivel {nivel} alcanzado. Más velocidad de ataque y vida restaurada.");
        }
    }
}
