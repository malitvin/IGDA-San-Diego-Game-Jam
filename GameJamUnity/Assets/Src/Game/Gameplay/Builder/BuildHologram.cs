//Unity
using UnityEngine;

namespace Gameplay.Building
{
    public class BuildHologram : MonoBehaviour
    {
        private Vector3 _setPosition = new Vector3(0, 0, 0);

        public void SetPosition(GridPosition pos)
        {
            _setPosition.x = pos.x;
            _setPosition.z = pos.z;
            _setPosition.y = pos.y;
            transform.position = _setPosition;
        }

        public Vector3 GetScale()
        {
            return transform.localScale;
        }

        public float GetSize()
        {
            return transform.localScale.x;
        }
    }
}
