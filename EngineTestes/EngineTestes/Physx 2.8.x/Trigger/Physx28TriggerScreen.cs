﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.SceneControl;
using PloobsEngine.Modelo;
using PloobsEngine.Material;
using PloobsEngine.Engine;
using Microsoft.Xna.Framework;
using PloobsEngine.Cameras;
using Microsoft.Xna.Framework.Graphics;
using PloobsEngine.Physics;
using StillDesign.PhysX;
using PloobsEngine.MessageSystem;
using PloobsEngine.Entity;

namespace EngineTestes
{

    public class ReciveMessage : IRecieveMessageEntity
    {
        public ReciveMessage()
        {
            ///Registra esta entidade
            ///REGISTER THE ENTITY
            EntityMapper.getInstance().AddEntity(this);
            ///Insere esta entidade em um canal (passara a receber mensagens deste canal)
            ///PARA ADICIONAR UMA ENTIDADE EM UM CANAL DEVE-SE ANTES REGISTRAR A ENTIDADE
            ///ADD TO THE TRIGGER CHANNEL
            EntityMapper.getInstance().AddgrouptagRecieveEntity("TriggerEvent", this);
        }
        
        #region IRecieveMessageEntity Members

        public bool HandleThisMessageType(SenderType type)
        {
            return true;
        }

        public Message mesRec = null;
        /// <summary>
        /// handle the message
        /// </summary>
        /// <param name="mes"></param>
        public void HandleMessage(Message mes)
        {
            if (mes.SenderType == PloobsEngine.MessageSystem.SenderType.EVENT && mes.Tag == "TriggerEvent")
            {
                PhysxTrigger trigger = (PhysxTrigger)mes.Content;
                mesRec = mes;

                ////HERE, or anywhere you can handle a message you handle the trigger =P !!!!!!!!!!!
            }
        }        

        #endregion

        #region IEntity Members
        long id;
        public long GetId()
        {
            return id;
        }

        public void SetId(long id)
        {
            this.id = id;
        }

        #endregion
    }

    [PloobsEngine.TestSuite.TesteVisualScreen]
    public class Physx28TriggerScreen : IScene
    {

        protected override void SetWorldAndRenderTechnich(out IRenderTechnic renderTech, out IWorld world)
        {
            PhysxPhysicWorld PhysxPhysicWorld = new PhysxPhysicWorld(new Vector3(0,-10,0));            
            world = new IWorld(PhysxPhysicWorld, new SimpleCuller());

            ForwardRenderTecnichDescription desc = ForwardRenderTecnichDescription.Default();
            desc.BackGroundColor = Color.CornflowerBlue;
            renderTech = new ForwardRenderTecnich(desc);
        }

        protected override void InitScreen(PloobsEngine.Engine.GraphicInfo GraphicInfo, PloobsEngine.Engine.EngineStuff engine)
        {
            base.InitScreen(GraphicInfo, engine);

            ///Adicionando Componente de Mensagens 
            ///Utilizado pelo Trigger
            ///Triggers uses this component
            MessageDeliver md = new MessageDeliver();
            engine.AddComponent(md);
        }


        protected override void LoadContent(GraphicInfo GraphicInfo, GraphicFactory factory ,IContentManager contentManager)
        {
            PhysxPhysicWorld PhysxPhysicWorld = World.PhysicWorld as PhysxPhysicWorld;

            base.LoadContent(GraphicInfo, factory, contentManager);
            {
                SimpleModel simpleModel = new SimpleModel(factory, "Model//cenario");

                PhysxTriangleMesh tmesh = new PhysxTriangleMesh(PhysxPhysicWorld,simpleModel, 
                    Matrix.Identity, Vector3.One);

                ForwardXNABasicShader shader = new ForwardXNABasicShader(ForwardXNABasicShaderDescription.Default());
                ForwardMaterial fmaterial = new ForwardMaterial(shader);
                IObject obj = new IObject(fmaterial, simpleModel, tmesh);
                this.World.AddObject(obj);
            }
                       

             {

                 ///Load a Model with a custom texture
                 SimpleModel sm2 = new SimpleModel(factory, "Model\\ball");
                 sm2.SetTexture(factory.CreateTexture2DColor(1, 1, Color.Aquamarine, false), TextureType.DIFFUSE);
                 ForwardXNABasicShader nd = new ForwardXNABasicShader();
                 IMaterial m =new ForwardMaterial(nd);                                 
                 
                 SphereShapeDescription SphereGeometry = new SphereShapeDescription(5f);
                 PhysxGhostObject PhysxPhysicObject = new PhysxGhostObject(new Vector3(100,100,100),Matrix.Identity, Vector3.One * 5f);
                 
                 IObject o = new IObject(m, sm2, PhysxPhysicObject);
                 this.World.AddObject(o);

                 PhysxTrigger PhysxTrigger = new PloobsEngine.Physics.PhysxTrigger(
                     SphereGeometry, Matrix.CreateTranslation(new Vector3(100, 100, 100)),
                     new PloobsEngine.Trigger.TriggerEvent("TriggerEvent", "TriggerEvent"), null, true);

                 World.AddTrigger(PhysxTrigger);

            }

            

            BallThrowPhysx28 BallThrowBullet = new BallThrowPhysx28(this.World, GraphicFactory);
            this.AttachCleanUpAble(BallThrowBullet);
            this.World.CameraManager.AddCamera(new CameraFirstPerson(GraphicInfo));

            ReciveMessage ReciveMessage = new ReciveMessage();
        }

    }
}
