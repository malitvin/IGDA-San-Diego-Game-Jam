//Unity
using UnityEngine;

namespace Gameplay.Building
{
    public class BuildingSystem : MonoBehaviour
    {
        public string _boardTag;

        public bool _buildModeEnabled;

        private Camera _mainCam;

        private GameBoard _gameBoard;
        private BuildHologram _buildHologram;

        private const float SEA_LEVEL = 0.5f;

        #region Unity Methods
        private void Start()
        {
            _mainCam = Camera.main;
            if(!_mainCam)
            {
                Debug.LogError("NO MAIN CAM FOUND ON BUILDINGSYSTEM");
            }
            _gameBoard = GameObject.FindGameObjectWithTag(_boardTag).GetComponent<GameBoard>();
            if(!_gameBoard)
            {
                Debug.LogError("NO GAME BOARD FOUND ON BUILDINGSYSTEM");
            }
            _buildHologram = GetComponentInChildren<BuildHologram>();
            if(!_buildHologram)
            {
                Debug.LogError("NO BUILD HOLOGRAM FOUND ON BUILDINGSYSTEM");
            }
        }
        private void Update()
        {
            if (_buildModeEnabled)
            {
                //Set our hologram position
                Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //IF WE ARE ON THE GAMEBOARD
                if (Physics.Raycast(ray, out hit, 100, _gameBoard.GetLayer()))
                {
                    Debug.Log(hit.transform.name);
                    Vector3 hitPosition = hit.point;
                    _buildHologram.transform.position = new Vector3(hitPosition.x, SEA_LEVEL, hitPosition.z);
                }
            }
            #endregion
        }
    }
}
