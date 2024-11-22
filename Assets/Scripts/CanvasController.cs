using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LeftSideController))]
[RequireComponent(typeof(RightSideController))]
public class CanvasController : MonoBehaviour
{
    //first layer of processing input data (touches)
    //divide touches into 2 groups: side and phase
    LeftSideController _leftSideController;
    RightSideController _rightSideController;
    //for correct divide touches
    bool _leftUpTouchChecker = false;
    bool _rightUpTouchChecker = false;

    void Start()
    {
        _leftSideController = GetComponent<LeftSideController>();
        _rightSideController = GetComponent<RightSideController>();
    }
    //Splits touches into left and right touches.
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
                Controller(leftSideTouches, _leftSideController, _leftUpTouchChecker, out _leftUpTouchChecker);
            else if (!_leftUpTouchChecker)
                Controller(null, _leftSideController, _leftUpTouchChecker, out _leftUpTouchChecker);

            if (rightSideTouches.Count > 0)
                Controller(rightSideTouches, _rightSideController, _rightUpTouchChecker, out _rightUpTouchChecker);
            else if (!_rightUpTouchChecker)
                Controller(null, _rightSideController, _rightUpTouchChecker, out _rightUpTouchChecker);
        }
    }
    
    void Controller(List<Touch> touches, ISideController controller, bool touchChecker, out bool outTouchChecker)
    {
        outTouchChecker = touchChecker;//outTouchChecker can't be null
        if (touches == null)//simulate release finger
        {
            outTouchChecker = true;
            controller.OnPointerUpBySide();
            return;
        }

        Touch touch = touches[0];//always use only first touch
        switch (touch.phase)
        {

            case TouchPhase.Began:
                outTouchChecker = false;
                controller.OnPointerDownBySide(touch);
                break;

            case TouchPhase.Moved:
                if (!touchChecker)
                    controller.OnDragBySide(touch);
                break;

            case TouchPhase.Stationary:
                if (!touchChecker)
                    controller.OnDragBySide(touch);
                break;

            case TouchPhase.Ended:
                outTouchChecker = true;
                controller.OnPointerUpBySide(touch);
                break;
        }
    }
}
