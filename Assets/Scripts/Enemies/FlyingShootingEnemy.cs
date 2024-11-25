using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShootingEnemy : AIBaseFlyingEnemy
{
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float stunTime;
    [SerializeField] private float shootingTime;
    [SerializeField] private float shootingCooldown;
    [SerializeField] private GameObject projectilePrefab;
    public Vector2 detectionShootingSize;
    public Vector2 playerPos;

    public bool showDetectionShootingRectangle = true;
    private bool canShoot = true;
    private bool shootPlayerNearby = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        PlayerEvents.OnPlayerHitDamageable.AddListener(GetKnockback);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Update()
    {
        base.Update();
        if (canShoot)
        {
            Vector2 detectionCenter = (Vector2)transform.position;
            shootPlayerNearby = Physics2D.OverlapBox(detectionCenter, detectionShootingSize, 0f, playerLayer);
            if (shootPlayerNearby)
            {
                StartCoroutine(ShootEnemy());
            }
        }
    }

    private IEnumerator ShootEnemy()
    {
        this.canShoot = false;
        this.canMove = false;
        playerPos = new Vector2(player.position.x, player.position.y + 1.0f);
        yield return new WaitForSeconds(shootingTime);
        ShootProjectile();
        this.canMove = true;

        yield return new WaitForSeconds(shootingCooldown);
        this.canShoot = true;
    }

    private void ShootProjectile()
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Projectile projectile = projectileInstance.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Initialize(playerPos, transform.position);
        }
    }


    private void GetKnockback(float damage, GameObject reciever)
    {
        if (this.gameObject == reciever)
        {
            StartCoroutine(KnockbackAction());

        }
    }

    private IEnumerator KnockbackAction()
    {
        this.canMove = false;

        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        rb.velocity = knockbackDirection * knockbackSpeed;

        yield return new WaitForSeconds(knockbackTime);

        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(stunTime);

        this.canMove = true;
    }


    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (showDetectionShootingRectangle)
        {
            Gizmos.color = Color.yellow;
            Vector2 detectionCenter = (Vector2)transform.position;
            Gizmos.DrawWireCube(detectionCenter, detectionShootingSize);
        }
    }
}


