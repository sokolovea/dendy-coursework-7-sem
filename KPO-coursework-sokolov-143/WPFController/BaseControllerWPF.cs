
using BaseController;
using BaseMenuModel.Components;
using System.Windows;
using System.Windows.Input;
using WPFView;

namespace WPFController
{
    /// <summary>
    /// ������� ���������� ��� ������������ WPF
    /// </summary>
    public abstract class BaseControllerWPF : ControllerMenu
    {
        /// <summary>
        /// ������� ������� �� ������
        /// </summary>
        public delegate void dKeyPressed(object sender, KeyEventArgs args);

        /// <summary>
        /// ������� ������� �� ������
        /// </summary>
        public event dKeyPressed OnKeyPressed;

        /// <summary>
        /// ����������� �������� ����������� WPF
        /// </summary>
        public BaseControllerWPF()
        {
        }

        /// <summary>
        /// ���������� ������� ������
        /// </summary>
        /// <param name="parSender">�����������</param>
        /// <param name="parArgs">������� �������</param>
        protected virtual void KeyDownEventHandler(object parSender, KeyEventArgs parArgs)
        {
            switch (parArgs.Key)
            {
                case Key.Up:
                case Key.Left:
                    Menu.FocusPrevious();
                    break;
                case Key.Down:
                case Key.Right:
                    Menu.FocusNext();
                    break;
                case Key.Enter:
                    Menu.SelectFocusedItem();
                    break;
            }
        }

        /// <summary>
        /// ��������� ����������� ����
        /// </summary>
        public override void Start()
        {
            Window window = WindowSingleton.GetInstance().GetWindow();
            window.KeyDown += KeyDownEventHandler;
        }

        /// <summary>
        /// ��������� ����������� ����
        /// </summary>
        public override void Stop()
        {
            Window window = WindowSingleton.GetInstance().GetWindow();
            window.KeyDown -= KeyDownEventHandler;
        }

    }
}

