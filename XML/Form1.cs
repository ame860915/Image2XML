using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//EmguCV
using Emgu.CV; //opencv 主要函式
using Emgu.CV.CvEnum; //CV列舉項
using Emgu.CV.Structure; //色彩形態定義
using Emgu.CV.Util; //特殊形別定義 CV使用

using System.IO;
using System.Xml;

namespace XML
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> ImageRGB;
        int H, W;
        Image<Gray, byte> ImagePaddy;
        int Xmin, Xmax, Ymin, Ymax;

        public Form1()
        {
            InitializeComponent();

            Form.CheckForIllegalCrossThreadCalls = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        
       

        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            using (FolderBrowserDialog FileImages = new FolderBrowserDialog())
            {
                FileImages.ShowDialog();

                if (FileImages.SelectedPath != "")
                {
                    label1.Text = FileImages.SelectedPath;
                }
            }
        }

        private void label2_MouseClick(object sender, MouseEventArgs e)
        {
            using (FolderBrowserDialog Save_Xml = new FolderBrowserDialog())
            {
                Save_Xml.ShowDialog();

                if (Save_Xml.SelectedPath != "")
                {
                    label2.Text = Save_Xml.SelectedPath;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() => Bounding_Box());
        }

        //Boy
        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(() => Bounding_Box_1());
        }

        void Bounding_Box()
        {
            if (comboBox_DeputyName.Text != "Select" || label2.Text != "Path..." || label1.Text != "Path...")
            {
                
                


                //try
                //{
                    button1.Enabled = false;
                    button2.Enabled = false;

                    //要檢測的資料夾路徑
                    string All_Path = label1.Text; //路徑
                    DirectoryInfo Path_Class = new DirectoryInfo(All_Path);
                    int quantityClass = Path_Class.GetDirectories("*").Length; //類別數量

                    string[] Classes = new string[quantityClass];
                    int n = 0;
                    foreach (var fi in Path_Class.GetDirectories("*"))
                    {
                        Classes[n] = fi.Name;
                        n++;
                    }

                    for (int i22 = 0; i22 < quantityClass; i22++)
                    {
                        string DParth = All_Path + @"\" + Classes[i22];

                        //檢測資料夾裡的圖片數量
                        DirectoryInfo Path_Images = new DirectoryInfo(DParth);
                        int quantity = Path_Images.GetFiles("*." + comboBox_DeputyName.Text).Length; //圖片數量
                        label4.Text = "Input : " + quantity.ToString();

                        //找Paddy
                        int count = 0;
                        foreach (var fi in Path_Images.GetFiles("*." + comboBox_DeputyName.Text))
                        {
                            count++;
                            label4.Text = "Input : " + quantity.ToString() + " / Output : " + count.ToString();
                            ImageRGB = new Image<Bgr, byte>(All_Path + @"\" + Classes[i22] + @"\" + fi.Name);
                            int H = ImageRGB.Height, W = ImageRGB.Width;
                            imageBox1.Image = ImageRGB;
                            //(治禾)
                            //Image<Hls, byte> ImageHLS = new Image<Hls, byte>(ImageRGB.Bitmap);
                            //Image<Gray, byte> ImageS = new Image<Gray, byte>(ImageHLS[2].Bitmap);
                            //(冬冬)
                            ImageRGB = ImageRGB.SmoothBlur(5, 5);

                        //Image<Gray, byte> og_otsu;

                        //og_otsu = new Image<Gray, byte>(ImageRGB.Bitmap);
                        //CvInvoke.Threshold(og_otsu, og_otsu, 0, 255, ThresholdType.Otsu);

                        //using (VectorOfVectorOfPoint contour = new VectorOfVectorOfPoint())
                        //{
                        //    double maxarea = 0; //取最大面積
                        //    int maxi = 0; //取最大面積的編號(contour的編號)
                        //    CvInvoke.FindContours(og_otsu, contour, null, RetrType.External, ChainApproxMethod.ChainApproxNone);
                        //    for (int i = 0; i < contour.Size; i++)
                        //    {
                        //        MessageBox.Show(CvInvoke.ContourArea(contour[i]).ToString());

                        //        if (CvInvoke.ContourArea(contour[i]) <= 250)
                        //        {
                        //            CvInvoke.DrawContours(og_otsu, contour, i, new MCvScalar(0), -1);
                        //        }
                        //        else
                        //        {
                        //            if (maxarea <= CvInvoke.ContourArea(contour[i]))
                        //            {
                        //                if (maxarea == 0)
                        //                {
                        //                    CvInvoke.DrawContours(og_otsu, contour, maxi, new MCvScalar(255), -1);
                        //                    maxi = i;
                        //                    maxarea = CvInvoke.ContourArea(contour[i]);

                        //                }
                        //                else
                        //                {
                        //                    CvInvoke.DrawContours(og_otsu, contour, maxi, new MCvScalar(0), -1);// 把多餘的雜訊變不見
                        //                    maxi = i;
                        //                    maxarea = CvInvoke.ContourArea(contour[i]);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}

                            Image<Lab, byte> ImageLab = new Image<Lab, byte>(ImageRGB.Bitmap);
                            Image<Gray, byte> ImageR = new Image<Gray, byte>(ImageRGB[2].Bitmap);
                            Image<Gray, byte> ImageL = new Image<Gray, byte>(ImageLab[0].Bitmap);
                            Image<Gray, byte> ImageLb = new Image<Gray, byte>(ImageLab[2].Bitmap);
                            Image<Gray, byte> ImageOTSU = new Image<Gray, byte>(W, H);
                            //CvInvoke.Threshold(ImageL, ImageL, 140, 255, ThresholdType.Binary);
                            CvInvoke.Threshold(ImageLb, ImageLb, 80, 255, ThresholdType.Binary);
                            CvInvoke.Threshold(ImageR, ImageR, 45, 255, ThresholdType.Binary);
                            ImageOTSU = ImageLb.And(ImageR);

                            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                            CvInvoke.FindContours(ImageOTSU, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                            int Max_i = 0;
                            double Area = 0;
                            for (int i = 0; i < contours.Size; i++)
                            {
                                double a = CvInvoke.ContourArea(contours[i], false);
                                if (Area < a)
                                {
                                    Max_i = i;
                                    Area = a;
                                }
                            }
                            ImagePaddy = new Image<Gray, byte>(W, H);
                            CvInvoke.DrawContours(ImagePaddy, contours, Max_i, new MCvScalar(255), -1); // 把找到的物體畫出來補洞(-1), 畫物體輪廓(0)
                            imageBox2.Image = ImagePaddy;

                            //Bounding Box
                            using (VectorOfPoint contour = contours[Max_i])
                            {
                                // 使用 BoundingRectangle 取得框選矩形
                                Image<Bgr, byte> ImageResult = new Image<Bgr, byte>(ImageRGB.Bitmap);
                                Rectangle BoundingBox = CvInvoke.BoundingRectangle(contour);
                                //CvInvoke.Rectangle(ImageResult, BoundingBox, new MCvScalar(255, 255, 255), 2);

                                //四點座標
                                Xmin = BoundingBox.X - 5; if (Xmin < 0) { Xmin = BoundingBox.X; }
                                Xmax = BoundingBox.X + BoundingBox.Width + 5; if (Xmax >= W) { Xmax = BoundingBox.X + BoundingBox.Width; }
                                Ymin = BoundingBox.Y - 5; if (Ymin < 0) { Ymin = BoundingBox.Y; }
                                Ymax = BoundingBox.Y + BoundingBox.Height + 5; if (Ymax >= H) { Ymax = BoundingBox.Y + BoundingBox.Height; }

                                CvInvoke.Line(ImageResult, new Point(Xmin, Ymin), new Point(Xmax, Ymin), new MCvScalar(0, 0, 255, 255), 3);
                                CvInvoke.Line(ImageResult, new Point(Xmin, Ymin), new Point(Xmin, Ymax), new MCvScalar(0, 0, 255, 255), 3);
                                CvInvoke.Line(ImageResult, new Point(Xmin, Ymax), new Point(Xmax, Ymax), new MCvScalar(0, 0, 255, 255), 3);
                                CvInvoke.Line(ImageResult, new Point(Xmax, Ymin), new Point(Xmax, Ymax), new MCvScalar(0, 0, 255, 255), 3);

                                imageBox3.Image = ImageResult;
                            }

                        ////Xml
                        using (FileStream fm = new FileStream(DParth + @"\" + Path.GetFileNameWithoutExtension(fi.Name) + ".xml", FileMode.Create, FileAccess.ReadWrite))
                        {
                            XmlTextWriter writer = new XmlTextWriter(fm, Encoding.UTF8);
                            writer.Formatting = Formatting.Indented; //對齊
                            writer.WriteStartDocument(); //X
                            writer.WriteStartElement("annotation");
                            writer.WriteElementString("folder", "images");
                            writer.WriteElementString("filename", fi.Name);
                            writer.WriteElementString("path", All_Path + @"\" + fi.Name);
                            writer.WriteStartElement("source");
                            writer.WriteElementString("database", "Unknown");
                            writer.WriteEndElement(); //source
                            writer.WriteStartElement("size");
                            writer.WriteElementString("width", W.ToString());
                            writer.WriteElementString("height", H.ToString());
                            writer.WriteElementString("depth", "3");
                            writer.WriteEndElement(); //size
                            writer.WriteElementString("segmented", "0");
                            writer.WriteStartElement("object");
                            string FileName = Path.GetFileNameWithoutExtension(DParth); // 只保留資料夾名稱
                            writer.WriteElementString("name", FileName);
                            writer.WriteElementString("pose", "Unspecified");
                            writer.WriteElementString("truncated", "0");
                            writer.WriteElementString("difficult", "0");
                            writer.WriteStartElement("bndbox");
                            writer.WriteElementString("xmin", Xmin.ToString());
                            writer.WriteElementString("ymin", Ymin.ToString());
                            writer.WriteElementString("xmax", Xmax.ToString());
                            writer.WriteElementString("ymax", Ymax.ToString());
                            writer.WriteEndElement(); //bndbox
                            writer.WriteEndElement(); //object
                            writer.WriteEndElement(); //annotation
                            writer.WriteEndDocument(); //X

                            writer.Flush();
                            writer.Close();
                        }

                    }
                    }
                //}
                //catch (Exception Cai) { MessageBox.Show("Error !"); }

                MessageBox.Show("OK");
            }

            button1.Enabled = true;
            button2.Enabled = true;

        }

        void Bounding_Box_1() 
        {
            if (comboBox_DeputyName.Text != "Select" || label2.Text != "Path..." || label1.Text != "Path...")
            {




                //try
                //{
                button1.Enabled = false;
                button2.Enabled = false;

                //要檢測的資料夾路徑
                string All_Path = label1.Text; //路徑
                DirectoryInfo Path_Class = new DirectoryInfo(All_Path);
                int quantityClass = 1; //類別數量

                string[] Classes = new string[quantityClass];
                int n = 0;
                foreach (var fi in Path_Class.GetDirectories("*"))
                {
                    Classes[n] = fi.Name;
                    n++;
                }

                for (int i22 = 0; i22 < quantityClass; i22++)
                {
                    string DParth = All_Path + @"\" + Classes[i22];

                    //檢測資料夾裡的圖片數量
                    DirectoryInfo Path_Images = new DirectoryInfo(DParth);
                    int quantity = Path_Images.GetFiles("*." + comboBox_DeputyName.Text).Length; //圖片數量
                    label4.Text = "Input : " + quantity.ToString();

                    //找Paddy
                    int count = 0;
                    foreach (var fi in Path_Images.GetFiles("*." + comboBox_DeputyName.Text))
                    {
                        count++;
                        label4.Text = "Input : " + quantity.ToString() + " / Output : " + count.ToString();
                        ImageRGB = new Image<Bgr, byte>(All_Path + @"\" + Classes[i22] + @"\" + fi.Name);
                        int H = ImageRGB.Height, W = ImageRGB.Width;
                        imageBox1.Image = ImageRGB;
                        //(治禾)
                        //Image<Hls, byte> ImageHLS = new Image<Hls, byte>(ImageRGB.Bitmap);
                        //Image<Gray, byte> ImageS = new Image<Gray, byte>(ImageHLS[2].Bitmap);
                        //(冬冬)
                        ImageRGB = ImageRGB.SmoothBlur(5, 5);

                        

                        Image<Lab, byte> ImageLab = new Image<Lab, byte>(ImageRGB.Bitmap);
                        Image<Gray, byte> ImageR = new Image<Gray, byte>(ImageRGB[2].Bitmap);
                        Image<Gray, byte> ImageL = new Image<Gray, byte>(ImageLab[0].Bitmap);
                        Image<Gray, byte> ImageLb = new Image<Gray, byte>(ImageLab[2].Bitmap);
                        Image<Gray, byte> ImageOTSU = new Image<Gray, byte>(W, H);
                        //CvInvoke.Threshold(ImageL, ImageL, 140, 255, ThresholdType.Binary);
                        CvInvoke.Threshold(ImageLb, ImageLb, 80, 255, ThresholdType.Binary);
                        CvInvoke.Threshold(ImageR, ImageR, 45, 255, ThresholdType.Binary);
                        ImageOTSU = ImageLb.And(ImageR);

                        VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                        CvInvoke.FindContours(ImageOTSU, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                        int Max_i = 0;
                        double Area = 0;
                        for (int i = 0; i < contours.Size; i++)
                        {
                            double a = CvInvoke.ContourArea(contours[i], false);
                            if (Area < a)
                            {
                                Max_i = i;
                                Area = a;
                            }
                        }
                        ImagePaddy = new Image<Gray, byte>(W, H);
                        CvInvoke.DrawContours(ImagePaddy, contours, Max_i, new MCvScalar(255), -1); // 把找到的物體畫出來補洞(-1), 畫物體輪廓(0)
                        imageBox2.Image = ImagePaddy;

                        //Bounding Box
                        using (VectorOfPoint contour = contours[Max_i])
                        {
                            // 使用 BoundingRectangle 取得框選矩形
                            Image<Bgr, byte> ImageResult = new Image<Bgr, byte>(ImageRGB.Bitmap);
                            Rectangle BoundingBox = CvInvoke.BoundingRectangle(contour);
                            //CvInvoke.Rectangle(ImageResult, BoundingBox, new MCvScalar(255, 255, 255), 2);

                            //四點座標
                            Xmin = BoundingBox.X - 5; if (Xmin < 0) { Xmin = BoundingBox.X; }
                            Xmax = BoundingBox.X + BoundingBox.Width + 5; if (Xmax >= W) { Xmax = BoundingBox.X + BoundingBox.Width; }
                            Ymin = BoundingBox.Y - 5; if (Ymin < 0) { Ymin = BoundingBox.Y; }
                            Ymax = BoundingBox.Y + BoundingBox.Height + 5; if (Ymax >= H) { Ymax = BoundingBox.Y + BoundingBox.Height; }

                            CvInvoke.Line(ImageResult, new Point(Xmin, Ymin), new Point(Xmax, Ymin), new MCvScalar(0, 0, 255, 255), 3);
                            CvInvoke.Line(ImageResult, new Point(Xmin, Ymin), new Point(Xmin, Ymax), new MCvScalar(0, 0, 255, 255), 3);
                            CvInvoke.Line(ImageResult, new Point(Xmin, Ymax), new Point(Xmax, Ymax), new MCvScalar(0, 0, 255, 255), 3);
                            CvInvoke.Line(ImageResult, new Point(Xmax, Ymin), new Point(Xmax, Ymax), new MCvScalar(0, 0, 255, 255), 3);

                            imageBox3.Image = ImageResult;
                        }

                        ////Xml
                        using (FileStream fm = new FileStream(DParth + @"\" + Path.GetFileNameWithoutExtension(fi.Name) + ".xml", FileMode.Create, FileAccess.ReadWrite))
                        {
                            XmlTextWriter writer = new XmlTextWriter(fm, Encoding.UTF8);
                            writer.Formatting = Formatting.Indented; //對齊
                            writer.WriteStartDocument(); //X
                            writer.WriteStartElement("annotation");
                            writer.WriteElementString("folder", "images");
                            writer.WriteElementString("filename", fi.Name);
                            writer.WriteElementString("path", All_Path + @"\" + fi.Name);
                            writer.WriteStartElement("source");
                            writer.WriteElementString("database", "Unknown");
                            writer.WriteEndElement(); //source
                            writer.WriteStartElement("size");
                            writer.WriteElementString("width", W.ToString());
                            writer.WriteElementString("height", H.ToString());
                            writer.WriteElementString("depth", "3");
                            writer.WriteEndElement(); //size
                            writer.WriteElementString("segmented", "0");
                            writer.WriteStartElement("object");
                            string FileName = Path.GetFileNameWithoutExtension(DParth); // 只保留資料夾名稱
                            writer.WriteElementString("name", FileName);
                            writer.WriteElementString("pose", "Unspecified");
                            writer.WriteElementString("truncated", "0");
                            writer.WriteElementString("difficult", "0");
                            writer.WriteStartElement("bndbox");
                            writer.WriteElementString("xmin", Xmin.ToString());
                            writer.WriteElementString("ymin", Ymin.ToString());
                            writer.WriteElementString("xmax", Xmax.ToString());
                            writer.WriteElementString("ymax", Ymax.ToString());
                            writer.WriteEndElement(); //bndbox
                            writer.WriteEndElement(); //object
                            writer.WriteEndElement(); //annotation
                            writer.WriteEndDocument(); //X

                            writer.Flush();
                            writer.Close();
                        }

                    }
                }
                //}
                //catch (Exception Cai) { MessageBox.Show("Error !"); }

                MessageBox.Show("OK");
            }

            button1.Enabled = true;
            button2.Enabled = true;
        }

    }
}
