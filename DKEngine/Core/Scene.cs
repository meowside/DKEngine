﻿using DKEngine.Core.Components;
using System.Collections.Generic;

namespace DKEngine.Core
{
    /// <summary>
    /// DKEngine library scene
    /// </summary>
    /// <seealso cref="DKEngine.IPage" />
    public abstract class Scene : IPage
    {
        public string Name = "";

        internal Camera BaseCamera;

        internal readonly Dictionary<string, Component> AllComponents;
        internal readonly Dictionary<string, int> ComponentCount;
        //internal readonly Dictionary<string, GameObject> AllGameObjects;

        internal readonly List<GameObject> Model;
        internal readonly List<Behavior> AllBehaviors;
        internal readonly List<Collider> AllGameObjectsColliders;

        internal readonly Stack<Component> NewlyGeneratedComponents;
        internal readonly Stack<Behavior> NewlyGeneratedBehaviors;

        internal readonly Stack<GameObject> GameObjectsToAddToRender;
        internal readonly Stack<GameObject> GameObjectsAddedToRender;

        internal readonly List<GameObject> DestroyObjectAwaitList;

        internal object[] argsPreLoad;
        internal object[] argsPostLoad;

        public Scene()
        {
            AllComponents = new Dictionary<string, Component>(0xFFFF);
            ComponentCount = new Dictionary<string, int>(0xFFFF);

            AllBehaviors = new List<Behavior>(0xFFFF);
            Model = new List<GameObject>(0xFFFF);
            AllGameObjectsColliders = new List<Collider>(0xFFFF);

            NewlyGeneratedComponents = new Stack<Component>(0xFFFF);
            NewlyGeneratedBehaviors = new Stack<Behavior>(0xFFFF);

            GameObjectsToAddToRender = new Stack<GameObject>(0xFFFF);
            GameObjectsAddedToRender = new Stack<GameObject>(0xFFFF);

            DestroyObjectAwaitList = new List<GameObject>(0xFFFF);
        }

        /// <summary>
        /// Initializes model of Scene.
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Sets the specified arguments.
        /// </summary>
        /// <param name="args">The arguments</param>
        public virtual void Set(params object[] args)
        { }

        /// <summary>
        /// Unloads this instance.
        /// </summary>
        public abstract void Unload();

        public static T Find<T>(string name)
            where T : Scene
        {
            return (T)Database.GetScene(name);
        }
    }
}