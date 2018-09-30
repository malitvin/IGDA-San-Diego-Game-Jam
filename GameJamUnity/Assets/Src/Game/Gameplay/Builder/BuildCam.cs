//Unity
using UnityEngine;

namespace Gameplay.Building
{
    public class BuildCam
    {
        private BuildConfig _buildConfig;
        private BuildConfig.BuildCameraData _buildCamData;
        private BuildConfig.WorldLimit _worldLimit;

        private Vector3 _acceleration;
        private Vector3 _velocity;
        private Vector3 _currentPosition;

        private GameplayCamera _gameplayCam;

        public BuildCam(BuildConfig buildConfig,GameplayCamera gameplayCam)
        {
            _buildConfig = buildConfig;
            _buildCamData = _buildConfig._buildCamData;
            _worldLimit = _buildConfig._buildCamData.worldLimit;
            _gameplayCam = gameplayCam;
        }

        public void OnUpdate()
        {
            _gameplayCam.transform.localPosition = Vector3.Lerp(_gameplayCam.transform.localPosition, _currentPosition, Time.deltaTime);
        }

        public void OnFixedUpdate()
        {
            Vector3 camPos = _gameplayCam.transform.position;

            int mouseX = (int)Input.mousePosition.x;
            int mouseY = (int)Input.mousePosition.y;

            if (mouseX < _buildCamData.edgeLimit && camPos.x > _worldLimit.x)
            {
                _acceleration.x = (-_gameplayCam.transform.right).x;
            }
            else if (mouseX > Screen.width - _buildCamData.edgeLimit && camPos.x < _worldLimit.width)
            {
                _acceleration.x = (_gameplayCam.transform.right).x;
            }

            if (mouseY < _buildCamData.edgeLimit && camPos.z > _worldLimit.y)
            {
                _acceleration.z = (-_gameplayCam.transform.forward).z;
            }
            else if (mouseY > Screen.height - _buildCamData.edgeLimit && camPos.z < _worldLimit.height)
            {
                _acceleration.z = (_gameplayCam.transform.forward).z;
            }

            float deltaTime = Time.fixedDeltaTime;
            _acceleration = _acceleration.normalized * _buildCamData.speed;
            _currentPosition = _gameplayCam.transform.localPosition + _velocity * deltaTime;

            float dragForce = (1.0f - _buildCamData.drag * deltaTime);
            _velocity = (_velocity + _acceleration * deltaTime) * dragForce;
            _velocity.y = 0;

            _acceleration = Vector3.zero;
            
        }
    }
}
