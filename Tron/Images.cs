using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tron
{
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource TailOrange = LoadImage("TailOrange.png");
        public readonly static ImageSource TailGreen = LoadImage("TailGreen.png");
        public readonly static ImageSource HeadOrange = LoadImage("HeadOrange.png");
        public readonly static ImageSource HeadGreen = LoadImage("HeadGreen.png");

        private static ImageSource LoadImage(string fileName)
        {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
        }
    }
}
