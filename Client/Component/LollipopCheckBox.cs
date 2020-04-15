using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Wolf_Hack.Client.Component
{
    class LollipopCheckBox : CheckBox
    {
        #region Variables
        Timer AnimationTimer = new Timer
        {
            Interval = 1
        };
        GraphicsPath RoundedRectangle;

        string EllipseBG = "#508ef5";
        string EllipseBorder = "#3b73d1";

        Color EllipseBackColor;
        Color EllipseBorderBackColor;

        Color EnabledUnCheckedColor = ColorTranslator.FromHtml("#bcbfc4");
        Color EnabledUnCheckedEllipseBorderColor = ColorTranslator.FromHtml("#a9acb0");

        Color DisabledEllipseBackColor = ColorTranslator.FromHtml("#c3c4c6");
        Color DisabledEllipseBorderBackColor = ColorTranslator.FromHtml("#90949a");

        uint PointAnimationNum = 4;
        #endregion

        #region  Properties
        [Category("Appearance")]
        public string EllipseColor
        {
            get => EllipseBG;
            set
            {
                EllipseBG = value;

                Invalidate();
            }
        }

        [Category("Appearance")]
        public string EllipseBorderColor
        {
            get => EllipseBorder;
            set
            {
                EllipseBorder = value;

                Invalidate();
            }
        }
        #endregion

        #region Events
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            AnimationTimer.Start();
        }

        protected override void OnResize(EventArgs e)
        {
            Height = 19;
            Width = 47;

            RoundedRectangle = new GraphicsPath();

            uint Radius = 10;

            RoundedRectangle.AddArc(11, 4, Radius - 1, Radius, 180, 90);
            RoundedRectangle.AddArc(Width - 21, 4, Radius - 1, Radius, -90, 90);
            RoundedRectangle.AddArc(Width - 21, Height - 15, Radius - 1, Radius, 0, 90);
            RoundedRectangle.AddArc(11, Height - 15, Radius - 1, Radius, 90, 90);

            RoundedRectangle.CloseAllFigures();

            Invalidate();
        }
        #endregion

        public LollipopCheckBox()
        {
            Height = 19;
            Width = 47;

            DoubleBuffered = true;

            AnimationTimer.Tick += new EventHandler(AnimationTick);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics Graphics = pevent.Graphics;

            Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Graphics.Clear(Parent.BackColor);

            EllipseBackColor = ColorTranslator.FromHtml(EllipseBG);
            EllipseBorderBackColor = ColorTranslator.FromHtml(EllipseBorder);

            Graphics.FillPath(new SolidBrush(Color.FromArgb(115, Enabled ? Checked ? EllipseBackColor : EnabledUnCheckedColor : EnabledUnCheckedColor)), RoundedRectangle);
            Graphics.DrawPath(new Pen(Color.FromArgb(50, Enabled ? Checked ? EllipseBackColor : EnabledUnCheckedColor : EnabledUnCheckedColor)), RoundedRectangle);

            Graphics.FillEllipse(new SolidBrush(Enabled ? Checked ? EllipseBackColor : Color.White : DisabledEllipseBackColor), PointAnimationNum, 0, 18, 18);
            Graphics.DrawEllipse(new Pen(Enabled ? Checked ? EllipseBorderBackColor : EnabledUnCheckedEllipseBorderColor : DisabledEllipseBorderBackColor), PointAnimationNum, 0, 18, 18);
        }

        void AnimationTick(object sender, EventArgs e)
        {
            if (Checked)
            {
                if (PointAnimationNum < 24)
                {
                    PointAnimationNum += 1;

                    Invalidate();
                }
            }
            else if (PointAnimationNum > 4)
            {
                PointAnimationNum -= 1;

                Invalidate();
            }
        }
    }
}