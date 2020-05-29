﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asteroid.src.entities;
using Asteroid.src.utils;
using Asteroid.src.network;
using Box2DX.Common;
using Asteroid.src.input;

namespace Asteroid.src.worlds
{
    class SpaceWorld : BaseWorld
    {
        Vector2 virtualSize;
        Vector2 realSize;

        public SpaceWorld(Vector2 virtualSize, Vector2 realSize)
        {
            this.virtualSize = virtualSize;
            this.realSize = realSize;
           
        }

        public override void Initialize(ActionGeneratorsManager inputManager)
        {
            this.inputManager = inputManager;

            //границы
            AddBorders();
            //обработка ввода и генерация IRemoteAction'ов
            AddInputHandlers();
            //выполнение IRemoteAction'ов
            AddActionExecutors();
        }

        void AddBorders()
        {

            AddEntity(
                 new InvisibleBorder(
                     Translator.VirtualToBox2DWorld(0, virtualSize.Y),
                     Translator.VirtualToBox2DWorld(virtualSize.X, virtualSize.Y)
                     )
                 );
            AddEntity(
                new InvisibleBorder(
                    Translator.VirtualToBox2DWorld(0, 0),
                    Translator.VirtualToBox2DWorld(virtualSize.X, 0)
                    )
                );
            AddEntity(
                new InvisibleBorder(
                    Translator.VirtualToBox2DWorld(0, 0),
                    Translator.VirtualToBox2DWorld(0, virtualSize.Y)
                    )
                );
            AddEntity(
                new InvisibleBorder(
                    Translator.VirtualToBox2DWorld(virtualSize.X, 0),
                    Translator.VirtualToBox2DWorld(virtualSize.X, virtualSize.Y)
                    )
                );
        }
        void AddInputHandlers()
        {
            inputManager.AddMouseClickListener(new input.MouseClickListener((MouseState state) =>
            {
                var screenMP = Mouse.GetState().Position;
                return new SpawnBoxAction()
                {
                    Position
                    = new Vec2(
                        Translator.realXtoBox2DWorld(screenMP.X),
                        Translator.realYtoBox2DWorld(screenMP.Y)
                        )
                };
            }));
        }
        void AddActionExecutors()
        {
            AddExecutor(typeof(SpawnBoxAction), (IRemoteAction _action) =>
            {
                var action = (SpawnBoxAction)_action;
                AddEntity(
                   new Box(
                       action.Position,
                       Translator.virtualXtoBox2DWorld(50),
                       Translator.virtualXtoBox2DWorld(50)
                ));
            });
        }


    }
}
