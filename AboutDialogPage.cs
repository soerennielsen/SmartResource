using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.Shell;

namespace DelegateAS.SmartResource
{
    class AboutDialogPage : UIElementDialogPage
    {
        private UIElement _child;

        protected override UIElement Child
        {
            get
            {
                if (_child == null)
                {
                    _child = new AboutPage();
                }
                return _child;
            }
        }
    }
}
