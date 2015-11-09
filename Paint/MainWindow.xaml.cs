using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ThuocTinhVe ttv = new ThuocTinhVe();
        AdornerLayer adnrLayer;
        VeAbstract XuLiVe;
        byte indexMoreColor = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            adnrLayer = AdornerLayer.GetAdornerLayer(MyCanvas);
            ttv = new ThuocTinhVe() { };
        }


        private void cbbShapes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (XuLiVe != null)
                XuLiVe.XuLiMouseDown();//reset xu li ve cu
           
            switch (cbbShapes.SelectedIndex)
            {
                case 0:
                    XuLiVe = new VeHinh(MyCanvas, ttv, Tools.Line);
                    break;
                case 1:
                    XuLiVe = new VeHinh(MyCanvas, ttv, Tools.Ellipse);
                    break;
                case 2:
                    XuLiVe = new VeHinh(MyCanvas, ttv, Tools.Rectangle);
                    break;
                case 3:
                    XuLiVe = new VeHinh(MyCanvas, ttv, Tools.Circle);
                    break;
                case 4:
                    XuLiVe = new VeHinh(MyCanvas, ttv, Tools.Square);
                    break;
                case 5:
                    XuLiVe = new VeText(MyCanvas, ttv);
                    break;
                case 6:
                    XuLiVe = new Select(MyCanvas, ttv);
                    break;

            }
            XuLiVe.ActionCreated += new DoActionHandler(OnActionCreated);
        }

        private void OnActionCreated(object sender, DoActionEventArgs e)
        {
            undoredoManager.Add(e.hanhdong);
        }

        private void cbbSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String value = cbbSize.SelectedValue.ToString();
            ttv.currSize = Int32.Parse(value);
        }

        private void cbbFill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbbFill.SelectedIndex)
            {
                case 0:
                    ttv.FillType = Fills.NoFill;
                    break;
                case 1:
                    ttv.FillType = Fills.Solid;
                    break;
                case 2:
                    ttv.FillType = Fills.Linear;
                    break;
                case 3:
                    ttv.FillType = Fills.Radial;
                    break;
                case 4:
                    ttv.FillType = Fills.Image;
                    break;
            }
        }

        private void cbbOutLine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbbOutLine.SelectedIndex)
            {
                case 0:
                    ttv.OutLineType = new DoubleCollection() { 1, 0 };
                    break;
                case 1:
                    ttv.OutLineType = new DoubleCollection() { 1, 4 };
                    break;
                case 2:
                    ttv.OutLineType = new DoubleCollection() { 4, 1 };
                    break;
                case 3:
                    ttv.OutLineType = new DoubleCollection() { 4, 2, 1, 2 };
                    break;               
            }
        }


        //============================================================================================
        
        
        


        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (XuLiVe != null)
                XuLiVe.XuLiMouseDown();
            
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (XuLiVe != null)
                XuLiVe.XuLiMouseMove();
        }

        private void MyCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (XuLiVe != null)
                XuLiVe.XuLiMouseUp();
        }

        private void Paint_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Delete)
            {
                XuLiVe.Delete();
            }
        }
        #region x
        bool selectedButtonColor1 = true;
        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButtonColor1)
            {
                btnColor1.Background = (sender as Button).Background;
                ttv.ColorOutLineBrush = btnColor1.Background;
            }
            else
            {
                btnColor2.Background = (sender as Button).Background;
                ttv.ColorFillBrush = btnColor2.Background;
            }
        }
        
        private void btnMoreColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.ColorDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color cl = new Color();
                cl.A = dialog.Color.A;
                cl.R = dialog.Color.R;
                cl.G = dialog.Color.G;
                cl.B = dialog.Color.B;

                Brush br = new SolidColorBrush(cl);
                if (selectedButtonColor1)
                    btnColor1.Background = br;
                else
                    btnColor2.Background = br;

                switch (indexMoreColor)
                {
                    case 1:
                        btnMoreColor1.Background = br;
                        break;
                    case 2:
                        btnMoreColor2.Background = br;
                        break;
                    case 3:
                        btnMoreColor3.Background = br;
                        break;
                    case 4:
                        btnMoreColor4.Background = br;
                        break;
                    case 5:
                        btnMoreColor5.Background = br;
                        break;
                    case 6:
                        btnMoreColor6.Background = br;
                        break;
                }
                indexMoreColor++;
                if (indexMoreColor == 7)
                    indexMoreColor = 1;

                ttv.ColorOutLineBrush = btnColor1.Background;
                ttv.ColorFillBrush = btnColor2.Background;

            }
        }

        private void btnColor1_Click(object sender, RoutedEventArgs e)
        {
            selectedButtonColor1 = true;
        }

        private void btnColor2_Click(object sender, RoutedEventArgs e)
        {
            selectedButtonColor1 = false;
        }

        




        //-----------------------------------------------
        private void btnCut_Click(object sender, RoutedEventArgs e)
        {
            XuLiVe.Cut();
        }
        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            XuLiVe.Copy();
        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            XuLiVe.Paste();
        }
        //---------------------------------------------------------
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if ( MyCanvas == null) return;
            Image saveData = Croper.Crop(MyCanvas, 0, 0, MyCanvas.Width, MyCanvas.Height);

            SaveFileDialog saveFileDialogue = new SaveFileDialog()
            {
                FileName = "Untitled",
                Filter = "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf"
            };
            if (saveFileDialogue.ShowDialog() == true)
            {
                //Image saveData = paintLogic.Save();
                BitmapEncoder encoder;
                #region Encoding
                string extension = System.IO.Path.GetExtension(saveFileDialogue.FileName);
                switch (extension.ToLower())
                {
                    case ".jpeg":
                        encoder = new JpegBitmapEncoder();
                        break;
                    case ".png":
                        encoder = new PngBitmapEncoder();
                        break;
                    case ".gif":
                        encoder = new GifBitmapEncoder();
                        break;
                    case ".tiff":
                        encoder = new TiffBitmapEncoder();
                        break;
                    case ".wmf":
                        encoder = new WmpBitmapEncoder();
                        break;
                    case ".bmp":
                        encoder = new BmpBitmapEncoder();
                        break;
                    default:
                        return;
                }
                #endregion
                encoder.Frames.Add(saveData.Source as BitmapFrame);
                using (FileStream file = File.Create(saveFileDialogue.FileName))
                {
                    encoder.Save(file);
                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialogue = new OpenFileDialog()
            {
                Filter = "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf"
            };

            if (openFileDialogue.ShowDialog() == true)
            {
                Stream imageStreamSource = new FileStream(openFileDialogue.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                BitmapDecoder decoder;
                #region Decoding
                string extension = System.IO.Path.GetExtension(openFileDialogue.FileName);
                switch (extension.ToLower())
                {
                    case ".jpeg":
                        decoder = new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    case ".png":
                        decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    case ".gif":
                        decoder = new GifBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    case ".tiff":
                        decoder = new TiffBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    case ".wmf":
                        decoder = new WmpBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    case ".bmp":
                        decoder = new BmpBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    default:
                        return;
                }
                #endregion
                BitmapSource LoadedBitmap = decoder.Frames[0];
                MyCanvas.Children.Clear();
                MyCanvas.Width = LoadedBitmap.Width;
                MyCanvas.Height = LoadedBitmap.Height;
                MyCanvas.Children.Add(
                            new Image() { Source = LoadedBitmap }
                        );

                
            }
        }



        #endregion

        //------------------------------------
        // undo redo

        UndoRedoManager undoredoManager = new UndoRedoManager();
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            undoredoManager.Undo(MyCanvas);
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            undoredoManager.Redo(MyCanvas);
        }
    }
}
