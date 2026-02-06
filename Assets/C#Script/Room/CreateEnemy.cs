
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    [Tooltip("要生成的预制体")]
    [SerializeField] private GameObject enemyPrefab;

    [Tooltip("生成间隔（秒），必须大于 0")]
    [SerializeField] private float spawnInterval = 5f;

    private Coroutine spawnRoutine;

    private void Start()
    {
        // 自动开始生成（如果预制体已配置）
        if (enemyPrefab == null)
        {
            Debug.LogWarning($"[{nameof(CreateEnemy)}] enemyPrefab 未设置，停止生成。", this);
            return;
        }

        if (spawnInterval <= 0f)
        {
            Debug.LogWarning($"[{nameof(CreateEnemy)}] spawnInterval 必须大于 0，当前值：{spawnInterval}。", this);
            return;
        }

        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 可选控制方法
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
        if (spawnRoutine == null && enemyPrefab != null && spawnInterval > 0f)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }
}
