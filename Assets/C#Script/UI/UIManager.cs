using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("面板")]
    public GameObject chooseUIPanel;
    public TextMeshProUGUI goldText;      // 金币显示
    public TextMeshProUGUI messageText;   // 提示信息

    [Header("房型按钮")]
    public Button[] roomButtons;          // 拖入按钮
    public TextMeshProUGUI[] costTexts;   // 对应消耗显示

    private System.Action onConfirmCallback;

    public static UIManager Instance;
    void Awake() => Instance = this;

    void Update()
    {
        // 实时更新金币显示
        goldText.text = $"金币: {GameDataManager.Instance.Gold}";
    }

    // Unlocker调用
    public void ShowUnlockUI(int roomId, System.Action onConfirm)
    {
        onConfirmCallback = onConfirm;
        chooseUIPanel.SetActive(true);
        messageText.text = $"解锁房间 {roomId}";
    }

    // Builder调用，更新各按钮消耗显示
    public void ShowRoomCosts(RoomBuilder.RoomConfig[] configs)
    {
        for (int i = 0; i < roomButtons.Length && i < configs.Length; i++)
        {
            int cost = configs[i].buildCost;
            costTexts[i].text = $"{configs[i].displayName}\n{cost}G";

            // 变红提示买不起
            bool canAfford = GameDataManager.Instance.Gold >= cost;
            costTexts[i].color = canAfford ? Color.white : Color.red;
        }
    }

    public void OnConfirmButton()
    {
        onConfirmCallback?.Invoke();
        // 不关闭面板，等Builder成功后再关
    }

    public void OnCancelButton()
    {
        onConfirmCallback = null;
        chooseUIPanel.SetActive(false);
    }

    public void ShowMessage(string msg)
    {
        messageText.text = msg;
        Invoke(nameof(ClearMessage), 2f);
    }

    void ClearMessage() => messageText.text = "";
}