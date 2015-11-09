using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paint
{
    class Select : VeAbstract
    {
        Rectangle selectRectangle;
        Image currImage;

        public Select(Canvas canvas, ThuocTinhVe ttv) : base(canvas, ttv)
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
                currImage = Ultilities.Crop(MyCanvas, Canvas.GetLeft(selectRectangle), Canvas.GetTop(selectRectangle), selectRectangle.Width, selectRectangle.Height);

                Rectangle rectangle = new Rectangle()
                {
                    Fill = thuocTinhVe.ColorFillBrush,
                    Width = selectRectangle.Width,
                    Height = selectRectangle.Height
                };
                Canvas.SetLeft(rectangle, Canvas.GetLeft(selectRectangle));
                Canvas.SetTop(rectangle, Canvas.GetTop(selectRectangle));

                MyCanvas.Children.Add(rectangle);

                Canvas.SetLeft(currImage, Canvas.GetLeft(selectRectangle));
                Canvas.SetTop(currImage, Canvas.GetTop(selectRectangle));

                MyCanvas.Children.Add(currImage);
                List<IHanhDong> hd = new List<IHanhDong>();
                hd.Add(new HanhDongThem(rectangle));
                hd.Add(new HanhDongThem(currImage));

                NotifyActionCreated(new HanhDongChuoi(hd));
                currAdnr = new RectangleAdorner(currImage);
                adnrLayer.Add(currAdnr);
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
            if ( selectRectangle != null)
            {
                MyCanvas.Children.Remove(selectRectangle);
                selectRectangle = null;
                
            }
            if (currImage != null)
            {
                MyCanvas.Children.Remove(currImage);
                NotifyActionCreated(new HanhDongXoa(currImage));
                currImage = null;

            }

        }

        public override void Cut()
        {
            Copy();
            Delete();
        }

        public override void Copy()
        {
            if (currImage != null)
                Clipboard.SetImage(currImage.Source as BitmapSource);
        }

        public override void Paste()
        {
            if (Clipboard.ContainsImage())
            {
                currImage = null;
                XuLiMouseDown();
                BitmapSource bmp = Clipboard.GetImage();
                currImage = new Image()
                {
                    Source = bmp,
                    Height = bmp.Height,
                    Width = bmp.Width
                };
                Canvas.SetLeft(currImage, 0);
                Canvas.SetTop(currImage, 0);

                MyCanvas.Children.Add(currImage);
                NotifyActionCreated(new HanhDongThem(currImage));
                currAdnr = new RectangleAdorner(currImage);
                adnrLayer.Add(currAdnr);
            }
        }


    }
}
