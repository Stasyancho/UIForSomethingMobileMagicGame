using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface ISideController
    {
        public void OnPointerDownBySide(Touch touch);
        public void OnDragBySide(Touch touch);
        public void OnPointerUpBySide();
        public void OnPointerUpBySide(Touch touch);
    }
}
