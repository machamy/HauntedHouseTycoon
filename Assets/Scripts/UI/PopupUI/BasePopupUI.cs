
using System;

public class BasePopupUI : BaseUI
{
    public event Action OnClose; 
    public override void Init()
    {
        UIManager.Instance.SetCanvas(gameObject, true);
    }

    public virtual void Close()
    {
        OnClose?.Invoke();
        UIManager.Instance.ClosePopupUI(this);
    }


}
