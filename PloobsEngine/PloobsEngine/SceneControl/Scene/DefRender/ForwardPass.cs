﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PloobsEngine.Material;
using PloobsEngine.Cameras;

namespace PloobsEngine.SceneControl
{
    internal class comparer : IComparer<IObject>
    {   
        #region IComparer<IObject> Members

        public  Vector3 CameraPosition;

        public int Compare(IObject x, IObject y)
        {
            float d1 = Vector3.Distance(CameraPosition, x.Position);
            float d2 = Vector3.Distance(CameraPosition, y.Position);
            
            if (d1 > d2)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        #endregion
    }

    public struct ForwardPassDescription
    {
        public ForwardPassDescription(bool ForwardDrawPass, bool ForwarPosDrawPass, bool DeferredPosDrawPass,bool sortByCameraDistance)
        {
            this.ForwardDrawPass = ForwardDrawPass;
            this.ForwarPosDrawPass = ForwarPosDrawPass;
            this.DeferredPosDrawPass = DeferredPosDrawPass;
            this.SortByCameraDistance = sortByCameraDistance;
        }

        public static ForwardPassDescription Default()
        {
            return new ForwardPassDescription(true, true, true,false);            
        }

        public bool ForwardDrawPass;
        public bool ForwarPosDrawPass;
        public bool DeferredPosDrawPass;
        public bool SortByCameraDistance;
    }


    public class ForwardPass : IForwardPass
    {
        ForwardPassDescription ForwardPassDescription;

        public ForwardPassDescription GetForwardPassDescription()
        {
            return ForwardPassDescription;
        }

        public void ApplyForwardPassDescription(ForwardPassDescription desc)
        {
            this.ForwardPassDescription = desc;
        }

        public ForwardPass(ForwardPassDescription ForwardPassDescription)
        {
            this.ForwardPassDescription = ForwardPassDescription;
        }
        
        comparer c = new comparer();

        public void Draw(GameTime gt, IWorld world,RenderHelper render)        
        {
            IEnumerable<IObject> list;
            if (ForwardPassDescription.DeferredPosDrawPass)
            {
                list = world.Culler.GetNotCulledObjectsList(MaterialType.DEFERRED);
                if (ForwardPassDescription.SortByCameraDistance)
                {
                    c.CameraPosition = world.CameraManager.ActiveCamera.Position;
                    list.OrderBy((a) => a, c);
                }

                foreach (IObject item in list)
                {
                    item.Material.PosDrawnPhase(gt, item, world.CameraManager.ActiveCamera, world.Lights, render);
                }
            }

            if (ForwardPassDescription.ForwardDrawPass || ForwardPassDescription.ForwarPosDrawPass)
            {
                list = world.Culler.GetNotCulledObjectsList(MaterialType.FORWARD);
                if (ForwardPassDescription.SortByCameraDistance)
                {
                    c.CameraPosition = world.CameraManager.ActiveCamera.Position;
                    list.OrderBy((a) => a, c);
                }

                if (ForwardPassDescription.ForwardDrawPass)
                {
                    foreach (IObject item in list)
                    {
                        item.Material.Drawn(gt, item, world.CameraManager.ActiveCamera, world.Lights, render);
                    }
                }

                if (ForwardPassDescription.ForwarPosDrawPass)
                {
                    foreach (IObject item in list)
                    {
                        item.Material.PosDrawnPhase(gt, item, world.CameraManager.ActiveCamera, world.Lights, render);
                    }
                }
            }

        }
        
    }
}
