//Unity
using UnityEngine;

namespace Gameplay.Building
{
    public class GameBoard : MonoBehaviour
    {
        public int GetLayer()
        {
            return (1 << gameObject.layer);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public float GetSize()
        {
            return transform.localScale.x;
        }
    }
}
