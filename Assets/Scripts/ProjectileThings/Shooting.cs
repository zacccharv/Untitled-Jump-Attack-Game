using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZaccCharv
{
    public class Shooting : MonoBehaviour
    {
        public GameObject projectile;
        private Transform parent;
        private GameObject newProjectile;
        public float xShootVel;
        public float yShootVel;
        public float _interval;

        private void Start()
        {
            parent = transform;
            InvokeRepeating("Shoot",_interval, _interval);
        }

        private void Update()
        {
            if (newProjectile != null)
            {
                newProjectile.GetComponent<Bullet>().xVel = xShootVel;
                newProjectile.GetComponent<Bullet>().yVel = yShootVel;
            }
        }

        void Shoot()
        {
            newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            newProjectile.transform.SetParent(parent, true);
        }
    }
}
