﻿//MIT, 2015-present, WinterDev

using System;
using PixelFarm.Drawing;
using LayoutFarm.CustomWidgets;
using LayoutFarm.UI;
namespace LayoutFarm.HtmlBoxes
{
    class CssScrollView : CssBox
    {
        CssScrollWrapper scrollView;
        //vertical scrollbar
        ScrollingRelation vscRelation;
        ScrollBar vscbar;
        //horizontal scrollbar
        ScrollingRelation hscRelation;
        ScrollBar hscbar;
        CssBox innerBox;
        HtmlHost _htmlhost;

        public CssScrollView(HtmlHost htmlhost, Css.BoxSpec boxSpec,
            IRootGraphics rootgfx)
            : base(boxSpec, rootgfx)
        {
            _htmlhost = htmlhost;
        }
        public CssBox InnerBox
        {
            get { return this.innerBox; }
        }
        public void SetInnerBox(CssBox innerBox)
        {
            if (this.innerBox != null)
            {
                return;
            }

            this.innerBox = innerBox;
            this.scrollView = new CssScrollWrapper(innerBox);
            //scroll barwidth = 10;
            bool needHScrollBar = false;
            bool needVScrollBar = false;
            int originalBoxW = (int)innerBox.VisualWidth;
            int originalBoxH = (int)innerBox.VisualHeight;
            int newW = originalBoxW;
            int newH = originalBoxH;
            const int scBarWidth = 10;
            if (innerBox.InnerContentHeight > innerBox.ExpectedHeight)
            {
                needVScrollBar = true;
                newW -= scBarWidth;
            }
            if (innerBox.InnerContentWidth > innerBox.ExpectedWidth)
            {
                needHScrollBar = true;
                newH -= scBarWidth;
            }
            innerBox.SetVisualSize(newW, newH);
            innerBox.SetExpectedSize(newW, newH);
            this.AppendToAbsoluteLayer(innerBox);
            //check if need vertical scroll and/or horizontal scroll

            //vertical scrollbar
            if (needVScrollBar)
            {
                this.vscbar = new ScrollBar(scBarWidth, needHScrollBar ? newH : originalBoxH);
                vscbar.ScrollBarType = ScrollBarType.Vertical;
                vscbar.MinValue = 0;
                vscbar.MaxValue = innerBox.VisualHeight;
                vscbar.SmallChange = 20;
                //add relation between viewpanel and scroll bar 
                vscRelation = new ScrollingRelation(vscbar.SliderBox, scrollView);
                //---------------------- 
                CssBox scBarWrapCssBox = LayoutFarm.Composers.CustomCssBoxGenerator.CreateWrapper(
                            _htmlhost,
                             this.vscbar,
                             this.vscbar.GetPrimaryRenderElement((RootGraphic)this.GetInternalRootGfx()),
                             CssBox.UnsafeGetBoxSpec(this), false);
                scBarWrapCssBox.SetLocation(newW, 0);
                this.AppendToAbsoluteLayer(scBarWrapCssBox);
            }

            if (needHScrollBar)
            {
                this.hscbar = new ScrollBar(needVScrollBar ? newW : originalBoxW, scBarWidth);
                hscbar.ScrollBarType = ScrollBarType.Horizontal;
                hscbar.MinValue = 0;
                hscbar.MaxValue = innerBox.VisualHeight;
                hscbar.SmallChange = 20;
                //add relation between viewpanel and scroll bar 
                hscRelation = new ScrollingRelation(hscbar.SliderBox, scrollView);
                //---------------------- 

                CssBox scBarWrapCssBox = LayoutFarm.Composers.CustomCssBoxGenerator.CreateWrapper(
                        _htmlhost,
                         this.hscbar,
                         this.hscbar.GetPrimaryRenderElement((RootGraphic)this.GetInternalRootGfx()),
                         CssBox.UnsafeGetBoxSpec(this), false);
                scBarWrapCssBox.SetLocation(0, newH);
                this.AppendToAbsoluteLayer(scBarWrapCssBox);
            }
        }



        class CssScrollWrapper : IScrollable
        {
            CssBox _cssbox;
            EventHandler _layoutFinish;
            EventHandler _viewportChanged;
            public CssScrollWrapper(CssBox cssbox)
            {
                this._cssbox = cssbox;
            }
            void IScrollable.SetViewport(int x, int y, object reqBy)
            {
                this._cssbox.SetViewport(x, y);
            }

            int IScrollable.ViewportX
            {
                get { return this._cssbox.ViewportX; }
            }

            int IScrollable.ViewportY
            {
                get { return this._cssbox.ViewportY; }
            }

            int IScrollable.ViewportWidth
            {
                get { return (int)this._cssbox.VisualWidth; }
            }

            int IScrollable.ViewportHeight
            {
                get { return (int)this._cssbox.VisualHeight; }
            }
            int IScrollable.InnerHeight
            {
                //content height of the cssbox
                get { return (int)_cssbox.InnerContentHeight; }
            }

            int IScrollable.InnerWidth
            {
                //content width of the cssbox
                get { return (int)_cssbox.InnerContentWidth; }
            }

            event EventHandler IScrollable.LayoutFinished
            {
                //TODO: review this
                add
                {
                    if (_layoutFinish == null)
                    {
                        _layoutFinish = value;
                    }
                    else
                    {
                        _layoutFinish += value;

                    }
                }
                remove
                {
                    if (_layoutFinish != null)
                    {
                        _layoutFinish -= value;
                    }
                }
            }
            event EventHandler IScrollable.ViewportChanged
            {
                //TODO: review this
                add
                {
                    if (_layoutFinish == null)
                    {
                        _viewportChanged = value;
                    }
                    else
                    {
                        _viewportChanged += value;

                    }
                }
                remove
                {
                    if (_viewportChanged != null)
                    {
                        _viewportChanged -= value;
                    }

                }
            }
        }
    }
}