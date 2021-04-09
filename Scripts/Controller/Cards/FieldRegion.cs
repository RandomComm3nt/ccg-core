﻿using CcgCore.Controller.Events;
using CcgCore.Model.Cards;
using CcgCore.Model.Parameters;
using System.Collections.Generic;
using System.Linq;

namespace CcgCore.Controller.Cards
{
    public class FieldRegion : ParameterScope
    {
        public delegate void RegionChange();
        public event RegionChange OnCardAdded;

        private readonly List<CardStack> cardStacks;
        private readonly CardGameControllerBase cardGameController;

        public FieldRegion(CardGameControllerBase cardGameController, ParameterScope parentScope = null)
            : base(ParameterScopeLevel.Region, parentScope ?? cardGameController)
        {
            cardStacks = new List<CardStack>();
            this.cardGameController = cardGameController;
        }

        public void AddCard(CardDefinition cardDefinition)
        {
            var card = cardGameController.CardFactory.CreateCard(cardDefinition, null);
            AddCardToNewStack(card);
        }

        public void AddCard(Card card)
        {
            AddCardToNewStack(card);
        }

        private void AddCardToNewStack(Card card)
        {
            var stack = new CardStack(this);
            cardStacks.Add(stack);
            stack.AddCard(card);
            CardEvent cardGameEvent = new CardEvent(CardEvent.CardEventType.CardAdded);
            card.RaiseEvent(cardGameEvent);

            if (cardGameEvent.IsCancelled)
            {
                RemoveChild(stack);
                cardStacks.Remove(stack);
                return;
            }

            OnCardAdded?.Invoke();
        }

        public void RemoveCard(Card card)
        {
            if (ChildScopes.Contains(card))
                RemoveChild(card);
            else if (cardStacks.Any(cs => cs.BaseCard == card))
            {
                CardStack scope = cardStacks.First(cs => cs.BaseCard == card);
                RemoveChild(scope);
                cardStacks.Remove(scope);
            }

            OnCardAdded?.Invoke();
        }

        public List<CardStack> FindCardStacks()
        {
            return cardStacks
                .ToList();
        }

        public List<Card> FindCards(bool searchInStacks = false, CardDefinition cardDefinition = null)
        {
            return cardStacks
                .SelectMany(s => searchInStacks ? s.StackedCards : s.StackedCards.Take(1))
                .Where(c => !cardDefinition || c.CardDefinition == cardDefinition)
                .ToList();
        }

        public int GetCardQuantity(CardDefinition cardDefinition)
        {
            return 0;// cardStacks.Where(cs => cs.CardDefinition == cardDefinition).Sum(cs => cs.Quantity);
        }
    }
}
