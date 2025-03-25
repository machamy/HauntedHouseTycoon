
using System;
using System.Collections.Generic;
using UnityEngine;

public class GuestVisualHolder : MonoBehaviour
{
    [SerializeField] private GuestParty _guestParty;
    [SerializeField] private GuestVisual DefualtVisual;
    [SerializeField] private List<GuestVisual> _visuals = new List<GuestVisual>();
    [SerializeField] private float _positionOffset = 0.3f;
    [SerializeField] private float _positionCoefficient = 0.1f;
    private int GuestCount => _visuals.Count;
    private float PositionCoeff => _positionOffset + _positionCoefficient * GuestCount;

    public void Initialize(GuestParty guestParty)
    {
        _guestParty = guestParty;
    }

    public void AddGuestVisual(GuestData guestData)
    {
        
        if (String.IsNullOrEmpty(guestData.prefabPath))
        {
            Debug.Log($"prefabPath is null or empty, use default");
            var newVisual = Instantiate(DefualtVisual, transform);
            _visuals.Add(newVisual);
            newVisual.Initialize( _guestParty);
        }
        else
        {
            // TODO prefab path 적용
            Debug.LogWarning("아직 구현 안된 로직");
            var newVisual = Instantiate(DefualtVisual, transform);
            _visuals.Add(newVisual);
            newVisual.Initialize( _guestParty);
        }

        UpdatePos();
    }

    public void UpdatePos()
    {
        if (GuestCount < 1)
        {
            Debug.LogError("비주얼 개수가 0임. UpdatePos 불가");
            return;
        }

        if (GuestCount == 1)
        {
            _visuals[0].transform.localPosition = Vector3.zero;
            return;
        }

        for(int i = 0; i < _visuals.Count; i++)
        {
            var visual = _visuals[i];
            Vector3 localPosition = visual.transform.localPosition;
            float rad = Mathf.Deg2Rad * (20 + (5*GuestCount) + 360*(i/(float)GuestCount));
            localPosition.x = PositionCoeff * Mathf.Cos(rad);
            localPosition.z = PositionCoeff * Mathf.Sin(rad);
            visual.transform.localPosition = localPosition;
        }
    }
    
    public void SetIsMoving(bool isMoving)
    {
        foreach (var visual in _visuals)
        {
            visual.SetIsMoving(isMoving);
        }
    }
    
    public void PlayAnimation(AnimationType state, int index = 0)
    {
        foreach (var visual in _visuals)
        {
            visual.PlayAnimation(state, index);
        }
    }
}
