//Unity
using UnityEngine;

namespace Gameplay.Building
{
    public class BuildHologram : MonoBehaviour
    {
        private Vector3 _setPosition = new Vector3(0, 0, 0);
        private Renderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }
        public void SetPosition(GridPosition pos)
        {
            _setPosition.x = pos.x;
            _setPosition.z = pos.z;
            _setPosition.y = pos.y;
            transform.position = _setPosition;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public Vector3 GetScale()
        {
            return transform.localScale;
        }

        public float GetSize()
        {
            return transform.localScale.x;
        }

        public float GetHalfSize()
        {
            return GetSize() / 2;
        }

        public void UpdateHologram(bool colliding,BuildConfig.HologramData data)
        {
            _renderer.material.color = colliding ? data.cantBuildColor : data.buildColor;
        }
    }
}
