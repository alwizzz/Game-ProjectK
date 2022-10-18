using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Interactable
{
    [SerializeField] RawIngredient rawIngredientPrefab;

    private void Start()
    {
        modelRenderers = GetComponentInChildren<RendererMaster>().GetRenderers();
    }
    public override void InteractionWhenPlayerNotHoldingItem(PlayerAction playerAction)
    {
        SpawnItemToPlayer(playerAction);
    }

    void SpawnItemToPlayer(PlayerAction playerAction)
    {
        var rawIngredient = Instantiate(
            rawIngredientPrefab,
            transform.position,
            Quaternion.identity
        );
        playerAction.GiveItemToHold(rawIngredient);
    }
}
