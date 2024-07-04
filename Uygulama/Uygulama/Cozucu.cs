using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Uygulama.Cozucu;
using static Uygulama.Form1;

namespace Uygulama
{
    internal class Cozucu
    {
        // resim1 + resim2 = resim3 hedefliyoruz
        Resim resim1, resim2, resim3;
        Resim hesaplananResim; // üretilem resim

        public Cozucu(Resim r1, Resim r2, Resim r3) 
        {
            this.resim1 = r1;
            this.resim2 = r2;
            this.resim3 = r3;
            resim1.HesapModunaGec();
            resim2.HesapModunaGec();
            resim3.HesapModunaGec();
        }

        //**// Kodunuzu buraya yazın            <-- *
        public List<FonkveParametreler> FonksiyonListesi; // kullanacağınız fonksiyonları burada saklayın

        // Algoritmanızı uygulayacağınız kodu buraya yazın
        // ihtiyaca göre başka fonksiyonlar veya classlar eklenebilir.
        // dönüş tipi ve parametreleri ihtiyacınıza göre belirleyin.
        public Sonuc Calistir()
        {
            // Resim hesaplananResim; // üretilem resim (yukarıda tanımlandı)
            FonksiyonListesi = RastgeleCozumOlustur(5); // Rastgele başlangıç çözümü oluşturuyor. fonksiyon sayısını artırıp azaltabilirsiniz.

            //**// algoritmanızın yer alacağı bölüm
            //          yeni çözüm üretmek için "FonksiyonListesi"ni güncelleyin.
            //          Yeni resim üretmek için "BütünlesikIslemYap" kullanın
            //          Hata değerini hesaplamak için MaliyetHesap(hesaplananResim, resim3); kullanın

            hesaplananResim = BütünlesikIslemYap(resim1, resim2, FonksiyonListesi, true); //**// Örnek çok fonksiyoından resim oluşturma




            double HataDegeri = MaliyetHesap(hesaplananResim, resim3);
            sonIslemler(); // orjinal ve hesaplanan resimleri resim moduna dönüştürür. resimler bundan sonra resim olarak kaydedilebilir.

            //**// bir dönüş değeri olacaksa bu bölümde oluşturulabilir.
            // Maliyet / Hata değeri
            // Hangi fonksiyonlar hangi parametrelerle kullanıldı
            // üretilen resim
            // Eğer algoritmanın ilerlemesi esnasında hata değerlerinin azaldığını göstermek istiyorsanız Sonuc yapısına başka bir dizi ekleyebilirsiniz.

            return new Sonuc(FonksiyonListesi, hesaplananResim, HataDegeri);
        }
        //**// Kodunuzu buraya yazın {son}      <-- *

        #region fonksiyonlar
        // Kullanacağımız fonksiyonlar
        static public double f1(double a, double r1, double b, double r2) // a*r1 + b*r2
        { return a * r1 + b * r2; }

        static public double f2(double a, double r1, double b, double r2)  // a*r1 * b*r2  
        { return a * r1 * b * r2; }

        static public double f3(double a, double r1, double b, double r2) // r1^a
        { return Math.Pow(r1, a); }

        static public double f4(double a, double r1, double b, double r2) // r2^b
        { return Math.Pow(r2, b); }

        static public double f5(double a, double r1, double b, double r2) // a * e^r1
        { return a * Math.Exp(r1); }

        static public double f6(double a, double r1, double b, double r2) // b * e^r2
        { return b * Math.Exp(r2); }

        static public double f7(double a, double r1, double b, double r2)  // a * ln r1 = ln r1^a
        { return a * Math.Log(r1); }

        static public double f8(double a, double r1, double b, double r2) // b * ln r2 = ln r2^b
        { return b * Math.Log(r2); }

        static public double f9(double a, double r1, double b, double r2) // sin (a * r1)
        { return Math.Sin(a * r1); }

        static public double f10(double a, double r1, double b, double r2) // sin (b * r2)
        { return Math.Sin(b * r2); }

        static public double f11(double a, double r1, double b, double r2) // cos (a * r1)
        { return Math.Cos(a * r1); }

        static public double f12(double a, double r1, double b, double r2) // cos (b * r2)
        { return Math.Cos(b * r2); }

        static public double f13(double a, double r1, double b, double r2) // a * Mutlak r1
        { return a * Math.Abs(r1); }

        static public double f14(double a, double r1, double b, double r2) // b * Mutlak r2
        { return b * Math.Abs(r2); }

        static public double f15(double a, double r1, double b, double r2) // a * relu r1
        { return (r1 > 0) ? a * r1 : 0; }

        static public double f16(double a, double r1, double b, double r2) // b * relu r2
        { return (r2 > 0) ? b * r2 : 0; }

        static public double f17(double a, double r1, double b, double r2) // max
        { return Math.Max(r1, r2); }

        static public double f18(double a, double r1, double b, double r2) // min
        { return Math.Min(r1, r2); }

        static public double f19(double a, double r1, double b, double r2) // a * sigmoid r1
        { return a / (1 + Math.Exp(-r1)); }

        static public double f20(double a, double r1, double b, double r2) // b * sigmoid r2
        { return b / (1 + Math.Exp(-r2)); }

        // Fonksiyonların bir dizide saklanması
        public Fonk[] Fonksiyonlar = new Fonk[] { f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11, f12, f13, f14, f15, f16, f17, f18, f19, f20 };

        #endregion fonksiyonlar

        // "fonk" fonksiyonunu r1 ve r2 resimlerinin tüm piksellerine uygulayarak 3'üncü resmi üretir ve sonuç olarak döndürür
        private Resim islemYap(Fonk fonk, double a, Resim r1, double b, Resim r2)
        {
            Resim sonuc = new Resim(r1.genislik, r1.yukseklik);
            
            for (int x = 0; x < r1.genislik; x++)
                for (int y = 0; y < r1.yukseklik; y++)
                    for (int c = 0; c < 3; c++)
                    {
                        sonuc.hesap[x, y, c] = fonk(a, r1.hesap[x, y, c], b, r2.hesap[x, y, c]);
                    }

            return sonuc;
        }

        // islemYap() fonksiyonunun benzeri
        // farkı: fonksiyonları resimlere uygularken resmin farklı noktalarına a ve b parametreleri koordinatlarına göre çarpanla hesaplar.
        private Resim koordinatliIslemYap(Fonk fonk, double a, Resim r1, double b, Resim r2)
        {
            Resim sonuc = new Resim(r1.genislik, r1.yukseklik);
            
            //for (int x = 0; x < r1.genislik; x++)
            Parallel.For(0, sonuc.genislik, x =>
            {
                for (int y = 0; y < r1.yukseklik; y++)
                    for (int c = 0; c < 3; c++)
                    {
                        sonuc.hesap[x, y, c] = fonk(Math.Log(x) * a, r1.hesap[x, y, c], Math.Log(y) * b, r2.hesap[x, y, c]);
                    }
            });

            return sonuc;
        }

        private Resim BütünlesikIslemYap(Resim r1, Resim r2, List<FonkveParametreler> aday, bool koordinatli)
        {
            List<Resim> resimler = new List<Resim>();
            if (koordinatli)
                for (int k = 0; k < aday.Count; k++)
                    resimler.Add(koordinatliIslemYap(Fonksiyonlar[aday[k].fonksiyon], aday[k].a, r1, aday[k].b, r2));
            else
                for (int k = 0; k < aday.Count; k++)
                    resimler.Add(islemYap(Fonksiyonlar[aday[k].fonksiyon], aday[k].a, r1, aday[k].b, r2));

            ResimleriKontrolEt(ref resimler);

            return CokluResimBirlestir(resimler);
        }

        private Resim CokluResimBirlestir(List<Resim> resimler)
        {
            Resim sonuc = new Resim(resimler[0].genislik, resimler[0].yukseklik);
            
            //for (int x = 0; x < sonuc.genislik; x++)
            Parallel.For(0, sonuc.genislik, x => {
                for (int y = 0; y < sonuc.yukseklik; y++)
                    for (int c = 0; c < 3; c++)
                    {
                        // sonuc.hesap[x, y, c] = resimler.Sum(h => h.hesap[x, y, c]); //**// Toplama
                        sonuc.hesap[x, y, c] = resimler.Sum(h => h.hesap[x, y, c]) / resimler.Count; //**// Ortalama
                                                                                                     // veya istediğiniz şekilde değiştirebilirsiniz.
                    }
            });
            
            return sonuc;
        }

        // Bazı resimlerde sonsuz sayı veya sıfıra bölme
        // gibi nedenlerle uygunsuz sayılar oluşmuşsa bunları ekarte etmek için.
        private void ResimleriKontrolEt(ref List<Resim> resimler)
        {
            foreach (Resim resim in resimler)
            {
                Parallel.For(0, resim.genislik, x =>
                {
                    for (int y = 0; y < resim.yukseklik; y++)
                        for (int c = 0; c < 3; c++)
                        {
                            if (!double.IsNormal(resim.hesap[x, y, c])) resim.hesap[x, y, c] = 0.0;
                        }
                }); 
            }
        }

        private List<FonkveParametreler> RastgeleCozumOlustur(int FonksiyonSayisi)
        {
            List<FonkveParametreler> rastgeleCozum = new List<FonkveParametreler>();
            Random rnd = new Random();
            for (int i = 0; i < FonksiyonSayisi; i++)
                rastgeleCozum.Add(new FonkveParametreler(rnd.Next(20), 2*rnd.NextDouble(), 2*rnd.NextDouble()));

            return rastgeleCozum;
        }

        void sonIslemler()
        {
            //resim1.ResimModunaGec();
            //resim2.ResimModunaGec();  // Bu 3 resimin tekrar resim moduna geçmesine gerek yok, ihtiyaç olursa açarsınız.
            //resim3.ResimModunaGec();
            hesaplananResim.ResimModunaGec();
        }

        // ürertilen resimleri toplayarak birleştirir
        private Resim islemleriTopla(Resim r1, Resim r2)
        {
            Resim sonuc = new Resim(r1.genislik, r1.yukseklik);
            for (int x = 0; x < r1.genislik; x++)
                for (int y = 0; y < r1.yukseklik; y++)
                    for (int c = 0; c < 3; c++)
                    {
                        sonuc.hesap[x, y, c] = r1.hesap[x, y, c] + r2.hesap[x, y, c];
                    }

            return sonuc;
        }

        // İki resim arasındaki farkı resim üzerinden hesaplar
        private double MaliyetResim(Resim resim1, Resim resim2)
        {
            if ((resim1.genislik != resim2.genislik) || (resim1.yukseklik != resim2.yukseklik))
            {
                throw new Exception("Hata: Resimlerin boyutları aynı olmalıdır.");
            }
            double HataDegeri = 0;
            for (int y = 0; y < resim1.yukseklik; y++)
            {
                for (int x = 0; x < resim1.genislik; x++)
                {
                    HataDegeri += Math.Abs(resim1.rgbDegerleri[x, y].R - resim2.rgbDegerleri[x, y].R)
                        + Math.Abs(resim1.rgbDegerleri[x, y].G - resim2.rgbDegerleri[x, y].G)
                        + Math.Abs(resim1.rgbDegerleri[x, y].B - resim2.rgbDegerleri[x, y].B);
                }
            }
            return HataDegeri;
        }

        // İki resim arasındaki farkı hesaplama yapısı üzerinden hesaplar
        private double MaliyetHesap(Resim resim1, Resim resim2)
        {
            if ((resim1.genislik != resim2.genislik) || (resim1.yukseklik != resim2.yukseklik))
            {
                throw new Exception("Hata: Resimlerin boyutları aynı olmalıdır.");
            }
            double HataDegeri = 0;
            for (int x = 0; x < resim1.genislik; x++)
                for (int y = 0; y < resim1.yukseklik; y++)
                    for (int c = 0; c < 3; c++)
                        HataDegeri += Math.Abs(resim1.hesap[x, y, c] - resim2.hesap[x, y, c]);

            return HataDegeri;
        }
    } // internal class Cozucu

    public class Resim
    {
        public int genislik;
        public int yukseklik;

        public Color[,] rgbDegerleri;
        public double[,,] hesap;

        //public Resim() { }

        public Resim(int genislik, int yukseklik)
        {
            this.genislik = genislik;
            this.yukseklik = yukseklik;
            rgbDegerleri = new Color[genislik, yukseklik];
            hesap = new double[genislik, yukseklik, 3];
        }

        public Resim(string dosyaYolu)
        {
            // Resmi yükle
            using (Bitmap resim = new Bitmap(dosyaYolu))
            {
                // Resmin genişliği ve yüksekliğini al
                genislik = resim.Width;
                yukseklik = resim.Height;

                // RGB değerlerini saklamak için dizi oluştur
                rgbDegerleri = new Color[genislik, yukseklik];

                // Resimdeki tüm pikselleri dolaş
                for (int i = 0; i < genislik; i++)
                {
                    for (int j = 0; j < yukseklik; j++)
                    {
                        // Pikselin rengini al
                        Color pikselRengi = resim.GetPixel(i, j);

                        // Rengi diziye ata
                        rgbDegerleri[i, j] = pikselRengi;
                    }
                }

            } // using (Bitmap image = new Bitmap(imagePath))

            hesap = new double[genislik, yukseklik, 3];
            HesapModunaGec();
        }

        public void HesapModunaGec()
        {
            //hesap = new double[genislik, yukseklik, 3];
            if (genislik * yukseklik == 0) throw new Exception("Resim Boş");
            for (int x = 0; x < genislik; x++)
                for (int y = 0; y < yukseklik; y++)
                {
                    hesap[x, y, 0] = rgbDegerleri[x, y].R;
                    hesap[x, y, 1] = rgbDegerleri[x, y].G;
                    hesap[x, y, 2] = rgbDegerleri[x, y].B;
                }
        }
        public void ResimModunaGec()
        {
            if (genislik * yukseklik == 0) throw new Exception("Resim Boş");
            for (int x = 0; x < genislik; x++)
                for (int y = 0; y < yukseklik; y++)
                {
                    rgbDegerleri[x, y] = Color.FromArgb(
                        Convert.ToInt32(hesap[x, y, 0]) % 256,
                        Convert.ToInt32(hesap[x, y, 1]) % 256,
                        Convert.ToInt32(hesap[x, y, 2]) % 256);
                }
        }
        public void ResimSonucuEkle(Resim digerResim)
        {
            if (genislik * yukseklik == 0) throw new Exception("Resim Boş");
            if (digerResim.genislik * digerResim.yukseklik == 0) throw new Exception("Eklenecek Resim Boş");
            for (int x = 0; x < genislik; x++)
                for (int y = 0; y < yukseklik; y++)
                    for (int c = 0; c < 3; c++)
                        hesap[x, y, c] += digerResim.hesap[x, y, c];
        }

    }

    public class FonkveParametreler
    {
        public int fonksiyon; // fonksiyon numarası
        public double a;
        public double b;

        public FonkveParametreler() { }
        public FonkveParametreler(int f, double aa, double bb)
        {
            fonksiyon = f;
            a = aa;
            b = bb;
        }
    }

    public class Sonuc
    {
        // int: Fonksiyon No, Parametreler (a ve b)
        public List<FonkveParametreler> Fonksiyonlar = new List<FonkveParametreler>();
        public Resim SonucResim;
        public double HataDegeri;
        public string Adi;
        private static int Sayac = 0;

        public Sonuc()
        {
            Adi = "Resim" + (Sayac++).ToString();
        }
        public Sonuc(List<FonkveParametreler> fonksiyonlar, Resim sonucResim, double hataDegeri)
        {
            Fonksiyonlar = fonksiyonlar;
            SonucResim = sonucResim;
            HataDegeri = hataDegeri;
            Adi = "Resim" + (Sayac++).ToString();
        }

        public override string ToString() 
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Adi);
            foreach (var f in Fonksiyonlar) sb.AppendLine($"f{f.fonksiyon}({f.a:0.0000}, {f.b:0.0000}");
            sb.AppendLine($"Hata Değeri = {HataDegeri}");
            return sb.ToString();
        }
    }

}
