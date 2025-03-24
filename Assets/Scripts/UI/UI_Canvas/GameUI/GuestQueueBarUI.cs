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
        
        /// <summary>
        /// 해당 손님 정보를 가진 노드를 추가한다.
        /// </summary>
        /// <param name="guestParty"></param>
        /// <returns></returns>
        public GuestNodeDisplay AddGuestNode(GuestParty guestParty)
        {
            GameObject node = Instantiate(this.node, nodeHolder.transform);
            GuestNodeDisplay nodeDisplay = Instantiate(guestNodeDisplay, displayHolder.transform);
            Vector3 position = transform.position;
            position.y = GetComponent<RectTransform>().rect.min.y;
            nodeDisplay.transform.position = position;
            nodeDisplay.SetGuest(guestParty);
            nodeDisplay.SetNode(node.transform);
            return nodeDisplay;
        }
    }
}