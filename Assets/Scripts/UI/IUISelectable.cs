using System;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public interface IUISelectable
    {
        Selectable Selectable { get; }

        bool FirstEnable { get; set; }

        UISelectableOnClick OnClickCallback { get; set; }

        void OnClick();

        void SelectOnEnable();
    }
}
