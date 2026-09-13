using UnityEngine;

namespace NeonRust.Armas
{
    public class PulsoBasico : ArmaBase
    {
        protected override void Disparar()
        {
            if (prefabProyectil != null && puntoDisparo != null)
            {
                // Instancia el proyectil en el punto de disparo con la rotación del arma
                GameObject bala = Instantiate(prefabProyectil, puntoDisparo.position, puntoDisparo.rotation);
                
                // Configura el daño del proyectil si tiene el script correspondiente
                Proyectil scriptProyectil = bala.GetComponent<Proyectil>();
                if (scriptProyectil != null)
                {
                    scriptProyectil.EstablecerDano(dano);
                }
            }
        }
    }
}
