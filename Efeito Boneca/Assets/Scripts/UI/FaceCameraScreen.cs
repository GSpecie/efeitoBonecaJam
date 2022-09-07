using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceCameraScreen : MonoBehaviour
{
    [SerializeField] Image _playerDashBar;
    [SerializeField] Image _playerShootBar;

    Vector3 newPlaceToDashBar = new Vector3(0, -10.2f, 0);

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        //Vector3 playerPos = Camera.main.WorldToScreenPoint(_player.transform.position);

        //_playerDashBar.transform.position = playerPos;

        Vector3 namePos = Camera.main.WorldToScreenPoint(this.transform.position);

        _playerShootBar.transform.position = namePos;

        _playerDashBar.transform.position = namePos + newPlaceToDashBar;

        //transform.position = _player.transform.position;

        //transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);

        //transform.LookAt(transform.position + _camera.transform.rotation * Vector3.back, _camera.transform.rotation * Vector3.down);
    }
}
