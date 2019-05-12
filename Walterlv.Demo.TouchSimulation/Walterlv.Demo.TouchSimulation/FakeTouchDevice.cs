using System.Windows;
using System.Windows.Input;

namespace Walterlv.Demo.TouchSimulation
{
    public class FakeTouchDevice : TouchDevice
    {
        /// <inheritdoc />
        public FakeTouchDevice(int deviceId, Window window) : base(deviceId)
        {
            Window = window;
        }

        /// <summary>
        /// 触摸点
        /// </summary>
        public Point Position { set; get; }

        /// <summary>
        /// 触摸大小
        /// </summary>
        public Size Size { set; get; }

        public void Down()
        {
            TouchAction = TouchAction.Down;

            if (!IsActive)
            {
                SetActiveSource(PresentationSource.FromVisual(Window));

                Activate();
                ReportDown();
            }
            else
            {
                ReportDown();
            }
        }

        public void Move()
        {
            TouchAction = TouchAction.Move;

            ReportMove();
        }

        public void Up()
        {
            TouchAction = TouchAction.Up;

            ReportUp();
            Deactivate();
        }


        private Window Window { get; }

        private TouchAction TouchAction { set; get; }

        /// <inheritdoc />
        public override TouchPoint GetTouchPoint(IInputElement relativeTo)
        {
            return new TouchPoint(this, Position, new Rect(Position, Size), TouchAction);
        }

        /// <inheritdoc />
        public override TouchPointCollection GetIntermediateTouchPoints(IInputElement relativeTo)
        {
            return new TouchPointCollection()
            {
                GetTouchPoint(relativeTo)
            };
        }
    }

    //public class DemoTouchDevice : TouchDevice
    //{
    //    public DemoTouchDevice(int deviceId) : base(deviceId)
    //    {
    //    }

    //    public override TouchPoint GetTouchPoint(IInputElement relativeTo)
    //    {
    //    }

    //    public override TouchPointCollection GetIntermediateTouchPoints(IInputElement relativeTo)
    //    {
    //    }
    //}
}