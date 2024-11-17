using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    //interface for left and right SideController
    //to avoid code duplication
    public interface ISideController
    {
        public void OnPointerDownBySide(Touch touch);
        public void OnDragBySide(Touch touch);
        public void OnPointerUpBySide();
        public void OnPointerUpBySide(Touch touch);
    }
}
