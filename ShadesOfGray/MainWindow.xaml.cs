using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Globalization;

namespace ShadesOfGray
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string testo;

        public static byte[] ConvertiByte(string tx)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(tx);
            return bytes;
        }

        public static void SaveCanvas(Window window, Canvas canvas, int dpi, string filename)
        {
            Size size = new Size(window.Width, window.Height);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap(
                (int)window.Width, //width
                (int)window.Height, //height
                dpi, //dpi x
                dpi, //dpi y
                PixelFormats.Pbgra32 // pixelformat
                );
            rtb.Render(canvas);

            SaveRTBAsPNG(rtb, filename);
        }

        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }

        public static List<string> ConvertiInLista(string tx, int lunghezza)
        {
            
            List<string> Lista = new List<string>();
            while (tx.Length > lunghezza)
            {
                Lista.Add(tx.Substring(0, lunghezza));
                tx = tx.Substring(lunghezza, tx.Length - lunghezza);
            }
            Lista.Add(tx.Substring(0, tx.Length));
            return Lista;
        }

        public static string ByteArrayToHexString(byte[] Bytes)
        {
            StringBuilder Result = new StringBuilder(Bytes.Length * 2);
            string HexAlphabet = "0123456789ABCDEF";

            foreach (byte B in Bytes)
            {
                Result.Append(HexAlphabet[(int)(B >> 4)]);
                Result.Append(HexAlphabet[(int)(B & 0xF)]);
            }

            return Result.ToString();
        }

        public static byte[] FileToByteArray(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        private static int CalcolaDimensioneQuadrato(int lunghezza)
        {
            if (lunghezza > 0)
            {
                int risultato = Convert.ToInt32(Math.Truncate(Math.Sqrt(lunghezza)));
                if (risultato * risultato < lunghezza)
                    return risultato + 1;
                else
                    return risultato;
            }
            return 0;
        }

        private void btTest_Click(object sender, RoutedEventArgs e)
        {
            Canvas _Immagine = new Canvas();
                        
            byte[] bArray = FileToByteArray(tbText.Text);

            Debug.WriteLine("-----------------------------------");
            Debug.WriteLine("Nome file aperto : " + tbText.Text);
            Debug.WriteLine("Totale byte : " + bArray.Length);

            int DimQuadrato = CalcolaDimensioneQuadrato (bArray.Length );

            Debug.WriteLine("-CALCOLO DIMENSIONE IMMAGINE-");
            Debug.WriteLine("Dimensione quadrato immagine : " + DimQuadrato);
            Debug.WriteLine("Byte totali a disposizione   : " + DimQuadrato * DimQuadrato);
            Debug.WriteLine("Byte inutilizzati            : " + (DimQuadrato * DimQuadrato - bArray.Length));

            // PARAMETRI GENERALI
            int dimBpx = 1;
            int dvX = 0;
            int dvY = 0;
            
            int cont = 0;
            for (int riga = 0; riga < DimQuadrato; riga++ )
            { 
                for (int colonna = 0; colonna < DimQuadrato; colonna++)
                {
                    System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
                    
                    Color ColorByte = Color.FromRgb (255,0,0);
                    if(cont < bArray.Length)
                        ColorByte = Color.FromArgb(255, bArray[cont], bArray[cont], bArray[cont]);
                    rect.Stroke = new SolidColorBrush(ColorByte);
                    rect.Fill = new SolidColorBrush(ColorByte);
                    rect.Width = dimBpx;
                    rect.Height = dimBpx;
                    Canvas.SetLeft(rect, dvX + colonna * dimBpx);
                    Canvas.SetTop(rect, dvY + riga * dimBpx);
                    
                    _Immagine.Children .Add(rect);

                    cont++;
                } // FINE COLONNE
            } // FINE RIGHE
            int vecchiaW = Convert.ToInt32 (this.Width);
            int vecchiaH = Convert.ToInt32 (this.Height);

            this.Width = DimQuadrato * dimBpx;
            this.Height = DimQuadrato * dimBpx;
            //this.Content = _Immagine;

            SaveCanvas(this, _Immagine , 96, "Export_" + tbText.Text + ".png");
            MessageBox.Show("OK");

            this.Width = vecchiaW;
            this.Height = vecchiaH;
             
        }

           

        private void btFile_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("OK");
        } // FINE TEST_CLICK
    }
}