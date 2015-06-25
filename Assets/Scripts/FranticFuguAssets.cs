using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla;
using Soomla.Store;

public class FranticFuguAssets : IStoreAssets
{
    #region Constants

    #region Currency

    public const string CURRENCY_SPONGE_ID = "currency_sponge";

    #endregion
    #region Unlocable Characters

    public const string CHAR_OCTO_ID = "char_octo";
    public const string CHAR_BOXFISH_ID = "char_boxfish";
    public const string CHAR_CUTTLEFISH_ID = "char_cuttlefish";
    public const string CHAR_JELLYFISH_ID = "char_jellyfish";
    public const string CHAR_SEAHORSE_ID = "char_seahorse";
    public const string CHAR_STINGRAY_ID = "char_stingray";

    #endregion
    #region Categories

    public const string CATEGORY_CHARACTER_NAME = "character";

    #endregion

    #endregion

    public int GetVersion()
    {
        return 0;
    }

    public VirtualCurrency[] GetCurrencies()
    {
        return new VirtualCurrency[] {
            CURRENCY_SPONGE
        };
    }

    public VirtualGood[] GetGoods()
    {
        return new VirtualGood[] {
            CHAR_OCTO,
            CHAR_BOXFISH,
            CHAR_CUTTLEFISH,
            CHAR_JELLYFISH,
            CHAR_SEAHORSE,
            CHAR_STINGRAY
        };
    }

    public VirtualCurrencyPack[] GetCurrencyPacks()
    {
        return new VirtualCurrencyPack[] { };
    }

    public VirtualCategory[] GetCategories()
    {
        return new VirtualCategory[] {
            CATEGORY_CHARACTER
        };
    }


    #region Virtual Currencies

    public static VirtualCurrency CURRENCY_SPONGE = new VirtualCurrency(
        "Sponge",       // name
        "",             // description
        CURRENCY_SPONGE_ID
    );

    #endregion  
    #region Unlockable Characters

    // This will be given at the start of the game.
    public static VirtualGood CHAR_OCTO = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,
        "Tako",
        "Takoyaki.",
        CHAR_OCTO_ID,
        new PurchaseWithVirtualItem(CURRENCY_SPONGE_ID, 0)
    );

    public static VirtualGood CHAR_BOXFISH = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,
        "Box Fish",
        "So square it wants to wear pants",
        CHAR_BOXFISH_ID,
        new PurchaseWithVirtualItem(CURRENCY_SPONGE_ID,100)
    );

    public static VirtualGood CHAR_CUTTLEFISH = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,
        "Cuttlefish",
        "It's so cuttly!",
        CHAR_CUTTLEFISH_ID,
        new PurchaseWithVirtualItem(CURRENCY_SPONGE_ID,100)
    );

    public static VirtualGood CHAR_JELLYFISH = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,
        "Jellyfish",
        "Bzzt.",
        CHAR_JELLYFISH_ID,
        new PurchaseWithVirtualItem(CURRENCY_SPONGE_ID,100)
    );

    public static VirtualGood CHAR_SEAHORSE = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,
        "Seahorse",
        "A cute lil dragon",
        CHAR_SEAHORSE_ID,
        new PurchaseWithVirtualItem(CURRENCY_SPONGE_ID, 100)
    );

    public static VirtualGood CHAR_STINGRAY = new EquippableVG(
        EquippableVG.EquippingModel.CATEGORY,
        "Stingray",
        "I sting.",
        CHAR_STINGRAY_ID,
        new PurchaseWithVirtualItem(CURRENCY_SPONGE_ID, 100)
    );

    #endregion
    #region Virtual Categories

    // Character
    public static VirtualCategory CATEGORY_CHARACTER = new VirtualCategory(
        CATEGORY_CHARACTER_NAME,
        new List<string>(new string[]{
            CHAR_OCTO_ID,
            CHAR_BOXFISH_ID,
            CHAR_CUTTLEFISH_ID,
            CHAR_JELLYFISH_ID,
            CHAR_SEAHORSE_ID,
            CHAR_STINGRAY_ID
        })
    );

    #endregion
}
