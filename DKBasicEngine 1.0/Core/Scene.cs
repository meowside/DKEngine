﻿using DKBasicEngine_1_0.Core.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0.Core
{
    public class Scene : IPage
    {
        public string Name = "";

        internal readonly List<GameObject> Model;
        internal readonly List<GameObject> AllGameObjects;
        internal readonly List<GameObject> NewlyGeneratedGameObjects;
        internal readonly List<Behavior>  AllComponents;
        internal readonly List<Behavior>  NewlyGeneratedComponents;
        internal readonly List<Collider>   AllGameObjectsColliders;

        public Scene()
        {
            NewlyGeneratedGameObjects = new List<GameObject>(65536);
            AllGameObjects            = new List<GameObject>(65536);
            Model                     = new List<GameObject>(65536);
            AllComponents             = new List<Behavior>(65536);
            NewlyGeneratedComponents  = new List<Behavior>(65536);
            AllGameObjectsColliders   = new List<Collider>(65536);
        }

        public enum Mode
        {
            View,
            Edit
        }

        public void New(string Name)
        {
            this.Name = Name;
        }

        internal void Init(string path, Mode mode)
        {
            BinaryReader br;

            try
            {
                br = new BinaryReader(new FileStream(path, FileMode.Open));
            }
            catch (IOException e)
            {
                throw new Exception(path + "\nWorld wasn't found", e);
            }

            try
            {
                this.Name = br.ReadString();
                int temp_ModelCount = br.ReadInt32();
                this.Model.Clear();

                /*for (int count = 0; count < temp_ModelCount; count++)
                {
                    Model.Add(new GameObject()
                                {
                                    TypeName = br.ReadString(),
                                    X = br.ReadInt32(),
                                    Y = br.ReadInt32(),
                                    Z = br.ReadInt32()
                                });
                }*/

                switch (mode)
                {
                    case Mode.View:
                        break;

                    case Mode.Edit:
                        break;

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                throw new Exception("World loading failed", e);
            }

            br.Close();
        }

        protected internal virtual void Init()
        { }

        /*public virtual void Start() { }

        public virtual void Update()
        {
            if (PageControls.Count > 1)
            {
                if(TimeOutControls.Elapsed > TimeOut)
                    TimeOutControls.Reset();

                if(TimeOutControls.ElapsedMilliseconds == 0)
                {
                    if (Engine.Input.IsKeyPressed(ConsoleKey.UpArrow))
                    {
                        TimeOutControls.Start();

                        if (FocusSelection > 0)
                            FocusSelection--;
                    }

                    if (Engine.Input.IsKeyPressed(ConsoleKey.DownArrow))
                    {
                        TimeOutControls.Start();

                        if (FocusSelection < PageControls.Count - 1)
                            FocusSelection++;
                    }
                }
            }
        }

        public void Destroy()
        {

        }

        public void Render()
        {
            
        }*/
    }
}