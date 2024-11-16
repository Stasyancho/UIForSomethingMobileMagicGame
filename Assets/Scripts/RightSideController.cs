using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightSideController : MonoBehaviour, ISideController
{
    [SerializeField] GameObject ring;
    [SerializeField] GameObject regions;
    [SerializeField] Image pointForRing;
    [SerializeField] List<Image> RedGreenBlueRegions;
    [SerializeField] Image pointForJoystick;
    [SerializeField] Image JoystickBackground;
    [SerializeField] Image Joystick;

    ControllerStatus status;
    bool PointOfTouch;
    Vector2 _JoystickBackgroundStartPosition;
    Vector2 _InputVector;
    string elems = "RGB";

    void Start()
    {
        status = ControllerStatus.Off;
        ClearColors();
        PointOfTouch = false;
        _JoystickBackgroundStartPosition = JoystickBackground.rectTransform.anchoredPosition;
    }
    public void OnPointerDownBySide(Touch touch)
    {
        Vector2 touchPosition;

        switch (status)
        {
            case ControllerStatus.Off:
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(pointForRing.rectTransform, touch.position, null, out touchPosition))
                {
                    if (Vector2.Distance(touchPosition, Vector2.zero) <= 100f)
                    {
                        PointOfTouch = true;
                    }
                }
                break;
            case ControllerStatus.Cast:
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(pointForRing.rectTransform, touch.position, null, out touchPosition))
                {
                    if (Vector2.Distance(touchPosition, Vector2.zero) <= 100f)
                    {
                        GlobalEvents.PickSpellInvoke(GetKey());
                        ChangeStatus();
                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(pointForJoystick.rectTransform, touch.position, null, out touchPosition))
                        {
                            JoystickBackground.rectTransform.anchoredPosition = new Vector2(touchPosition.x, touchPosition.y);
                        }
                    }
                }
                break;
            case ControllerStatus.Direction:
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(pointForJoystick.rectTransform, touch.position, null, out touchPosition))
                {
                    JoystickBackground.rectTransform.anchoredPosition = new Vector2(touchPosition.x, touchPosition.y);
                }
                break;
        }
    }
    public void OnDragBySide(Touch touch)
    {
        Vector2 touchPosition;

        if (status != ControllerStatus.Direction)
        {
            ClearColors();
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(pointForRing.rectTransform, touch.position, null, out touchPosition))
            {
                if (Vector2.Distance(touchPosition, Vector2.zero) > 100f)
                {
                    PointOfTouch = false;
                    if (Vector2.Distance(touchPosition, Vector2.zero) <= 300f && status == ControllerStatus.Cast)
                    {
                        switch ((int)((Vector3.SignedAngle(Vector3.up, new Vector3(touch.position.x, touch.position.y) - ring.transform.position, Vector3.forward) + 360f) % 360f / 120f))
                        {
                            case 0:
                                RedGreenBlueRegions[0].color = new Color(1f, 0, 0, 1f);
                                break;
                            case 1:
                                RedGreenBlueRegions[1].color = new Color(0, 1f, 0, 1f);
                                break;
                            case 2:
                                RedGreenBlueRegions[2].color = new Color(0, 0, 1f, 1f);
                                break;
                        }
                    }
                }
            }
        }
        else
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickBackground.rectTransform, touch.position, null, out touchPosition))
            {
                _InputVector = new Vector2(touchPosition.x * 2 / JoystickBackground.rectTransform.sizeDelta.x, touchPosition.y * 2 / JoystickBackground.rectTransform.sizeDelta.y);

                if (_InputVector.magnitude < 0.2f)
                {
                    _InputVector = Vector2.zero;
                }
                else if (_InputVector.magnitude > 1f)
                {
                    _InputVector = _InputVector.normalized;
                }

                Joystick.rectTransform.anchoredPosition = new Vector2(_InputVector.x * JoystickBackground.rectTransform.sizeDelta.x / 2, _InputVector.y * JoystickBackground.rectTransform.sizeDelta.y / 2);

                GlobalEvents.CastedJoystickMoveInvoke(_InputVector);
            }
        }
    }
    public void OnPointerUpBySide(Touch touch)
    {
        if (status != ControllerStatus.Direction)
        {
            Vector2 touchPosition;
            ClearColors();

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(pointForRing.rectTransform, touch.position, null, out touchPosition))
            {
                if (Vector2.Distance(touchPosition, Vector2.zero) <= 100f)
                {
                    if (PointOfTouch)
                    {
                        ChangeStatus();
                    }
                }
                else
                {
                    if (Vector2.Distance(touchPosition, Vector2.zero) <= 300f && status == ControllerStatus.Cast)
                    {
                        switch ((int)((Vector3.SignedAngle(Vector3.up, new Vector3(touch.position.x, touch.position.y) - ring.transform.position, Vector3.forward) + 360f) % 360f / 120f))
                        {
                            case 0:
                                AddElement('R');
                                break;
                            case 1:
                                AddElement('G');
                                break;
                            case 2:
                                AddElement('B');
                                break;
                        }
                        Debug.Log("Spell:" + elems);
                    }
                }
                PointOfTouch = false;
            }
        }
        else
        {
            if (_InputVector != Vector2.zero)
            {
                GlobalEvents.CastSpellInvoke(GetKey(), _InputVector);
                Joystick.rectTransform.anchoredPosition = Vector2.zero;
                _InputVector = Vector2.zero;
                ChangeStatus();
            }
            JoystickBackground.rectTransform.anchoredPosition = _JoystickBackgroundStartPosition;
        }
    }
    public void OnPointerUpBySide()
    {
        if (status == ControllerStatus.Direction)
        {
            GlobalEvents.CastedJoystickMoveInvoke(new Vector2(0, 0));
            JoystickBackground.rectTransform.anchoredPosition = _JoystickBackgroundStartPosition;
            Joystick.rectTransform.anchoredPosition = Vector2.zero;
            _InputVector = Vector2.zero;
        }
    }

    private void ChangeStatus()
    {
        switch (status)
        {
            case ControllerStatus.Off:

                regions.SetActive(true);

                status = ControllerStatus.Cast;
                break;
            case ControllerStatus.Cast:

                JoystickBackground.gameObject.SetActive(true);
                ring.SetActive(false);

                status = ControllerStatus.Direction;
                break;
            case ControllerStatus.Direction:

                JoystickBackground.gameObject.SetActive(false);
                regions.SetActive(false);
                ring.SetActive(true);

                status = ControllerStatus.Off;
                break;
        }
    }

    enum ControllerStatus
    {
        Off, //only button
        Cast, //button + pie menu
        Direction //0nly joystick
    }

    private void ClearColors()
    {
        RedGreenBlueRegions[0].color = new Color(1f, 0, 0, 0.4f);
        RedGreenBlueRegions[1].color = new Color(0, 1f, 0, 0.4f);
        RedGreenBlueRegions[2].color = new Color(0, 0, 1f, 0.4f);
    }
    void AddElement(char elem)
    {
        if (elem == 'R' ||  elem == 'G' || elem == 'B')
        {
            elems += elem;
            elems = elems.Substring(1);
        }
    }

    int GetKey()
    {
        int key = 0;

        foreach (char c in elems)
        {
            switch (c)
            {
                case 'G': key += 1; break;
                case 'B': key += 4; break;
            }
        }
        return key;
    }
}
