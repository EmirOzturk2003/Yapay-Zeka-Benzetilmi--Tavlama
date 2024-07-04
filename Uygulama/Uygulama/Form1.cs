// Bu kodu çalýþtýrmak için;
// 1. Visual Studioda yeni bir C# Masaüstü uygulamasý açýn.
// 2. Formun üzerine bir listbox ve 3 button ekleyin.
// 3. "System.Drawing.Common" NuGet paketini ekleyin.
// 4. Aþaðýdaki kodu kopyalayýp yapýþtýrýn.
// 5. buttonlar için onClick eventlerini baðlayýn

using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Uygulama
{
    public partial class Form1 : Form
    {

        public delegate double Fonk(double a, double r1, double b, double r2); // Fonksiyon tipi tanýmý

        public Form1()
        {
            InitializeComponent();
        }

        //**//                      Buraya resim dosyalarýnýn yolu yazýlacak             <-- *
        string[] resimDosyalari = ["resim1.bmp", "resim2.bmp", "hedefResim.bmp"];
        string uretilenResim = "YeniResim.bmp";  // Sizin ürettiðiniz resmi buraya kaydedecek

        Cozucu coz;

        private void Baslat()
        {
            coz = new Cozucu(new Resim(resimDosyalari[0]), new Resim(resimDosyalari[1]), new Resim(resimDosyalari[2]));
            Sonuc sonuc = coz.Calistir();
            ResimYaz(sonuc.SonucResim);
            //**// Sonucu ekrana yazdýrýn                                               <-- *
            listBox1.Items.Add(sonuc.ToString()); // Örnek sonuç formatý, deðiþtirebilirsiniz
                                                  // mesela yeni resmi gösterebilirsiniz
                                                  // Bu format yeterli deðil. Daha güzel gösterilebilir.
        }


        #region ÖrnekÇalýþtýrma
        // Örnek çalýþtýrma
        private void button1_Click(object sender, EventArgs e) // fonksiyon sonuçlarý için örnek
        {
            listBox1.Items.Add("Çalýþtýðýnýz klasör:");
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

        private void button2_Click(object sender, EventArgs e) // resim için örnek
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Resim Aç";
                dlg.Filter = "bmp dosyalarý (*.bmp)|*.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Resim okunan = new Resim(dlg.FileName);

                    // RGB deðerlerini ve resim boyutlarýný kullan
                    // Örneðin, boyutlarý yazdýrabilirsiniz.
                    listBox1.Items.Add($"Resmin boyutlarý: {okunan.genislik}x{okunan.yukseklik}");

                    // RGB deðerlerinin bir kýsmýný ekrana yazdýralým  )
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
                            // Resmi biraz deðiþtirelim                          
                            okunan.rgbDegerleri[i, j] = Color.FromArgb(255 - piksekrengi.R, 255 - piksekrengi.G, 255 - piksekrengi.B);
                        }
                    }
                    ResimYaz(okunan);
                }
            } // using (OpenFileDialog dlg = new OpenFileDialog())
        } // private void button2_Click(object sender, EventArgs e)

        private void button3_Click(object sender, EventArgs e) // Çözücü için örnek
        {
            Baslat();
        }

        #endregion ÖrnekÇalýþtýrma


        private void ResimYaz(Resim resim)
        {
            // RGB deðerlerinin bulunduðu dizi
            Color[,] rgbDegerleri = resim.rgbDegerleri;
            int width = resim.genislik;
            int height = resim.yukseklik;

            // Yeni bir bitmap oluþtur
            using (Bitmap image = new Bitmap(width, height))
            {
                // Diziyi dolaþ ve her pikseli resme yerleþtir
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        // Diziden renk deðerini al ve resmin ilgili pikseline yerleþtir
                        image.SetPixel(i, j, rgbDegerleri[i, j]);
                    }
                }

                // Resmi kaydet
                image.Save(uretilenResim, ImageFormat.Bmp);

                //**// kaydetmek istediðiniz yolu çalýþtýrma esnasýnda belirlemek için üsteki satýrý kapatýp bu bölümü açýn.
                /*     
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog1.FileName; 
                    image.Save(savePath, ImageFormat.Jpeg); // Veya baþka bir format seçebilirsiniz
                }
                */
            }
        }

        
    }


}
