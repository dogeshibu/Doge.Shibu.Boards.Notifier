using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Doge.Shibu.Boards.Notifier.ViewModels;
using MahApps.Metro.Controls;

namespace Doge.Shibu.Boards.Notifier
{
    public sealed class MetroWindowManager : WindowManager
    {

        private MetroWindow CreateNormalWindow(object view)
        {
            var window = new MetroWindow
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.CanMinimize,
                MinHeight = 150,
                MinWidth = 500,
                Title = "Notifier",
                ShowMinButton = true,
                Icon = new BitmapImage(new Uri(@"Resources/circular55.ico", UriKind.Relative))
            };

            window.SetResourceReference(MetroWindow.GlowBrushProperty, "AccentColorBrush");

            var owner = InferOwnerOf(window);
            if (owner != null)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.Owner = owner;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }


            window.SetValue(View.IsGeneratedProperty, true);
            return window;
        }

        private MetroWindow CreateNotificationWindow(INotificationViewModel model, object view)
        {
            var window = new MetroWindow
            {
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                MinHeight = 150,
                MinWidth = 500,
                Title = model.Title
            };

            window.SetResourceReference(MetroWindow.GlowBrushProperty, "AccentColorBrush");
            window.SetValue(View.IsGeneratedProperty, true);

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = desktopWorkingArea.Right - window.MinWidth;
            window.Top = desktopWorkingArea.Bottom - window.MinHeight;
            window.Topmost = true;

            window.Activated += (_, __) =>
            {
                if (!window.IsVisible)
                {
                    window.Show();
                }

                if (window.WindowState == WindowState.Minimized)
                {
                    window.WindowState = WindowState.Normal;
                }

                window.Activate();
                window.Topmost = true;  // important
                window.Topmost = false; // important
                window.Focus();         // important
            };

            Task.Delay(model.CloseAfter)
                .ContinueWith(_ => window.Close(),TaskScheduler.FromCurrentSynchronizationContext());

            return window;
        }


        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var window = view as MetroWindow;

            if (window == null)
            {
                if (model is INotificationViewModel)
                {
                    window = CreateNotificationWindow(model as INotificationViewModel,view);
                }
                else
                {
                    window = CreateNormalWindow(view);
                }
            }
            else
            {
                var owner2 = this.InferOwnerOf(window);
                if (owner2 != null && isDialog)
                {
                    window.Owner = owner2;
                }
            }
            return window;
        }
    }
}
