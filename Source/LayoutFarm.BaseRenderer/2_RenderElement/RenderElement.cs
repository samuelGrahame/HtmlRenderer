﻿// 2015,2014 ,Apache2, WinterDev
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PixelFarm.Drawing;
using LayoutFarm.RenderBoxes;
namespace LayoutFarm
{


    public abstract partial class RenderElement : IRenderElement
    {


        RootGraphic rootGfx;
        IParentLink parentLink;
        object controller;
        int propFlags;

        public RenderElement(RootGraphic rootGfx, int width, int height)
        {
            this.b_width = width;
            this.b_height = height;
            this.rootGfx = rootGfx;
#if DEBUG
            dbug_totalObjectId++;
            dbug_obj_id = dbug_totalObjectId;
            //this.dbug_SetFixedElementCode(this.GetType().Name);
#endif
        }
        public RootGraphic Root
        {
            get { return this.rootGfx; }
        }



        //==============================================================
        //controller-listener
        public object GetController()
        {
            return controller;
        }
        public void SetController(object controller)
        {
            this.controller = controller;
        }
        public bool TransparentForAllEvents
        {
            get
            {
                return (propFlags & RenderElementConst.TRANSPARENT_FOR_ALL_EVENTS) != 0;
            }
            set
            {
                propFlags = value ?
                       propFlags | RenderElementConst.TRANSPARENT_FOR_ALL_EVENTS :
                       propFlags & ~RenderElementConst.TRANSPARENT_FOR_ALL_EVENTS;

            }
        }

        //==============================================================
        //parent/child ...
        public bool HasParent
        {
            get
            {
                return this.parentLink != null;
            }
        }
        public virtual void ClearAllChildren()
        {

        }
        public IParentLink ParentLink
        {
            get
            {
                return parentLink;
            }
        }
        public virtual RenderElement ParentRenderElement
        {
            get
            {
                if (parentLink == null)
                {
                    return null;
                }
                return parentLink.ParentRenderElement;
            }
        }
        public static void RemoveParentLink(RenderElement childElement)
        {
            childElement.parentLink = null;
        }
        public static void SetParentLink(RenderElement childElement, IParentLink parentLink)
        {
            childElement.parentLink = parentLink;
        }
        public bool MayHasChild
        {
            get { return (propFlags & RenderElementConst.MAY_HAS_CHILD) != 0; }
            protected set
            {
                propFlags = value ?
                      propFlags | RenderElementConst.MAY_HAS_CHILD :
                      propFlags & ~RenderElementConst.MAY_HAS_CHILD;
            }
        }
        public bool MayHasViewport
        {
            get { return (propFlags & RenderElementConst.MAY_HAS_VIEWPORT) != 0; }
            protected set
            {
                propFlags = value ?
                      propFlags | RenderElementConst.MAY_HAS_VIEWPORT :
                      propFlags & ~RenderElementConst.MAY_HAS_VIEWPORT;
            }
        }
        public virtual RenderElement FindOverlapedChildElementAtPoint(RenderElement afterThisChild, Point point)
        {
            return null;
        }
        public virtual void ChildrenHitTestCore(HitChain hitChain)
        {
        }

        //==============================================================
        public bool Visible
        {
            get
            {
                return ((propFlags & RenderElementConst.HIDDEN) == 0);
            }

        }
        public void SetVisible(bool value)
        {

            propFlags = value ?
                propFlags & ~RenderElementConst.HIDDEN :
                propFlags | RenderElementConst.HIDDEN;

            if (parentLink != null)
            {
                this.InvalidateGraphics();
            }
        }
        public bool IsBlockElement
        {
            get
            {
                return ((propFlags & RenderElementConst.IS_BLOCK_ELEMENT) == RenderElementConst.IS_BLOCK_ELEMENT);
            }
            set
            {
                propFlags = value ?
                     propFlags | RenderElementConst.IS_BLOCK_ELEMENT :
                     propFlags & ~RenderElementConst.IS_BLOCK_ELEMENT;
            }
        }


        //==============================================================


        //internal methods  
        internal bool IsInRenderChain
        {
            get
            {
                return (propFlags & RenderElementConst.IS_IN_RENDER_CHAIN) != 0;
            }
            set
            {
                propFlags = value ?
                   propFlags | RenderElementConst.IS_IN_RENDER_CHAIN :
                   propFlags & ~RenderElementConst.FIRST_ARR_PASS;

            }
        }

        internal bool FirstArrangementPass
        {

            get
            {
                return (propFlags & RenderElementConst.FIRST_ARR_PASS) != 0;
            }
            set
            {
                propFlags = value ?
                   propFlags | RenderElementConst.FIRST_ARR_PASS :
                   propFlags & ~RenderElementConst.FIRST_ARR_PASS;
            }
        }



    }
}