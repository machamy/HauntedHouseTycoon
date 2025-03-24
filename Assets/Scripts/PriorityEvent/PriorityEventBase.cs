using System;
using System.Collections.Generic;
using UnityEngine.Events;


/// <summary>
/// 우선순위 이벤트
/// priority의 값이 낮을수록 먼저 호출된다.
/// </summary>
public abstract class PriorityEventBase
{
    protected bool _isInvoking = false;
    protected List<int> _keysToClear = new List<int>();
    protected bool _clearAll = false;
}
