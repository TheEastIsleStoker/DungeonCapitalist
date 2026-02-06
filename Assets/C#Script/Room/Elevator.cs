using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Elevator Settings")]
    [Tooltip("目标传送位置的 Transform（必填）")]
    [SerializeField] private Transform destination;

    [Tooltip("在触发器内停留多少秒后传送")]
    [SerializeField] private float delaySeconds = 2f;

    [Tooltip("要判定为玩家的 Tag（区分大小写），默认 \"player\"")]
    [SerializeField] private string playerTag = "player";

    // 跟踪每个进入触发器的 collider 对应的协程，以支持并发和取消
    private readonly Dictionary<Collider2D, Coroutine> activeCoroutines = new Dictionary<Collider2D, Coroutine>();

    private void Reset()
    {
        // 尝试在 Reset 时将 Collider 设置为触发器
        var col = GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsPlayer(other)) return;
        if (destination == null)
        {
            Debug.LogWarning($"[{nameof(Elevator)}] destination 未设置，无法传送。", this);
            return;
        }

        if (!activeCoroutines.ContainsKey(other))
        {
            var coro = StartCoroutine(TeleportAfterDelay(other));
            activeCoroutines.Add(other, coro);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsPlayer(other)) return;

        if (activeCoroutines.TryGetValue(other, out var coro))
        {
            StopCoroutine(coro);
            activeCoroutines.Remove(other);
        }
    }

    private bool IsPlayer(Collider2D col)
    {
        // 使用可配置的 tag 进行判定
        return col != null && col.CompareTag(playerTag);
    }

    private IEnumerator TeleportAfterDelay(Collider2D playerCollider)
    {
        float elapsed = 0f;
        while (elapsed < delaySeconds)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (playerCollider == null)
        {
            activeCoroutines.Remove(playerCollider);
            yield break;
        }

        // 尝试使用 Rigidbody2D 安全地设置位置并清除速度；否则设置 transform.position
        var rb = playerCollider.attachedRigidbody;
        if (rb != null)
        {
            rb.position = (Vector2)destination.position;
            rb.velocity = Vector2.zero;
        }
        else
        {
            playerCollider.transform.position = destination.position;
        }

        activeCoroutines.Remove(playerCollider);
    }
}
