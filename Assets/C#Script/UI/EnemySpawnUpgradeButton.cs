using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySpawnUpgradeButton : MonoBehaviour
{
    [Header("引用")]
    [Tooltip("要升级的 CreateEnemy 脚本")]
    [SerializeField] private CreateEnemy createEnemy;

    [Tooltip("升级按钮")]
    [SerializeField] private Button upgradeButton;

    [Tooltip("按钮文本（显示等级和价格）")]
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("升级价格配置")]
    [Tooltip("升级到 Lv.2 的价格")]
    [SerializeField] private int level2Cost = 50;

    [Tooltip("升级到 Lv.3 的价格")]
    [SerializeField] private int level3Cost = 100;

    private void Start()
    {
        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(OnUpgradeClick);

        UpdateButtonUI();
    }

    private void Update()
    {
        // 实时更新按钮状态
        UpdateButtonUI();
    }

    private void OnUpgradeClick()
    {
        if (createEnemy == null)
        {
            Debug.LogWarning("[EnemySpawnUpgradeButton] createEnemy 未设置");
            return;
        }

        if (!createEnemy.CanUpgrade)
        {
            Debug.Log("已达到最高等级");
            return;
        }

        int cost = GetCurrentUpgradeCost();

        if (GameDataManager.Instance.SpendGold(cost))
        {
            if (createEnemy.Upgrade())
            {
                Debug.Log($"升级成功！花费 {cost} 金币");
                UpdateButtonUI();
            }
        }
        else
        {
            Debug.Log("金币不足！");
        }
    }

    private void UpdateButtonUI()
    {
        if (createEnemy == null || buttonText == null || upgradeButton == null)
            return;

        if (!createEnemy.CanUpgrade)
        {
            // 已满级
            buttonText.text = $"生成速度 MAX\nLv.{createEnemy.MaxLevel}";
            upgradeButton.interactable = false;
            return;
        }

        int cost = GetCurrentUpgradeCost();
        int nextLevel = createEnemy.CurrentLevel + 1;
        bool canAfford = GameDataManager.Instance.Gold >= cost;

        buttonText.text = $"加速生成 Lv.{nextLevel}\n{cost}G";
        buttonText.color = canAfford ? Color.white : Color.red;
        upgradeButton.interactable = canAfford;
    }

    private int GetCurrentUpgradeCost()
    {
        if (createEnemy == null) return 0;

        switch (createEnemy.CurrentLevel)
        {
            case 1: return level2Cost;  // 当前 Lv.1，升到 Lv.2
            case 2: return level3Cost;  // 当前 Lv.2，升到 Lv.3
            default: return 0;
        }
    }
}