using UnityEngine;
using UnityEngine.Serialization;

namespace UI.GameUI
{
    /// <summary>
    /// 손님 Queue Bar UI를 관리하는 클래스
    /// </summary>
    public class GuestQueueBarUI : MonoBehaviour
    {
        [Header("Prefabs")] 
        [SerializeField] private GameObject node;
        [SerializeField] private GuestNodeDisplay guestNodeDisplay;
        [Header("References")]
        [SerializeField] private GameObject nodeHolder;
        [SerializeField] private GameObject displayHolder;
        
        public GuestNodeDisplay AddGuestNode(Guest guest)
        {
            GameObject node = Instantiate(this.node, nodeHolder.transform);
            GuestNodeDisplay nodeDisplay = Instantiate(guestNodeDisplay, displayHolder.transform);
            Vector3 position = transform.position;
            position.y = GetComponent<RectTransform>().rect.min.y;
            nodeDisplay.transform.position = position;
            nodeDisplay.SetGuest(guest);
            nodeDisplay.SetNode(node.transform);
            return nodeDisplay;
        }
    }
}