//Unity
using UnityEngine;

namespace Gameplay.Building
{
    [RequireComponent(typeof(MeshFilter))]
    public class BuildHologram : MonoBehaviour
    {
        private Vector3 _setPosition = new Vector3(0, 0, 0);
        private Renderer _renderer;
        private Renderer _Renderer
        {
            get { return _renderer ?? (_renderer = GetComponent<Renderer>()); }
        }

        private MeshFilter _mesh;
        private MeshFilter _meshFilter
        {
            get { return _mesh ?? (_mesh = GetComponent<MeshFilter>()); }
        }

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

        public void SetMesh(Mesh mesh)
        {
            if(mesh)
            {
                _meshFilter.mesh = mesh;
            }else
            {
                Debug.LogError("NO MESH IS VALID FOR HOLOGRAM");
            }
        }

        public void SetMaterial(Material mat)
        {
            _Renderer.material = mat;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public Vector3 GetScale()
        {
            return transform.localScale;
        }

        public void SetScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }

        public float GetSize()
        {
            return transform.localScale.x;
        }

        public float GetHalfSize()
        {
            return GetSize() / 2;
        }

        public void UpdateHologram(bool canBuild,BuildConfig.HologramData data)
        {
            _Renderer.material.color = canBuild ? data.buildColor : data.cantBuildColor;
        }
    }
}
