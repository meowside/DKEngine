﻿using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using MarIO.Assets.Models;

namespace MarIO.Assets.Scripts
{
    class MainMenuSpawnScript : Script
    {
        public MainMenuSpawnScript(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            GameObject.Instantiate<Goomba>(new Vector3(270, 150, 0), new Vector3(), new Vector3(1, 1, 1));
            e.Parent.Destroy();
        }

        protected override void Start()
        {
            Goomba e = new Goomba()
            {
                Name = "Bot"
            };
            e.Transform.Position = new Vector3(270, 150, 0);
        }

        protected override void Update()
        { }
    }
}
