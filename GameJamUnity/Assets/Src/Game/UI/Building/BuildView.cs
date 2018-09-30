using UnityEngine.UI;
using GhostGen;

namespace UI.Building
{
    public class BuildView : UIView
    {
        public HorizontalLayoutGroup _buildableParent;
        public HorizontalLayoutGroup _inventoryParent;

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

    }
}
