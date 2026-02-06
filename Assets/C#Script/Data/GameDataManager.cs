using UnityEngine;
using System.IO;

public class GameDataManager : MonoBehaviour
{
    //找GameData的路径
    private string path;
    private GameData data;

    public static GameDataManager Instance;

    public int Gold => data.gold;
    public int UnlockRoomNum => data.unlockRoomNum;

    void Awake()
    {
        Instance = this;
        path = Path.Combine(Application.persistentDataPath, "GameData.json");
        Load();
    }

    void Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            // 初始值：3关已解锁，100金币
            data = new GameData { unlockRoomNum = 3, gold = 100 };
            Save();
        }
        Debug.Log($"加载存档：金币{data.gold}，已解锁{data.unlockRoomNum}");
    }
    
    //保存数据
    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    // 修改金币
    public bool SpendGold(int amount)
    {
        if (data.gold < amount) return false;
        data.gold -= amount;
        Save();
        return true;
    }

    //增加金币
    public void AddGold(int amount)
    {
        data.gold += amount;
        Save();
    }

    // 更新解锁进度
    public void UpdateUnlockNum(int num)
    {
        data.unlockRoomNum = num;
        Save();
    }
}