﻿//Apache2, 2014-present, WinterDev

using System;
using System.Collections.Generic;

namespace LayoutFarm
{
    public static class HtmlHostCreatorHelper
    {
        public static HtmlBoxes.HtmlHost CreateHtmlHost(AppHost appHost,
            EventHandler<ContentManagers.ImageRequestEventArgs> imageReqHandler,
            EventHandler<ContentManagers.TextRequestEventArgs> textReq)
        {
            HtmlBoxes.HtmlHost htmlhost = new HtmlBoxes.HtmlHost();
            htmlhost.SetRootGraphics(appHost.RootGfx);

            List<HtmlBoxes.HtmlContainer> htmlContUpdateList = new List<HtmlBoxes.HtmlContainer>();
            appHost.RootGfx.ClearingBeforeRender += (s, e) =>
            {

                //1.
                htmlhost.ClearUpdateWaitingCssBoxes();
                 
                int j = htmlContUpdateList.Count;
                for (int i = 0; i < j; ++i)
                {

                    var htmlCont = htmlContUpdateList[i];
                    htmlCont.RefreshDomIfNeed();
                    htmlCont.IsInUpdateQueue = false;
                }
                htmlContUpdateList.Clear();
            };
            htmlhost.RegisterCssBoxGenerator(new LayoutFarm.CustomWidgets.MyCustomCssBoxGenerator(htmlhost));
            htmlhost.AttachEssentailHandlers(imageReqHandler, textReq);
            htmlhost.SetHtmlContainerUpdateHandler(htmlCont =>
            {
                if (!htmlCont.IsInUpdateQueue)
                {
                    htmlCont.IsInUpdateQueue = true;
                    htmlContUpdateList.Add(htmlCont);
                }
            });

            PaintLab.Svg.VgResourceIO._vgIODelegate = RequestImgAyncs;

            return htmlhost;
        }


        static ContentManagers.ImageContentManager _contentMx;
        static void RequestImgAyncs(LayoutFarm.ImageBinder binder, PaintLab.Svg.SvgRenderElement imgRun, object requestFrom)
        {
            if (_contentMx == null)
            {
                _contentMx = new ContentManagers.ImageContentManager();
                _contentMx.ImageLoadingRequest += (s, e) =>
                {
                    e.SetResultImage(LoadImgForSvgElem(e.ImagSource));
                };
            }
            _contentMx.AddRequestImage(binder);
        }
        static PixelFarm.Drawing.Image LoadImgForSvgElem(string imgName)
        {
            //temp fix
            if (imgName == "html32.png")
            {
                //TODO: review document root
                imgName = @"D:\projects\HtmlRenderer\Source\Test8_HtmlRenderer.Demo\Samples\SvgSamples\" + imgName;
            }
            //handle resource load
            if (!System.IO.File.Exists(imgName))
            {

            }


            using (System.Drawing.Bitmap gdiBmp = new System.Drawing.Bitmap(imgName))
            {
                int w = gdiBmp.Width;
                int h = gdiBmp.Height;

                var bmpData = gdiBmp.LockBits(new System.Drawing.Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                int[] buffer = new int[w * h];
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, buffer, 0, w * h);
                gdiBmp.UnlockBits(bmpData);

                return new PixelFarm.CpuBlit.ActualBitmap(gdiBmp.Width, gdiBmp.Height, buffer);
            }
           
        }
    }
}