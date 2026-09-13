using UnityEngine;

namespace NeonRust.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Weapon Stats")]
        public float fireRate = 0.5f;
        public float damage = 10f;
        public GameObject projectilePrefab;
        public Transform firePoint;

        protected float nextFireTime = 0f;

        public virtual void TryShoot()
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }

        // Método abstracto que define CÓMO dispara el arma
        protected abstract void Shoot();
    }
}
