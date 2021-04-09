﻿using CcgCore.Controller.Cards;
using CcgCore.Model.Cards;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CcgCore.Model.Effects
{
    public class AddCardsEffect : TargetedCardEffect
    {
        [SerializeField, FoldoutGroup("@DisplayLabel")] private RegionSelectionType regionSelectionType;
        [SerializeField, FoldoutGroup("@DisplayLabel"), ValueDropdown("@CcgCore.Controller.CardGameEditor.GetAllCards")] private List<CardDefinition> cards = new List<CardDefinition>();

        public override void ActivateEffects(CardEffectActivationContext context)
        {
            var targets = GetTargetActors(context);
            foreach (var actor in targets)
            {
                var scopes = actor.ActorScope.GetAllChildScopesAtLevel(Parameters.ParameterScopeLevel.Region).Select(s => s as FieldRegion).ToList();
                var regions = regionSelectionType switch
                {
                    RegionSelectionType.All => scopes,
                    RegionSelectionType.Random => scopes.GetRange(Random.Range(0, scopes.Count), 1),
                    RegionSelectionType.ByTemplate => throw new System.Exception(),
                    _ => throw new System.NotImplementedException(),
                };
                foreach (var region in regions)
                {
                    foreach (var card in cards)
                        region.AddCard(card);
                }
            }
        }

        private enum RegionSelectionType
        {
            All = 0,
            Random,
            ByTemplate,
        }

        public override string DisplayLabel => cards.Count == 1 && cards[0] ? $"Add card {cards[0].name} to {target}" : $"Add Cards to {target}";
    }
}
