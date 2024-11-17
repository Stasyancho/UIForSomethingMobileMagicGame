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
    LeftSideController leftSideController;
    RightSideController rightSideController;
    //for correct divide touches
    private bool leftUpTouchChecker = false;
    private bool rightUpTouchChecker = false;

    void Start()
    {
        leftSideController = GetComponent<LeftSideController>();
        rightSideController = GetComponent<RightSideController>();
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
                Controller(leftSideTouches, leftSideController, leftUpTouchChecker, out leftUpTouchChecker);
            else if (!leftUpTouchChecker)
                Controller(null, leftSideController, leftUpTouchChecker, out leftUpTouchChecker);

            if (rightSideTouches.Count > 0)
                Controller(rightSideTouches, rightSideController, rightUpTouchChecker, out rightUpTouchChecker);
            else if (!rightUpTouchChecker)
                Controller(null, rightSideController, rightUpTouchChecker, out rightUpTouchChecker);
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
