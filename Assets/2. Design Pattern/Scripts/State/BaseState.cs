using MyProject.Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.State{

    public class BaseState : IState
    {
        public Player player;

        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }

        public void Initialize(Player player)
        {
            this.player = player;
        }

        public virtual void Update()
        {
            player.text.text = $"{GetType().Name} : {player.stateStay:n0}";
            player.stateStay += Time.deltaTime;
        }
    }
}
