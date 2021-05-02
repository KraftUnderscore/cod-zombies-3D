using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float roundsPerSecond;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private ParticleSystem particles;

    private Transform shootPoint;
    private float timeToNextShot = 0f;

    private void Start()
    {
        shootPoint = Camera.main.transform;
    }

    private void OnEnable()
    {
        timeToNextShot = 0f;
    }
    public void Shoot()
    {
        if (timeToNextShot > 0f)
        {
            timeToNextShot -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            particles.Play();
            timeToNextShot = 1f / roundsPerSecond;
            RaycastHit[] hits = Physics.RaycastAll(shootPoint.position, shootPoint.forward, range, targetMask);

            foreach (var hit in hits)
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.transform.GetComponent<Enemy>().GetDamage(damage);
                }
            }
        }
    }
}
