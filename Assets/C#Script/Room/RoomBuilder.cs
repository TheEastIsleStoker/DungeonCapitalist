using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    [Header("房型配置")]
    public RoomConfig[] roomConfigs;

    private LockRoom currentSlot;

    public static RoomBuilder Instance;
    void Awake() => Instance = this;

    [System.Serializable]
    public class RoomConfig
    {
        public string typeName;      // 变量名
        public GameObject prefab;    // 预制体
        public int buildCost;        // 建造消耗
        public string displayName;   // 显示名
    }

    // Unlocker调用，准备建造
    public void PrepareBuild(LockRoom slot)
    {
        currentSlot = slot;
        // 更新UI显示各房型消耗
        UIManager.Instance.ShowRoomCosts(roomConfigs);
    }

    // UI按钮调用
    public void TryBuildRoom(string roomType)
    {
        if (currentSlot == null) return;

        var config = System.Array.Find(roomConfigs, r => r.typeName == roomType);
        if (config == null)
        {
            Debug.LogError("未知房型: " + roomType);
            return;
        }

        // 验证金币
        if (!GameDataManager.Instance.SpendGold(config.buildCost))
        {
            UIManager.Instance.ShowMessage("金币不足！");
            return;
        }

        // 扣费成功，生成房间
        Build(config);
    }

    void Build(RoomConfig config)
    {
        Vector3 pos = currentSlot.transform.position;
        Quaternion rot = currentSlot.transform.rotation;

        Instantiate(config.prefab, pos, rot);
        Destroy(currentSlot.gameObject);

        UIManager.Instance.ShowMessage($"建造了{config.displayName}！");
        currentSlot = null;
    }

    public int GetCost(string roomType)
    {
        var config = System.Array.Find(roomConfigs, r => r.typeName == roomType);
        return config?.buildCost ?? 9999;
    }
}