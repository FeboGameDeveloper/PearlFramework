using System;

namespace Pearl.UI
{
    public interface IPearlSelectable
    {
        event Action OnSelected;
        event Action OnDeselected;
        event Action OnHighlighted;
        event Action OnNotHighlighted;
        event Action OnPressed;
        event Action OnUp;
    }
}
