using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerDetector : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public Vector2 detectionSize = new Vector2(5f, 2f);
    public Vector2 headDetectionSize = new Vector2(1.5f, 0.5f);
    public bool showDetectionPlayerRectangle = true;
    public bool showHeadDetectionRectangle = true;

    protected bool movingRight = false;
    private bool playerNearby = false;
    private bool playerAbove = false;
    private bool canMove = true;

    public float wallRayDistance = 0.5f;
    public float edgeRayDistance = 1.0f;

    [SerializeField] private float checkInterval = 0.1f;
    [SerializeField] private bool playerInRange = false;
    [SerializeField] private Transform playerTransform;
    public Vector2 lastKnownPlayerPosition = Vector2.zero;

    public UnityEvent OnPlayerFound = new UnityEvent();
    public UnityEvent OnPlayerLost = new UnityEvent();

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(CheckSurroundings());
    }

    private void Update()
    {
        if (playerInRange)
        {
            lastKnownPlayerPosition = playerTransform.position;
        }
    }

    private IEnumerator CheckSurroundings()
    {
        do
        {
            yield return new WaitForSeconds(checkInterval);

            Vector2 detectionCenter = (Vector2)transform.position + Vector2.up * detectionSize.y / 2;
            playerNearby = Physics2D.OverlapBox(detectionCenter, detectionSize, 0f, playerLayer);

            Vector2 headDetectionCenter = (Vector2)transform.position + Vector2.up * (headDetectionSize.y / 2 + 1f);
            playerAbove = Physics2D.OverlapBox(headDetectionCenter, headDetectionSize, 0f, playerLayer);
            if (canMove)
            {
                if (playerNearby && !playerAbove)
                {
                    if (!playerInRange) // Just found player
                    {
                        playerInRange = true;
                        OnPlayerFound.Invoke();
                        Debug.Log("Player Found");
                    }
                }
                else
                {
                    if (playerInRange) // Player just exited zone
                    {
                        playerInRange = false;
                        OnPlayerLost.Invoke();
                        Debug.Log("Patrolling");
                    }
                }
            }
        } while (gameObject.activeInHierarchy);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float direction = movingRight ? 1 : -1;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * wallRayDistance);

        Gizmos.color = Color.blue;
        Vector3 edgeRayOrigin = transform.position + Vector3.right * direction * 0.5f;
        Gizmos.DrawLine(edgeRayOrigin, edgeRayOrigin + Vector3.down * edgeRayDistance);

        if (showDetectionPlayerRectangle)
        {
            Gizmos.color = Color.green;
            Vector2 detectionCenter = (Vector2)transform.position + Vector2.up * detectionSize.y / 2;
            Gizmos.DrawWireCube(detectionCenter, detectionSize);
        }

        if (showHeadDetectionRectangle)
        {
            Gizmos.color = Color.yellow;
            Vector2 headDetectionCenter = (Vector2)transform.position + Vector2.up * (headDetectionSize.y / 2 + 1f);
            Gizmos.DrawWireCube(headDetectionCenter, headDetectionSize);
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(lastKnownPlayerPosition, 0.2f);
    }
}
