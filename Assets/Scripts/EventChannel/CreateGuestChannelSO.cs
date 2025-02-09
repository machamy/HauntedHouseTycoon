using UnityEngine;

namespace EventChannel
{
    [CreateAssetMenu(fileName = "CreateGuestChannel", menuName = "EventChannel/CreateGuestChannel")]
    public class CreateGuestChannelSO : ScriptableObject
    {
        public delegate Guest CreateGuest(Vector3 position = default);
        private CreateGuest OnCreateGuest;
        
        /// <summary>
        /// 손님을 만드는 이벤트 리스너를 등록한다.
        /// 기존 리스너는 덮어쓰기 된다.(하나의 리스너만 등록 가능)
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterListener(CreateGuest listener)
        {
            OnCreateGuest = listener;
        }
        
        public void UnregisterListener(CreateGuest listener)
        {
            OnCreateGuest -= listener;
        }
        
        public void ClearListener()
        {
            OnCreateGuest = null;
        }
        
        public Guest RaiseCreateGuest(Vector3 position = default)
        {
            return OnCreateGuest?.Invoke(position);
        }
    }
}