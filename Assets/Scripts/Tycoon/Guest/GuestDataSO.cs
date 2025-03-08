
using UnityEngine;

[CreateAssetMenu(fileName = "GuestDataSO", menuName = "ScriptableObjects/GuestDataSO", order = 1)]
public class GuestDataSO : ScriptableObject
{
    [SerializeField] private GuestData guestData;

    public GuestData OriginalGuestData => guestData;
    
    public GuestData GetCopy()
    {
        GuestData copy = new GuestData();
        guestData.CopyTo(copy);
        return copy;
    }
}
