using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LockRoom : MonoBehaviour
{
    [SerializeField] private int roomId;
    public int RoomId => roomId;
    public bool IsUnlocked;

    void Awake()
    {
        if (int.TryParse(gameObject.name, out int id))
            roomId = id;
    }

    void OnMouseDown()
    {
        RoomUnlocker.Instance.OnRoomClicked(this);
    }

    public void MarkUnlocked()
    {
        IsUnlocked = true;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.green;
    }
}