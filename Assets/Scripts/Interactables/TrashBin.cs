using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : Interactable
{

    [SerializeField] LevelMaster levelMaster;
    [SerializeField] CookingRecipe cookingRecipe;
    [SerializeField] UncompletedDish basePlateDishPrefab;
    //[SerializeField] CookingRecipe.DishState basePlateDishState;
    [SerializeField] UncompletedDish baseBowlDishPrefab;
    //[SerializeField] CookingRecipe.DishState baseBowlDishState;

    private void Awake()
    {
        levelMaster = FindObjectOfType<LevelMaster>();
        levelSFXManager = FindObjectOfType<LevelSFXManager>();
    }

    private void Start()
    {
        cookingRecipe = levelMaster.GetCookingRecipe();

        rendererMaster = GetComponentInChildren<RendererMaster>();
        childRenderers = rendererMaster.GetChildRenderers();

        //Debug.Log(colorForNonSpawner + " " + colorForSpawner);
    }

    public override void InteractionWhenPlayerHoldingItem(PlayerAction playerAction)
    {
        var playerHeldItem = playerAction.GetHeldItem();
        var playerItemTypeString = playerHeldItem.GetType().ToString();
        if(playerItemTypeString == "UncompletedDish")
        {
            levelSFXManager.PlayTrashBinSFX();

            ((UncompletedDish)playerHeldItem).ClearMixedIngredient();
            // Debug.Log("mubazir bro");
        }
        else if(playerItemTypeString == "CompletedDish")
        {
            levelSFXManager.PlayTrashBinSFX();

            CompletedDish cDish = (CompletedDish)playerAction.TakeHeldItem();
            var cDishBase = cDish.GetBaseDish();
            //Debug.Log(cDishBase);
            Destroy(cDish.gameObject);
            Item spawn = null;
            if(cDishBase == Dish.BaseDish.Bowl)
            {
                spawn = Instantiate(baseBowlDishPrefab, transform.position, Quaternion.identity);
            }
            else if(cDishBase == Dish.BaseDish.Plate)
            {
                spawn = Instantiate(basePlateDishPrefab, transform.position, Quaternion.identity);
            }
            playerAction.GiveItemToHold(spawn);
        }
        else if(playerItemTypeString == "RawIngredient" || playerItemTypeString == "ProcessedIngredient")
        {
            levelSFXManager.PlayTrashBinSFX();

            DestroyItemFromPlayer(playerAction);
        }
    }

    void DestroyItemFromPlayer(PlayerAction playerAction)
    {
        var item = playerAction.TakeHeldItem();
        //if(item is Dish)
        //{

        //} else if(item is Ingredient)
        //{

        //}

        item.gameObject.SetActive(false);
        item.MoveToPivot(transform);
        Destroy(item);
    }
}
