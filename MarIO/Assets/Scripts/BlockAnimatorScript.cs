﻿using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using DKEngine;

namespace MarIO.Assets.Scripts
{
    class BlockAnimatorScript : Script
    {
        float AnimationHeight = 2;
        float AnimationSpeed = 20;

        public BlockAnimatorScript(GameObject Parent)
            : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {

        }

        protected override void Start()
        {

        }

        protected override void Update()
        {
            if(Shared.BlocksToUpdate.Count > 0)
            {
                for(int i = 0; i < Shared.BlocksToUpdate.Count; i++)
                {
                    float StartBlockY = Shared.BlocksStartPositions[i];
                    Block CurrentBlock = Shared.BlocksToUpdate[i];

                    if(CurrentBlock.State == Block.CollisionState.Up && StartBlockY - AnimationHeight < CurrentBlock.Transform.Position.Y)
                    {
                        CurrentBlock.Transform.Position -= new Vector3(0, Engine.DeltaTime * AnimationSpeed, 0);

                        if(CurrentBlock.Transform.Position.Y <= StartBlockY - AnimationHeight)
                        {
                            CurrentBlock.State = Block.CollisionState.Down;
                            CurrentBlock.Transform.Position += new Vector3(0, CurrentBlock.Transform.Position.Y - (StartBlockY + AnimationHeight), 0);
                        }
                    }
                    else if(CurrentBlock.State == Block.CollisionState.Down && CurrentBlock.Transform.Position.Y < StartBlockY)
                    {
                        CurrentBlock.Transform.Position += new Vector3(0, Engine.DeltaTime * AnimationSpeed, 0);

                        if(CurrentBlock.Transform.Position.Y > StartBlockY)
                        {
                            CurrentBlock.State = Block.CollisionState.Stay;
                            CurrentBlock.Transform.Position -= new Vector3(0, StartBlockY - CurrentBlock.Transform.Position.Y, 0);
                            
                            Shared.BlocksStartPositions.RemoveAt(i);
                            Shared.BlocksToUpdate.RemoveAt(i);

                            i--;
                        }
                    }
                }
            }
        }
    }
}