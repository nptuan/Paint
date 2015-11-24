using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Paint
{
    class VeText : VeAbstract
    {
        Rectangle selectRectangle;
        TextBox currTextBox;

        public VeText(Canvas canvas, ThuocTinhVe ttv) : base(canvas, ttv)
        {

        }

        //--------------------
        public override void XuLiMouseDown()
        {
            base.XuLiMouseDown();
            selectRectangle = null;
            if (adnrLayer != null && currAdnr != null)
            {
                adnrLayer.Remove(currAdnr);
                currAdnr = null;
            }

            if ( currTextBox != null)
            {
                MyCanvas.Children.Remove(currTextBox);
                RotateTransform rotateTransform = currTextBox.RenderTransform as RotateTransform;
                double angle = (rotateTransform != null) ? rotateTransform.Angle : 0;
                TextBlock textblock = new TextBlock()
                {
                    FontSize = currTextBox.FontSize,
                    Width = currTextBox.Width,
                    Height = currTextBox.Height,
                    Text = currTextBox.Text,
                    FontFamily = thuocTinhVe.currFont,
                    Foreground = currTextBox.Foreground,
                    Background = currTextBox.Background,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = new RotateTransform(angle),
                    TextWrapping = TextWrapping.Wrap
                };

                
                Canvas.SetLeft(textblock, Canvas.GetLeft(currTextBox) + 3);
                Canvas.SetTop(textblock, Canvas.GetTop(currTextBox) + 1);

                MyCanvas.Children.Add(textblock);
                NotifyActionCreated(new HanhDongThem(textblock));
            }


        }

        //-------------------------
        public override void XuLiMouseMove()
        {
            base.XuLiMouseMove();
            if (MyCanvas == null || startPoint == null)
                return;

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Point dropPoint = Mouse.GetPosition(MyCanvas);
                if (selectRectangle == null)
                {
                    selectRectangle = new Rectangle()
                    {
                        Stroke = Brushes.Black,
                        StrokeDashArray = new DoubleCollection() { 4, 2 },
                        StrokeThickness = 1,
                        Fill = Brushes.Transparent
                    };

                    MyCanvas.Children.Add(selectRectangle);

                }
                Canvas.SetLeft(selectRectangle, Math.Min(startPoint.Value.X, dropPoint.X));
                Canvas.SetTop(selectRectangle, Math.Min(startPoint.Value.Y, dropPoint.Y));

                selectRectangle.Width = Math.Abs(startPoint.Value.X - dropPoint.X);
                selectRectangle.Height = Math.Abs(startPoint.Value.Y - dropPoint.Y);
            }
        }

        //---------------
        public override void XuLiMouseUp()
        {
            base.XuLiMouseUp();
            if (selectRectangle != null)
            {
                MyCanvas.Children.Remove(selectRectangle);
                currTextBox = new TextBox()
                {
                    Width = selectRectangle.Width,
                    Height = selectRectangle.Height,
                    Foreground = thuocTinhVe.ColorOutLineBrush,
                    FontSize = thuocTinhVe.currSize,
                    FontFamily = thuocTinhVe.currFont,
                    //Background = currTextBox.Background,
                    Background = thuocTinhVe.ColorFillBrush
                };                

                Canvas.SetLeft(currTextBox, Canvas.GetLeft(selectRectangle));
                Canvas.SetTop(currTextBox, Canvas.GetTop(selectRectangle));

                MyCanvas.Children.Add(currTextBox);
                currAdnr = new RectangleAdorner(currTextBox);
                adnrLayer.Add(currAdnr);

                currTextBox.Focus();
            }

        }

        //--------------------------------
        public override void Delete()
        {
            if (adnrLayer != null && currAdnr != null)
            {
                adnrLayer.Remove(currAdnr);
                currAdnr = null;

            }
            if (selectRectangle != null)
            {
                MyCanvas.Children.Remove(selectRectangle);
                selectRectangle = null;

            }
            if (currTextBox != null)
            {
                MyCanvas.Children.Remove(currTextBox);
                currTextBox  = null;

            }
            ///khong undo
        }
    }
}
