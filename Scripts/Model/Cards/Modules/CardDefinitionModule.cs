﻿using CcgCore.Controller.Actors;
using CcgCore.Controller.Cards;

namespace CcgCore.Model.Cards
{
    public abstract class CardDefinitionModule
    {
        public virtual bool ValidateCardCanBeUsed(Actor actor) => true;

        public virtual void ActivateCard(CardEffectActivationContext context, Card thisCard)
        { }
    }
}
