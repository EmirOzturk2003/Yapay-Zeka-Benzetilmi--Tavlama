// Bu kodu �al��t�rmak i�in;
// 1. Visual Studioda yeni bir C# Masa�st� uygulamas� a��n.
// 2. Formun �zerine bir listbox ve 3 button ekleyin.
// 3. "System.Drawing.Common" NuGet paketini ekleyin.
// 4. A�a��daki kodu kopyalay�p yap��t�r�n.
// 5. buttonlar i�in onClick eventlerini ba�lay�n

using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Uygulama
{
    public partial class Form1 : Form
    {

        public delegate double Fonk(double a, double r1, double b, double r2); // Fonksiyon tipi tan�m�

        public Form1()
        {
            InitializeComponent();
        }

        //**//                      Buraya resim dosyalar�n�n yolu yaz�lacak             <-- *
        string[] resimDosyalari = ["resim1.bmp", "resim2.bmp", "hedefResim.bmp"];
        string uretilenResim = "YeniResim.bmp";  // Sizin �retti�iniz resmi buraya kaydedecek

        Cozucu coz;

        private void Baslat()
        {
            coz = new Cozucu(new Resim(resimDosyalari[0]), new Resim(resimDosyalari[1]), new Resim(resimDosyalari[2]));
            Sonuc sonuc = coz.Calistir();
            ResimYaz(sonuc.SonucResim);
            //**// Sonucu ekrana yazd�r�n                                               <-- *
            listBox1.Items.Add(sonuc.ToString()); // �rnek sonu� format�, de�i�tirebilirsiniz
                                                  // mesela yeni resmi g�sterebilirsiniz
                                                  // Bu format yeterli de�il. Daha g�zel g�sterilebilir.
        }


        #region �rnek�al��t�rma
        // �rnek �al��t�rma
        private void button1_Click(object sender, EventArgs e) // fonksiyon sonu�lar� i�in �rnek
        {
            listBox1.Items.Add("�al��t���n�z klas�r:");
            listBox1.Items.Add(Directory.GetCurrentDirectory());

            coz = new Cozucu(new Resim(resimDosyalari[0]), new Resim(resimDosyalari[1]), new Resim(resimDosyalari[2]));
            int r1 = 2, r2 = 5;
            double a = 0.25, b = 2;
            for (int i = 0; i < coz.Fonksiyonlar.Length; i++)
            {
                double sonuc = coz.Fonksiyonlar[i](a, r1, b, r2);
                listBox1.Items.Add($"f{i + 1} => {sonuc}");
            }
            
        }

        private void button2_Click(object sender, EventArgs e) // resim i�in �rnek
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Resim A�";
                dlg.Filter = "bmp dosyalar� (*.bmp)|*.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Resim okunan = new Resim(dlg.FileName);

                    // RGB de�erlerini ve resim boyutlar�n� kullan
                    // �rne�in, boyutlar� yazd�rabilirsiniz.
                    listBox1.Items.Add($"Resmin boyutlar�: {okunan.genislik}x{okunan.yukseklik}");

                    // RGB de�erlerinin bir k�sm�n� ekrana yazd�ral�m  )
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            Color pixelColor = okunan.rgbDegerleri[i, j];
                            listBox1.Items.Add($"RGB({i}, {j}) - Red: {pixelColor.R}, Green: {pixelColor.G}, Blue: {pixelColor.B}");
                        }
                    }

                    for (int i = 0; i < okunan.genislik; i++)
                    {
                        for (int j = 0; j < okunan.yukseklik; j++)
                        {
                            Color piksekrengi = okunan.rgbDegerleri[i, j];
                            // Resmi biraz de�i�tirelim                          
                            okunan.rgbDegerleri[i, j] = Color.FromArgb(255 - piksekrengi.R, 255 - piksekrengi.G, 255 - piksekrengi.B);
                        }
                    }
                    ResimYaz(okunan);
                }
            } // using (OpenFileDialog dlg = new OpenFileDialog())
        } // private void button2_Click(object sender, EventArgs e)

        private void button3_Click(object sender, EventArgs e) // ��z�c� i�in �rnek
        {
            Baslat();
        }

        #endregion �rnek�al��t�rma


        private void ResimYaz(Resim resim)
        {
            // RGB de�erlerinin bulundu�u dizi
            Color[,] rgbDegerleri = resim.rgbDegerleri;
            int width = resim.genislik;
            int height = resim.yukseklik;

            // Yeni bir bitmap olu�tur
            using (Bitmap image = new Bitmap(width, height))
            {
                // Diziyi dola� ve her pikseli resme yerle�tir
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        // Diziden renk de�erini al ve resmin ilgili pikseline yerle�tir
                        image.SetPixel(i, j, rgbDegerleri[i, j]);
                    }
                }

                // Resmi kaydet
                image.Save(uretilenResim, ImageFormat.Bmp);

                //**// kaydetmek istedi�iniz yolu �al��t�rma esnas�nda belirlemek i�in �steki sat�r� kapat�p bu b�l�m� a��n.
                /*     
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog1.FileName; 
                    image.Save(savePath, ImageFormat.Jpeg); // Veya ba�ka bir format se�ebilirsiniz
                }
                */
            }
        }

        
    }


}
