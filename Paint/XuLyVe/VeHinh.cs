using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Paint
{
    class VeHinh : VeAbstract
    {
        private Shape currShape = null;
        private Tools currTool;

        public VeHinh(Canvas canvas, ThuocTinhVe ttv, Tools tool) : base(canvas, ttv)
        {
            currTool = tool;
        }
      
        private void setThuocTinh(Shape shape)
        {
            shape.Stroke = thuocTinhVe.ColorOutLineBrush;
            shape.StrokeThickness = thuocTinhVe.currSize;
            shape.Fill = thuocTinhVe.getFillBrush();
            shape.StrokeDashArray = thuocTinhVe.OutLineType;

        }

        //--------------------
        public override void XuLiMouseDown()
        {
            base.XuLiMouseDown();
            currShape = null;
            if (adnrLayer != null && currAdnr != null)
            {
                adnrLayer.Remove(currAdnr);
                currAdnr = null;
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

                if (currTool == Tools.Ellipse || currTool == Tools.Circle)
                {
                    if (currShape == null)
                    {
                        currShape = new Ellipse();
                        setThuocTinh(currShape);
                        MyCanvas.Children.Add(currShape);
                        NotifyActionCreated(new HanhDongThem(currShape));

                    }

                    Canvas.SetLeft(currShape, Math.Min(startPoint.Value.X, dropPoint.X));
                    Canvas.SetTop(currShape, Math.Min(startPoint.Value.Y, dropPoint.Y));

                    currShape.Width = Math.Abs(startPoint.Value.X - dropPoint.X);
                    currShape.Height = Math.Abs(startPoint.Value.Y - dropPoint.Y);

                    if (currTool == Tools.Circle)
                    {
                        currShape.Width = Math.Min(currShape.Width, currShape.Height);
                        currShape.Height = currShape.Width;
                    }


                }
                else if (currTool == Tools.Rectangle || currTool == Tools.Square)
                {
                    if (currShape == null)
                    {
                        currShape = new Rectangle();
                        setThuocTinh(currShape);
                        MyCanvas.Children.Add(currShape);
                        NotifyActionCreated(new HanhDongThem(currShape));

                    }
                    Canvas.SetLeft(currShape, Math.Min(startPoint.Value.X, dropPoint.X));
                    Canvas.SetTop(currShape, Math.Min(startPoint.Value.Y, dropPoint.Y));

                    currShape.Width = Math.Abs(startPoint.Value.X - dropPoint.X);
                    currShape.Height = Math.Abs(startPoint.Value.Y - dropPoint.Y);

                    if (currTool == Tools.Square)
                    {
                        currShape.Width = Math.Min(currShape.Width, currShape.Height);
                        currShape.Height = currShape.Width;
                    }

                }
                else if (currTool == Tools.Line)
                {
                    if (currShape == null)
                    {
                        currShape = new Line();
                        setThuocTinh(currShape);
                        MyCanvas.Children.Add(currShape);
                        NotifyActionCreated(new HanhDongThem(currShape));

                    }
                    (currShape as Line).X1 = startPoint.Value.X;
                    (currShape as Line).Y1 = startPoint.Value.Y;
                    (currShape as Line).X2 = dropPoint.X;
                    (currShape as Line).Y2 = dropPoint.Y;
                }
            }
        }
        public override void XuLiMouseUp()
        {
            base.XuLiMouseUp();
        
            if (currShape != null && MyCanvas != null)
            {
                if (currTool == Tools.Line)
                    adnrLayer.Add(currAdnr = new LineAdorner(currShape as Line));
                else if (currTool == Tools.Rectangle || currTool == Tools.Square)
                    adnrLayer.Add(currAdnr = new RectangleAdorner(currShape));
                else if (currTool == Tools.Ellipse || currTool == Tools.Circle)
                    adnrLayer.Add(currAdnr = new RectangleAdorner(currShape));
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
            if ( currShape != null)
            { 
                MyCanvas.Children.Remove(currShape);
                NotifyActionCreated(new HanhDongXoa(currShape));
                currShape = null;
            }
            startPoint = null;
        }

    }
}
