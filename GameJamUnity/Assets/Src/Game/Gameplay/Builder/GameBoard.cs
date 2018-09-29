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
    }
}
