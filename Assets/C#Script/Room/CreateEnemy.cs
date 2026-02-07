using System.Collections;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    [Header("生成设置")]
    [Tooltip("要生成的预制体")]
    [SerializeField] private GameObject enemyPrefab;

    [Tooltip("初始生成间隔（秒）")]
    [SerializeField] private float baseInterval = 20f;

    [Header("升级设置")]
    [Tooltip("二级间隔（秒）")]
    [SerializeField] private float level2Interval = 15f;

    [Tooltip("三级间隔（秒）")]
    [SerializeField] private float level3Interval = 10f;

    private int currentLevel = 1;
    private float currentInterval;
    private Coroutine spawnRoutine;

    // 属性供外部查询
    public int CurrentLevel => currentLevel;
    public int MaxLevel => 3;
    public bool CanUpgrade => currentLevel < MaxLevel;
    public float CurrentInterval => currentInterval;

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning($"[{nameof(CreateEnemy)}] enemyPrefab 未设置，停止生成。", this);
            return;
        }

        currentInterval = baseInterval;
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(currentInterval);
        }
    }

    /// <summary>
    /// 升级生成速度（由升级按钮脚本调用）
    /// </summary>
    public bool Upgrade()
    {
        if (!CanUpgrade)
        {
            Debug.LogWarning($"[{nameof(CreateEnemy)}] 已达到最高等级 {MaxLevel}");
            return false;
        }

        currentLevel++;

        switch (currentLevel)
        {
            case 2:
                currentInterval = level2Interval;
                break;
            case 3:
                currentInterval = level3Interval;
                break;
        }

        Debug.Log($"[{nameof(CreateEnemy)}] 升级到 Lv.{currentLevel}，生成间隔: {currentInterval}s");

        RestartSpawning();
        return true;
    }

    private void RestartSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }

        if (enemyPrefab != null && currentInterval > 0f)
        {
            spawnRoutine = StartCoroutine(SpawnLoop());
        }
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    public void StartSpawning()
    {
        if (spawnRoutine == null && enemyPrefab != null && currentInterval > 0f)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }
}
