using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftSideController : MonoBehaviour, ISideController
{
    [SerializeField] Image _pointForJoystick;
    [SerializeField] Image _JoystickBackground;
    [SerializeField] Image _Joystick;

    Vector2 _JoystickBackgroundStartPosition;
    Vector2 _InputVector;

    private void Start()
    {  
        _JoystickBackgroundStartPosition = _JoystickBackground.rectTransform.anchoredPosition;
    }
    public void OnPointerDownBySide(Touch touch)
    {
        Vector2 joystickBackgroundPosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_pointForJoystick.rectTransform, touch.position, null, out joystickBackgroundPosition))
        {
            _JoystickBackground.rectTransform.anchoredPosition = new Vector2(joystickBackgroundPosition.x, joystickBackgroundPosition.y);
        }
    }
    //only for intarface
    public void OnPointerUpBySide(Touch touch)
    {
        OnPointerUpBySide();
    }
    //return to start position
    public void OnPointerUpBySide()
    {
        _JoystickBackground.rectTransform.anchoredPosition = _JoystickBackgroundStartPosition;
        _Joystick.rectTransform.anchoredPosition = Vector2.zero;
        _InputVector = Vector2.zero;
    }

    public void OnDragBySide(Touch touch)
    {
        Vector2 joystickPosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_JoystickBackground.rectTransform, touch.position, null, out joystickPosition))
        {
            _InputVector = new Vector2(joystickPosition.x * 2 / _JoystickBackground.rectTransform.sizeDelta.x, joystickPosition.y * 2 / _JoystickBackground.rectTransform.sizeDelta.y);
            //magnitude - vector length
            //"if (magnitude> 1)" - joystick will run away
            //condition "<0.25" for possibility cancel movement
            if (_InputVector.magnitude < 0.25f)
            {
                _InputVector = Vector2.zero;
            }
            else if (_InputVector.magnitude > 1f)
            {
                _InputVector = _InputVector.normalized;
            }

            _Joystick.rectTransform.anchoredPosition = new Vector2(_InputVector.x * _JoystickBackground.rectTransform.sizeDelta.x / 2, _InputVector.y * _JoystickBackground.rectTransform.sizeDelta.y / 2);
            //connectiion with another elements works through Events
            GlobalEvents.MovedJoystickMoveInvoke(_InputVector);
        }
    }
}
