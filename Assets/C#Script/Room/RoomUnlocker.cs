using UnityEngine;

public class RoomUnlocker : MonoBehaviour
{
    [Header("所有房间槽位")]
    public LockRoom[] allRooms;

    public static RoomUnlocker Instance;
    void Awake() => Instance = this;

    void Start()
    {
        UpdateRoomVisuals();
    }

    // LockRoom点击时调用
    public void OnRoomClicked(LockRoom clickedRoom)
    {
        if (CanUnlock(clickedRoom.RoomId))
        {
            UIManager.Instance.ShowUnlockUI(clickedRoom.RoomId, () =>
            {
                ConfirmUnlock(clickedRoom);
            });
        }
        else
        {
            int needId = GameDataManager.Instance.UnlockRoomNum + 1;
            UIManager.Instance.ShowMessage($"需先解锁房间 {needId}");
        }
    }

    bool CanUnlock(int roomId)
    {
        return roomId == GameDataManager.Instance.UnlockRoomNum + 1;
    }

    void ConfirmUnlock(LockRoom room)
    {
        // 更新解锁进度
        GameDataManager.Instance.UpdateUnlockNum(room.RoomId);
        room.MarkUnlocked();

        // 进入建造流程
        RoomBuilder.Instance.PrepareBuild(room);
    }

    void UpdateRoomVisuals()
    {
        int unlocked = GameDataManager.Instance.UnlockRoomNum;
        foreach (var room in allRooms)
        {
            if (room.RoomId <= unlocked)
                room.MarkUnlocked();
        }
    }
}