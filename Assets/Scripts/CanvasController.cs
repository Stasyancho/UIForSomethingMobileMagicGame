using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LeftSideController))]
[RequireComponent(typeof(RightSideController))]
public class CanvasController : MonoBehaviour
{
    LeftSideController leftSideController;
    private bool leftUpTouchChecker = false;

    RightSideController rightSideController;
    private bool rightUpTouchChecker = false;

    void Start()
    {
        leftSideController = GetComponent<LeftSideController>();
        rightSideController = GetComponent<RightSideController>();
    }

    public void Update()
    {
        if (Input.touchCount > 0)
        {
            List<Touch> leftSideTouches = new List<Touch>();
            List<Touch> rightSideTouches = new List<Touch>();
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < Screen.width / 2)
                    leftSideTouches.Add(touch);
                else
                    rightSideTouches.Add(touch);
            }
            if (leftSideTouches.Count > 0)
                LeftController(leftSideTouches);
            else if (!leftUpTouchChecker)
            {
                LeftController();
                leftUpTouchChecker = true;
            }

            if (rightSideTouches.Count > 0)
                RightController(rightSideTouches);
            else if (!rightUpTouchChecker)
            {
                RightController();
                rightUpTouchChecker = true;
            }
        }
    }
    void LeftController(List<Touch> touches = null)
    {
        if (touches == null)
        {
            leftSideController.OnPointerUpBySide();
            return;
        }

        Touch touch = touches[0];
        switch (touch.phase)
        {

            case TouchPhase.Began:
                leftUpTouchChecker = false;
                leftSideController.OnPointerDownBySide(touch);
                break;

            case TouchPhase.Moved:
                if (!leftUpTouchChecker)
                    leftSideController.OnDragBySide(touch);
                break;

            case TouchPhase.Stationary:
                if (!leftUpTouchChecker)
                    leftSideController.OnDragBySide(touch);
                break;

            case TouchPhase.Ended:
                leftUpTouchChecker = true;
                leftSideController.OnPointerUpBySide();
                break;
        }
    }
    void RightController(List<Touch> touches = null)
    {
        if (touches == null)
        {
            rightSideController.OnPointerUpBySide();
            return;
        }

        Touch touch = touches[0];
        switch (touch.phase)
        {

            case TouchPhase.Began:
                rightUpTouchChecker = false;
                rightSideController.OnPointerDownBySide(touch);
                break;

            case TouchPhase.Moved:
                if (!rightUpTouchChecker)
                    rightSideController.OnDragBySide(touch);
                break;

            case TouchPhase.Stationary:
                if (!rightUpTouchChecker)
                    rightSideController.OnDragBySide(touch);
                break;

            case TouchPhase.Ended:
                rightUpTouchChecker = true;
                rightSideController.OnPointerUpBySide(touch);
                break;
        }
    }
}
