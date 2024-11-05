using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftSideController : MonoBehaviour
{
    [SerializeField] private Image pointForJoystick;
    [SerializeField] private Image JoystickBackground;
    [SerializeField] private Image Joystick;

    private Vector2 _JoystickBackgroundStartPosition;
    private Vector2 _InputVector;

    private void Start()
    {
        _JoystickBackgroundStartPosition = JoystickBackground.rectTransform.anchoredPosition;
    }
    public void OnPointerDownBySide(Touch touch)
    {
        Vector2 joystickBackgroundPosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(pointForJoystick.rectTransform, touch.position, null, out joystickBackgroundPosition))
        {
            JoystickBackground.rectTransform.anchoredPosition = new Vector2(joystickBackgroundPosition.x, joystickBackgroundPosition.y);
        }
    }

    public void OnPointerUpBySide()
    {
        JoystickBackground.rectTransform.anchoredPosition = _JoystickBackgroundStartPosition;
        Joystick.rectTransform.anchoredPosition = Vector2.zero;
        _InputVector = Vector2.zero;
    }

    public void OnDragBySide(Touch touch)
    {
        Vector2 joystickPosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickBackground.rectTransform, touch.position, null, out joystickPosition))
        {
            _InputVector = new Vector2(joystickPosition.x * 2 / JoystickBackground.rectTransform.sizeDelta.x, joystickPosition.y * 2 / JoystickBackground.rectTransform.sizeDelta.y);

            if (_InputVector.magnitude < 0.25f)
            {
                _InputVector = Vector2.zero;
            }
            else if (_InputVector.magnitude > 1f)
            {
                _InputVector = _InputVector.normalized;
            }

            Joystick.rectTransform.anchoredPosition = new Vector2(_InputVector.x * JoystickBackground.rectTransform.sizeDelta.x / 2, _InputVector.y * JoystickBackground.rectTransform.sizeDelta.y / 2);

            GlobalEvents.MovedJoystickMoveInvoke(_InputVector);
        }
    }
}
