using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    //watch to event for movement and just waits trigger
    [SerializeField]private float _moveSpeed;
    [SerializeField]private float _rotateSpeed;
    CharacterController _characterController;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        GlobalEvents.MovedJoystickMoveAdd(MoveCharacter);
        GlobalEvents.MovedJoystickMoveAdd(RotateCharacter);
    }

    public void MoveCharacter(Vector2 data)
    {
        Vector3 direction = new Vector3(data.x, 0, data.y);
        _characterController.Move(direction * _moveSpeed * Time.deltaTime);
    }

    public void RotateCharacter(Vector2 data)
    {
        Vector3 direction = new Vector3(data.x, 0, data.y);
        if (Vector3.Angle(transform.forward, direction) > 0)
        {
            direction = Vector3.RotateTowards(transform.forward, direction, _rotateSpeed * Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
