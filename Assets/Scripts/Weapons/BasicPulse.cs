using UnityEngine;

namespace NeonRust.Weapons
{
    public class BasicPulse : WeaponBase
    {
        protected override void Shoot()
        {
            if (projectilePrefab != null && firePoint != null)
            {
                // Instancia el proyectil en el punto de disparo con la rotación del arma
                GameObject bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                
                // Configura el daño del proyectil si tiene el script correspondiente
                Projectile projScript = bullet.GetComponent<Projectile>();
                if (projScript != null)
                {
                    projScript.SetDamage(damage);
                }
            }
        }
    }
}
