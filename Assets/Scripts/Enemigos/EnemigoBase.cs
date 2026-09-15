using UnityEngine;
using NeonRust.Nucleo;
using NeonRust.Armas;

namespace NeonRust.Enemigos
{
    // Ya no requerimos NavMeshAgent para hacerlo más fácil y sin necesidad de "Hornear" mapas
    public class EnemigoBase : Entidad
    {
        [Header("IA 2.5D")]
        public float velocidad = 3.5f;
        public float rangoAtaque = 12f;
        public GameObject prefabChatarra;
        
        private Transform objetivo;
        private SistemaDisparo sistemaDisparo;
        private Animator anim;
        private Vector3 direccionPatrulla = Vector3.left;
        private Rigidbody rb;

        private void Awake()
        {
            sistemaDisparo = GetComponent<SistemaDisparo>();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
        }

        protected override void Morir()
        {
            // Instanciar chatarra al morir
            if (prefabChatarra != null)
            {
                GameObject chatarra = Instantiate(prefabChatarra, transform.position + Vector3.up, Quaternion.identity);
                // Si la chatarra tiene Rigidbody, que salte un poquito
                Rigidbody rbChatarra = chatarra.GetComponent<Rigidbody>();
                if (rbChatarra != null) rbChatarra.AddForce(new Vector3(Random.Range(-2f, 2f), 5f, 0), ForceMode.Impulse);
            }
            
            base.Morir();
        }

        private void Start()
        {
            // Buscar al jugador por tag
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            if (jugador != null)
            {
                objetivo = jugador.transform;
            }
        }

        private void Update()
        {
            if (objetivo == null) return;

            float distanciaX = Mathf.Abs(transform.position.x - objetivo.position.x);
            float distanciaY = Mathf.Abs(transform.position.y - objetivo.position.y);

            // Si el jugador está en el mismo nivel (Y) y cerca (X)
            if (distanciaX <= rangoAtaque && distanciaY < 2f)
            {
                // Mirar al jugador
                if (objetivo.position.x > transform.position.x)
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                else
                    transform.rotation = Quaternion.Euler(0, -90, 0);

                // Apuntar arma directo al jugador
                if (sistemaDisparo != null && sistemaDisparo.puntoDisparo != null)
                {
                    Vector3 objPunto = new Vector3(objetivo.position.x, objetivo.position.y + 1f, 0);
                    sistemaDisparo.puntoDisparo.LookAt(objPunto);
                }

                if (anim != null) anim.SetFloat("velocidad", 0f);
                if (sistemaDisparo != null) sistemaDisparo.IntentarDisparar();
                
                // Detenerse (si usamos rigidbody dinámico)
                if (rb != null && !rb.isKinematic) rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            }
            else
            {
                // Moverse (Patrulla simple)
                if (anim != null) anim.SetFloat("velocidad", 1f);
                
                // Girar cuerpo
                if (direccionPatrulla.x > 0) transform.rotation = Quaternion.Euler(0, 90, 0);
                else transform.rotation = Quaternion.Euler(0, -90, 0);

                if (rb != null && !rb.isKinematic)
                {
                    rb.linearVelocity = new Vector3(direccionPatrulla.x * velocidad, rb.linearVelocity.y, 0);
                }
                else
                {
                    transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
                }

                // Cambiar dirección al azar o si choca
                if (Random.Range(0, 1000) < 5)
                {
                    direccionPatrulla = -direccionPatrulla;
                }
            }
        }
    }
}
