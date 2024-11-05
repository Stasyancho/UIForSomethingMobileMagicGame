using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private GameObject _character;
    [SerializeField] private float _returnSpeed;
    [SerializeField] private float _height;
    [SerializeField] private float _rearDistance;

    private Vector3 currentVector;

    void Start()
    {
        transform.position = new Vector3(_character.transform.position.x, _character.transform.position.y + _height, _character.transform.position.z - _rearDistance);
        transform.rotation = Quaternion.LookRotation(_character.transform.position - transform.position);
    }

    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        currentVector = new Vector3(_character.transform.position.x, _character.transform.position.y + _height, _character.transform.position.z - _rearDistance);
        transform.position = Vector3.Lerp(transform.position, currentVector, _returnSpeed * Time.deltaTime);
    }
}
