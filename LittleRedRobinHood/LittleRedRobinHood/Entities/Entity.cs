﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleRedRobinHood.Entities
{
    class Entity
    {
        public readonly int entityID;
        public readonly bool isPlayer;
        public readonly bool isCollide;
        public readonly bool isProjectile;
        public readonly bool isShackle;
        public readonly bool isPatrol;
        public readonly bool isHoming;
        public readonly bool isText;
        public Entity(int id, bool player, bool collide, bool projectile, bool shackled, bool patrol, bool homing, bool text)
        {
            this.entityID = id;
            this.isPlayer = player;
            this.isCollide = collide;
            this.isProjectile = projectile;
            this.isShackle = shackled;
            this.isPatrol = patrol;
            this.isHoming = homing;
            this.isText = text;
        }
        /*
        public int getID()
        {
            return this.entityID;
        }
        public bool isPlayer()
        {
            return this.isplayer;
        }
        public bool isCollide()
        {
            return this.iscollide;
        }
        public bool isProjectile()
        {
            return this.isprojectile;
        }
        public bool isShackled()
        {
            return this.isshackled;
        }
        public bool isPatrol()
        {
            return this.ispatrol;
        }
        public bool isHoming()
        {
            return this.ishoming;
        }
        public bool isText()
        {
            return this.istext;
        }
        public bool isStageEnd()
        {
            return this.isstageEnd;
        }
         * */
    }
}