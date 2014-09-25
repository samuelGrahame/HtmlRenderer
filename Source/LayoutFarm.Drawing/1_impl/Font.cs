﻿
namespace LayoutFarm.Drawing
{
    public abstract class Font : System.IDisposable
    {
        public abstract string Name { get; }
        public abstract float Size { get; }
        public abstract FontStyle Style { get; }
        public abstract object InnerFont { get; }
        public abstract void Dispose();
        public abstract int Height { get; }
        public abstract System.IntPtr ToHfont();
    }

    public abstract class FontFamily
    {
       
        public abstract string Name { get; }
    }

    public abstract class StringFormat
    {
        public abstract object InnerFormat { get; }
    }
}