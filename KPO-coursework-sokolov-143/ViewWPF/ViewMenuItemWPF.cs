
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using BaseMenuModel;

namespace ViewWPF
{
    public class ViewMenuItemWPF
    {
        public delegate void dEnter(int parId);
        public event dEnter Enter = null;
        private FrameworkElement _parentControl = null;
        Button _button = null;
        Brush _brush = null;
        public ViewMenuItemWPF(BaseMenuModel.MenuItem parItem) : base(parItem)
        {
            _button = new Button();
            _button.Content = parItem.Name;
            _button.Click += (s, e) => { Enter?.Invoke(this.Item.ID); };
            Height = (int)_button.Height;
            Width = (int)_button.Width;
            parItem.Selected += ParItem_Selected;
        }

        private void ParItem_Selected()
        {
            _button.Focus();
            Draw();
            //_brush = _button.Background;
            //_button.Background = Brushes.Magenta;
            //_parentControl?.InvalidateVisual();
        }
        public override void Draw()
        {
            _button.Margin = new Thickness(X, Y, 0, 0);
            if (this.Item.State == Model.Menu.MenuItem.States.Focused || this.Item.State == Model.Menu.MenuItem.States.Selected)
                _button.Background = Brushes.Magenta;
            else
                _button.Background = _brush;
        }

        public void SetParentControl(FrameworkElement parControl)
        {
            if (_parentControl == null)
            {
                _parentControl = parControl;
                ((IAddChild)_parentControl).AddChild(_button);
            }
        }

    }
