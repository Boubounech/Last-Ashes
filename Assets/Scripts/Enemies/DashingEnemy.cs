using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashingEnemy : AIBaseEnemy
{
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    public float timeBeforeDash = 0.75f;
    public float timeAfterDash = 0.5f;

    public override void Awake()
    {
        base.Awake();
        OnStartAttacking.AddListener(delegate { StartCoroutine(DashTowardsPlayer(movingRight ? 1 : -1)); });
    }

    IEnumerator DashTowardsPlayer(float direction)
    {
        yield return new WaitForSeconds(timeBeforeDash);

        float dashTime = 0;
        while (dashTime < dashDuration)
        {
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, Vector2.right * direction, wallRayDistance, groundLayer);
            Vector2 edgeRayOrigin = transform.position + Vector3.right * direction * 0.5f;
            RaycastHit2D groundHit = Physics2D.Raycast(edgeRayOrigin, Vector2.down, edgeRayDistance, groundLayer);
            if (wallHit.collider != null || groundHit.collider == null)
            {
                break;
            }

            transform.Translate(Vector2.right * direction * dashSpeed * Time.deltaTime);
            dashTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(timeAfterDash);

        OnEndAttacking.Invoke();
    }

}
