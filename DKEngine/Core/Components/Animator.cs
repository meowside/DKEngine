﻿/*
* (C) 2017 David Knieradl
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DKEngine.Core.Components
{
    /// <summary>
    /// Used for GameObject material animation
    /// </summary>
    /// <seealso cref="DKEngine.Core.Components.Behavior" />
    /// <seealso cref="DKEngine.IAnimated" />
    public class Animator : Behavior, IAnimated
    {
        public TimeSpan CurrentAnimationTime;
        internal Dictionary<string, AnimationNode> Animations;
        private AnimationNode _current;

        public int NumberOfPlays { get; private set; } = 0;

        public AnimationNode Current
        {
            get { return _current; }
            set
            {
                if (value != _current)
                {
                    _current = value;
                    Parent.Model = _current.Animation;
                    NumberOfPlays = 0;
                    CurrentAnimationTime = new TimeSpan(0);
                }
            }
        }

        public int AnimationState
        {
            get
            {
                return (int)(CurrentAnimationTime.TotalMilliseconds / Parent.Model.DurationPerFrame % Parent.Model.Frames);
            }
        }

        public Animator(GameObject Parent)
            : base(Parent)
        {
            this.CurrentAnimationTime = new TimeSpan(0);
            this.Animations = new Dictionary<string, AnimationNode>();

            this.Name = string.Format("{0}_{1}", Parent.Name, nameof(Animator));
        }

        /// <summary>
        /// Adds the animation.
        /// </summary>
        /// <param name="Name">The animation node name.</param>
        /// <param name="Source">The source material for animation node.</param>
        public void AddAnimation(string Name, Material Source)
        {
            Animations.Add(Name, new AnimationNode(Name, Source));
            if (Animations.Count == 1)
            {
                Play(Animations.ElementAt(0).Key);
            }
        }

        /// <summary>
        /// Adds the animation.
        /// </summary>
        /// <param name="Name">The animation node name.</param>
        /// <param name="MaterialKey">The material key to search for material.</param>
        public void AddAnimation(string Name, string MaterialKey)
        {
            Animations.Add(Name, new AnimationNode(Name, Database.GetGameObjectMaterial(MaterialKey)));
            if (Animations.Count == 1)
            {
                Play(Animations.ElementAt(0).Key);
            }
        }

        /// <summary>
        /// Plays the specified animation name.
        /// </summary>
        /// <param name="AnimationName">Name of the animation.</param>
        public void Play(string AnimationName)
        {
            if (AnimationName != Current?.Name)
            {
                AnimationNode Result;

                try
                {
                    Result = Animations[AnimationName];
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Animation \"{0}\"not found\n{1}", AnimationName, e);
                    return;
                }

                Current = Result;
            }
        }

        protected internal override void Update()
        {
            if (Parent?.Model?.Frames > 1)
            {
                CurrentAnimationTime = CurrentAnimationTime.Add(new TimeSpan(0, 0, 0, 0, (int)(Engine.DeltaTime * 1000)));

                if (CurrentAnimationTime.TotalMilliseconds > Parent.Model.Duration)
                {
                    CurrentAnimationTime = CurrentAnimationTime.Subtract(new TimeSpan(0, 0, 0, 0, Parent.Model.Duration));
                    NumberOfPlays++;
                }
            }
        }

        protected internal override void Start()
        { }

        public override void Destroy()
        {
            Engine.UpdateEvent -= UpdateHandle;

            Parent = null;
            UpdateHandle = null;
        }
    }
}