using UnityEngine;

public class EnemySpawnerInteraction : MonoBehaviour
{
    [Header("UI 引用")]
    [Tooltip("点击后显示的升级 Canvas")]
    [SerializeField] private GameObject upgradeCanvas;

    private void Start()
    {
        // 初始隐藏 UI
        if (upgradeCanvas != null)
            upgradeCanvas.SetActive(false);
    }

    private void OnMouseDown()
    {
        // 点击物体时显示 UI
        if (upgradeCanvas != null)
        {
            upgradeCanvas.SetActive(true);
        }
    }

    // 供关闭按钮调用
    public void CloseUpgradeUI()
    {
        if (upgradeCanvas != null)
        {
            upgradeCanvas.SetActive(false);
        }
    }
}