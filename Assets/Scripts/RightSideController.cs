using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightSideController : MonoBehaviour, ISideController
{
    [SerializeField] GameObject _ring;
    [SerializeField] GameObject _regions;
    [SerializeField] Image _pointForRing;
    [SerializeField] List<Image> _RedGreenBlueRegions;
    [SerializeField] Image _pointForJoystick;
    [SerializeField] Image _JoystickBackground;
    [SerializeField] Image _Joystick;

    ControllerStatus status;  
    bool PointOfTouch;
    Vector2 _joystickBackgroundStartPosition;
    Vector2 _inputVector;
    string elems = "RGB";

    void Start()
    {
        status = ControllerStatus.Off;
        ClearColors();
        PointOfTouch = false;
        _joystickBackgroundStartPosition = _JoystickBackground.rectTransform.anchoredPosition;
    }

    public void OnPointerDownBySide(Touch touch)
    {
        Vector2 touchPosition;

        switch (status)
        {
            case ControllerStatus.Off://activate checker for button
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_pointForRing.rectTransform, touch.position, null, out touchPosition))
                {
                    if (Vector2.Distance(touchPosition, Vector2.zero) <= 100f)
                    {
                        PointOfTouch = true;
                    }
                }
                break;
            case ControllerStatus.Cast://jump to next status and event call(don't need release button)
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_pointForRing.rectTransform, touch.position, null, out touchPosition))
                {
                    if (Vector2.Distance(touchPosition, Vector2.zero) <= 100f)
                    {
                        GlobalEvents.PickSpellInvoke(GetKey());
                        ChangeStatus();
                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_pointForJoystick.rectTransform, touch.position, null, out touchPosition))
                        {
                            _JoystickBackground.rectTransform.anchoredPosition = new Vector2(touchPosition.x, touchPosition.y);
                        }
                    }
                }
                break;
            case ControllerStatus.Direction:
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_pointForJoystick.rectTransform, touch.position, null, out touchPosition))
                {
                    _JoystickBackground.rectTransform.anchoredPosition = new Vector2(touchPosition.x, touchPosition.y);
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
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_pointForRing.rectTransform, touch.position, null, out touchPosition))
            {
                //2 functions:
                //if (finger go out from button) then checker = false
                //if ((finger become between 100 and 300) and (now you pick spell) then some "pie piece" must be active
                if (Vector2.Distance(touchPosition, Vector2.zero) > 100f) 
                {
                    PointOfTouch = false;
                    if (Vector2.Distance(touchPosition, Vector2.zero) <= 300f && status == ControllerStatus.Cast)
                    {
                        switch ((int)((Vector3.SignedAngle(Vector3.up, new Vector3(touch.position.x, touch.position.y) - _ring.transform.position, Vector3.forward) + 360f) % 360f / 120f))
                        {
                            case 0:
                                _RedGreenBlueRegions[0].color = new Color(1f, 0, 0, 1f);
                                break;
                            case 1:
                                _RedGreenBlueRegions[1].color = new Color(0, 1f, 0, 1f);
                                break;
                            case 2:
                                _RedGreenBlueRegions[2].color = new Color(0, 0, 1f, 1f);
                                break;
                        }
                    }
                }
            }
        }
        else
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_JoystickBackground.rectTransform, touch.position, null, out touchPosition))
            {
                _inputVector = new Vector2(touchPosition.x * 2 / _JoystickBackground.rectTransform.sizeDelta.x, touchPosition.y * 2 / _JoystickBackground.rectTransform.sizeDelta.y);
                //magnitude - vector length
                //"if (magnitude> 1)" - joystick will run away
                //condition "<0.25" for possibility cancel movement
                if (_inputVector.magnitude < 0.2f)
                {
                    _inputVector = Vector2.zero;
                }
                else if (_inputVector.magnitude > 1f)
                {
                    _inputVector = _inputVector.normalized;
                }

                _Joystick.rectTransform.anchoredPosition = new Vector2(_inputVector.x * _JoystickBackground.rectTransform.sizeDelta.x / 2, _inputVector.y * _JoystickBackground.rectTransform.sizeDelta.y / 2);

                //connectiion with another elements works through Events
                GlobalEvents.CastedJoystickMoveInvoke(_inputVector);
            }
        }
    }
    public void OnPointerUpBySide(Touch touch)
    {
        if (status != ControllerStatus.Direction)
        {
            Vector2 touchPosition;
            ClearColors();

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_pointForRing.rectTransform, touch.position, null, out touchPosition))
            {
                if (Vector2.Distance(touchPosition, Vector2.zero) <= 100f)
                {
                    //"ChangeStatus" will be work if your finger stated on button from press to release
                    //this condition only for "ControllerStatus.Off"
                    if (PointOfTouch)
                    {
                        ChangeStatus();
                    }
                }
                else
                {
                    if (Vector2.Distance(touchPosition, Vector2.zero) <= 300f && status == ControllerStatus.Cast)
                    {
                        //save picked "pie piece"
                        switch ((int)((Vector3.SignedAngle(Vector3.up, new Vector3(touch.position.x, touch.position.y) - _ring.transform.position, Vector3.forward) + 360f) % 360f / 120f))
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
            //return to start position
            //if direction picked then activated event
            if (_inputVector != Vector2.zero)
            {
                GlobalEvents.CastSpellInvoke(GetKey(), _inputVector);
                _Joystick.rectTransform.anchoredPosition = Vector2.zero;
                _inputVector = Vector2.zero;
                ChangeStatus();
            }
            _JoystickBackground.rectTransform.anchoredPosition = _joystickBackgroundStartPosition;
        }
    }
    //return to start position
    public void OnPointerUpBySide()
    {
        if (status == ControllerStatus.Direction)
        {
            GlobalEvents.CastedJoystickMoveInvoke(new Vector2(0, 0));
            _JoystickBackground.rectTransform.anchoredPosition = _joystickBackgroundStartPosition;
            _Joystick.rectTransform.anchoredPosition = Vector2.zero;
            _inputVector = Vector2.zero;
        }
    }
    //sets required UI elements visible
    private void ChangeStatus()
    {
        switch (status)
        {
            case ControllerStatus.Off:

                _regions.SetActive(true);

                status = ControllerStatus.Cast;
                break;
            case ControllerStatus.Cast:

                _JoystickBackground.gameObject.SetActive(true);
                _ring.SetActive(false);

                status = ControllerStatus.Direction;
                break;
            case ControllerStatus.Direction:

                _JoystickBackground.gameObject.SetActive(false);
                _regions.SetActive(false);
                _ring.SetActive(true);

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
        _RedGreenBlueRegions[0].color = new Color(1f, 0, 0, 0.4f);
        _RedGreenBlueRegions[1].color = new Color(0, 1f, 0, 0.4f);
        _RedGreenBlueRegions[2].color = new Color(0, 0, 1f, 0.4f);
    }
    //always only 3 elements
    void AddElement(char elem)
    {
        if (elem == 'R' ||  elem == 'G' || elem == 'B')
        {
            elems += elem;
            elems = elems.Substring(1);
        }
    }
    //there calculate spell code.
    //R = 0
    //G = 1
    //B = 4
    //this parameters provide unique code
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
