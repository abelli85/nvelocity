using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NVelocitySL.Context
{
    public interface ICloneable
    {
        /// <summary> Clones this context object.
        /// 
        /// </summary>
        /// <returns> A deep copy of this <code>Context</code>.
        /// </returns>
        object Clone();

    }
}
