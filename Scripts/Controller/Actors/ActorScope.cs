﻿using CcgCore.Model.Parameters;

namespace CcgCore.Controller.Actors
{
    public class ActorScope : ParameterScope
    {
        public ActorScope(ParameterScope parentScope) 
            : base(ParameterScopeLevel.Actor, parentScope)
        {
        }
    }
}
