using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Marionnettiste : MonoBehaviour
{
    public SpriteRenderer render;
    public GameObject[] enemyPrefabs; 
    public Transform[] spawnPoints;  
    public BoxCollider2D rightCollider;
    public BoxCollider2D leftCollider;
    public float fadeDuration = 1f;
    public int enemiesPerWave = 3;
    public float timeBeforeFade = 1f;
    private bool isVulnerable = false;
    private int currentWave = 0;
    private int waves = 5;
    private bool isDead = false;
    [SerializeField] private string bossID = "Marionnettiste";

    public UnityEvent OnDeath = new UnityEvent();
    private AudioSource audioSource;

    private void Awake()
    {
        rightCollider.enabled = false;
        leftCollider.enabled = false;
        GetComponent<Damageable>().OnDamaged.AddListener(delegate { isVulnerable = false; });
        OnDeath.AddListener(delegate
        {
            SaveManager.instance.bossesToSave.Add(this.bossID);
            isDead = true;
            GameEvents.OnPlayerKilledBoss.Invoke();
        });
        this.audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (SaveManager.instance.bossesSaved.Contains(this.bossID) || SaveManager.instance.bossesToSave.Contains(this.bossID))
        {
            isDead = true;
        }
        if (isDead)
        {
            Destroy(this.gameObject);
        }
        render.color = new Color(1, 1, 1, 1);
        StartCoroutine(StartFight());
    }

    IEnumerator StartFight()
    {
        while (currentWave < waves)
        {
            yield return StartCoroutine(FadeOut());
            SpawnEnemies();
            StartCoroutine(MoveToPosition(new Vector3(transform.position.x, 10.0f, transform.position.z), fadeDuration));
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(FadeIn());
            StartCoroutine(MoveToPosition(new Vector3(transform.position.x, 4.5f, transform.position.z), fadeDuration));
            isVulnerable = true;
            rightCollider.enabled = true;
            leftCollider.enabled = true;
            while (isVulnerable)
            {
                yield return new WaitForSeconds(0.5f);
            }

            rightCollider.enabled = false;
            leftCollider.enabled = false;

            currentWave++;
        }

        EndFight();
    }

    IEnumerator FadeIn()
    {
        float timer = 0;
        float startAlpha = 0.5f;
        float endAlpha = 1f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            render.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        render.color = new Color(1, 1, 1, endAlpha);
    }

    IEnumerator FadeOut()
    {
        float timer = fadeDuration;
        float targetAlpha = 0.5f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float alpha = Mathf.Lerp(targetAlpha, 1f, timer / fadeDuration);
            render.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        render.color = new Color(1, 1, 1, targetAlpha);
    }

    IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, timer / duration);
            yield return null;
        }
        transform.position = targetPosition;
    }

    void SpawnEnemies()
    {
        List<int> usedPositions = new List<int>();
        List<int> chosenEnemies = new List<int>();

        while (chosenEnemies.Count < enemiesPerWave)
        {
            int randomEnemy = Random.Range(0, 5);
            if (!chosenEnemies.Contains(randomEnemy))
            {
                chosenEnemies.Add(randomEnemy);
            }
        }
        foreach (int enemyIndex in chosenEnemies)
        {
            Transform spawnPoint = null;

            if (enemyIndex <= 1) 
            {
                spawnPoint = GetAvailableSpawnPoint(new[] { 0, 1 }, usedPositions);
            }
            else 
            {
                spawnPoint = GetAvailableSpawnPoint(new[] { 2, 3 }, usedPositions);
            }

            if (spawnPoint != null)
            {
                Instantiate(enemyPrefabs[enemyIndex], spawnPoint.position, Quaternion.identity);
            }
        }
    }

    Transform GetAvailableSpawnPoint(int[] validIndexes, List<int> usedPositions)
    {
        List<int> availableIndexes = new List<int>();
        foreach (int index in validIndexes)
        {
            if (!usedPositions.Contains(index))
            {
                availableIndexes.Add(index);
            }
        }

        if (availableIndexes.Count > 0)
        {
            int selectedIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
            usedPositions.Add(selectedIndex); 
            return spawnPoints[selectedIndex];
        }

        return null; 
    }
    void EndFight()
    {
        OnDeath.Invoke();
        this.audioSource.Stop();
        StartCoroutine(deathCooldown());
    }

    IEnumerator deathCooldown()
    {
        render.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    public string GetId()
    {
        return this.bossID;
    }


}
