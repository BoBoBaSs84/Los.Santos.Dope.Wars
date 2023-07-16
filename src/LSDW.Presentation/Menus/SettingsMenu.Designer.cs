﻿#pragma warning disable CS1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using LemonUI.Menus;
using LSDW.Presentation.Items;
using RESX = LSDW.Presentation.Properties.Resources;

namespace LSDW.Presentation.Menus;

internal sealed partial class SettingsMenu
{
	private void AddMenuItems()
	{
    SettingListItem<int> dealerDownTimeInHoursItem = new(RESX.UI_Settings_Dealer_DownTimeInHours_Title)
    {
      Description = RESX.UI_Settings_Dealer_DownTimeInHours_Description,
      Items = _settingsService.DealerSettings.GetDownTimeInHoursValues(),
      SelectedItem = _settingsService.DealerSettings.GetDownTimeInHours()
    };
    dealerDownTimeInHoursItem.ItemChanged += OnDealerDownTimeInHoursItemChanged;
    Add(dealerDownTimeInHoursItem);
    SettingCheckboxItem dealerHasArmorItem = new(RESX.UI_Settings_Dealer_HasArmor_Title)
    {
      Description = RESX.UI_Settings_Dealer_HasArmor_Description,
      Checked = _settingsService.DealerSettings.GetHasArmor()
    };
    dealerHasArmorItem.CheckboxChanged += OnDealerHasArmorItemCheckboxChanged;
    Add(dealerHasArmorItem);
    SettingCheckboxItem dealerHasWeaponsItem = new(RESX.UI_Settings_Dealer_HasWeapons_Title)
    {
      Description = RESX.UI_Settings_Dealer_HasWeapons_Description,
      Checked = _settingsService.DealerSettings.GetHasWeapons()
    };
    dealerHasWeaponsItem.CheckboxChanged += OnDealerHasWeaponsItemCheckboxChanged;
    Add(dealerHasWeaponsItem);
    SettingListItem<float> marketSpecialOfferChanceItem = new(RESX.UI_Settings_Market_SpecialOfferChance_Title)
    {
      Description = RESX.UI_Settings_Market_SpecialOfferChance_Description,
      Items = _settingsService.MarketSettings.GetSpecialOfferChanceValues(),
      SelectedItem = _settingsService.MarketSettings.GetSpecialOfferChance()
    };
    marketSpecialOfferChanceItem.ItemChanged += OnMarketSpecialOfferChanceItemChanged;
    Add(marketSpecialOfferChanceItem);
    SettingListItem<int> marketInventoryChangeIntervalItem = new(RESX.UI_Settings_Market_InventoryChangeInterval_Title)
    {
      Description = RESX.UI_Settings_Market_InventoryChangeInterval_Description,
      Items = _settingsService.MarketSettings.GetInventoryChangeIntervalValues(),
      SelectedItem = _settingsService.MarketSettings.GetInventoryChangeInterval()
    };
    marketInventoryChangeIntervalItem.ItemChanged += OnMarketInventoryChangeIntervalItemChanged;
    Add(marketInventoryChangeIntervalItem);
    SettingListItem<int> marketPriceChangeIntervalItem = new(RESX.UI_Settings_Market_PriceChangeInterval_Title)
    {
      Description = RESX.UI_Settings_Market_PriceChangeInterval_Description,
      Items = _settingsService.MarketSettings.GetPriceChangeIntervalValues(),
      SelectedItem = _settingsService.MarketSettings.GetPriceChangeInterval()
    };
    marketPriceChangeIntervalItem.ItemChanged += OnMarketPriceChangeIntervalItemChanged;
    Add(marketPriceChangeIntervalItem);
    SettingListItem<float> marketMaximumDrugPriceItem = new(RESX.UI_Settings_Market_MaximumDrugPrice_Title)
    {
      Description = RESX.UI_Settings_Market_MaximumDrugPrice_Description,
      Items = _settingsService.MarketSettings.GetMaximumDrugPriceValues(),
      SelectedItem = _settingsService.MarketSettings.GetMaximumDrugPrice()
    };
    marketMaximumDrugPriceItem.ItemChanged += OnMarketMaximumDrugPriceItemChanged;
    Add(marketMaximumDrugPriceItem);
    SettingListItem<float> marketMinimumDrugPriceItem = new(RESX.UI_Settings_Market_MinimumDrugPrice_Title)
    {
      Description = RESX.UI_Settings_Market_MinimumDrugPrice_Description,
      Items = _settingsService.MarketSettings.GetMinimumDrugPriceValues(),
      SelectedItem = _settingsService.MarketSettings.GetMinimumDrugPrice()
    };
    marketMinimumDrugPriceItem.ItemChanged += OnMarketMinimumDrugPriceItemChanged;
    Add(marketMinimumDrugPriceItem);
    SettingListItem<float> playerExperienceMultiplierItem = new(RESX.UI_Settings_Player_ExperienceMultiplier_Title)
    {
      Description = RESX.UI_Settings_Player_ExperienceMultiplier_Description,
      Items = _settingsService.PlayerSettings.GetExperienceMultiplierValues(),
      SelectedItem = _settingsService.PlayerSettings.GetExperienceMultiplier()
    };
    playerExperienceMultiplierItem.ItemChanged += OnPlayerExperienceMultiplierItemChanged;
    Add(playerExperienceMultiplierItem);
    SettingCheckboxItem playerLooseDrugsOnDeathItem = new(RESX.UI_Settings_Player_LooseDrugsOnDeath_Title)
    {
      Description = RESX.UI_Settings_Player_LooseDrugsOnDeath_Description,
      Checked = _settingsService.PlayerSettings.GetLooseDrugsOnDeath()
    };
    playerLooseDrugsOnDeathItem.CheckboxChanged += OnPlayerLooseDrugsOnDeathItemCheckboxChanged;
    Add(playerLooseDrugsOnDeathItem);
    SettingCheckboxItem playerLooseMoneyOnDeathItem = new(RESX.UI_Settings_Player_LooseMoneyOnDeath_Title)
    {
      Description = RESX.UI_Settings_Player_LooseMoneyOnDeath_Description,
      Checked = _settingsService.PlayerSettings.GetLooseMoneyOnDeath()
    };
    playerLooseMoneyOnDeathItem.CheckboxChanged += OnPlayerLooseMoneyOnDeathItemCheckboxChanged;
    Add(playerLooseMoneyOnDeathItem);
    SettingCheckboxItem playerLooseDrugsWhenBustedItem = new(RESX.UI_Settings_Player_LooseDrugsWhenBusted_Title)
    {
      Description = RESX.UI_Settings_Player_LooseDrugsWhenBusted_Description,
      Checked = _settingsService.PlayerSettings.GetLooseDrugsWhenBusted()
    };
    playerLooseDrugsWhenBustedItem.CheckboxChanged += OnPlayerLooseDrugsWhenBustedItemCheckboxChanged;
    Add(playerLooseDrugsWhenBustedItem);
    SettingCheckboxItem playerLooseMoneyWhenBustedItem = new(RESX.UI_Settings_Player_LooseMoneyWhenBusted_Title)
    {
      Description = RESX.UI_Settings_Player_LooseMoneyWhenBusted_Description,
      Checked = _settingsService.PlayerSettings.GetLooseMoneyWhenBusted()
    };
    playerLooseMoneyWhenBustedItem.CheckboxChanged += OnPlayerLooseMoneyWhenBustedItemCheckboxChanged;
    Add(playerLooseMoneyWhenBustedItem);
    SettingListItem<int> playerInventoryExpansionPerLevelItem = new(RESX.UI_Settings_Player_InventoryExpansionPerLevel_Title)
    {
      Description = RESX.UI_Settings_Player_InventoryExpansionPerLevel_Description,
      Items = _settingsService.PlayerSettings.GetInventoryExpansionPerLevelValues(),
      SelectedItem = _settingsService.PlayerSettings.GetInventoryExpansionPerLevel()
    };
    playerInventoryExpansionPerLevelItem.ItemChanged += OnPlayerInventoryExpansionPerLevelItemChanged;
    Add(playerInventoryExpansionPerLevelItem);
    SettingListItem<int> playerStartingInventoryItem = new(RESX.UI_Settings_Player_StartingInventory_Title)
    {
      Description = RESX.UI_Settings_Player_StartingInventory_Description,
      Items = _settingsService.PlayerSettings.GetStartingInventoryValues(),
      SelectedItem = _settingsService.PlayerSettings.GetStartingInventory()
    };
    playerStartingInventoryItem.ItemChanged += OnPlayerStartingInventoryItemChanged;
    Add(playerStartingInventoryItem);
    SettingCheckboxItem traffickingDiscoverDealerItem = new(RESX.UI_Settings_Trafficking_DiscoverDealer_Title)
    {
      Description = RESX.UI_Settings_Trafficking_DiscoverDealer_Description,
      Checked = _settingsService.TraffickingSettings.GetDiscoverDealer()
    };
    traffickingDiscoverDealerItem.CheckboxChanged += OnTraffickingDiscoverDealerItemCheckboxChanged;
    Add(traffickingDiscoverDealerItem);
    SettingListItem<float> traffickingBustChanceItem = new(RESX.UI_Settings_Trafficking_BustChance_Title)
    {
      Description = RESX.UI_Settings_Trafficking_BustChance_Description,
      Items = _settingsService.TraffickingSettings.GetBustChanceValues(),
      SelectedItem = _settingsService.TraffickingSettings.GetBustChance()
    };
    traffickingBustChanceItem.ItemChanged += OnTraffickingBustChanceItemChanged;
    Add(traffickingBustChanceItem);
    SettingListItem<int> traffickingWantedLevelItem = new(RESX.UI_Settings_Trafficking_WantedLevel_Title)
    {
      Description = RESX.UI_Settings_Trafficking_WantedLevel_Description,
      Items = _settingsService.TraffickingSettings.GetWantedLevelValues(),
      SelectedItem = _settingsService.TraffickingSettings.GetWantedLevel()
    };
    traffickingWantedLevelItem.ItemChanged += OnTraffickingWantedLevelItemChanged;
    Add(traffickingWantedLevelItem);
	}
  private void OnDealerDownTimeInHoursItemChanged(object sender, ItemChangedEventArgs<int> args)
  {
    if (sender is not SettingListItem<int> item)
      return;
    _settingsService.DealerSettings.SetDownTimeInHours(item.SelectedItem);
  }
  private void OnDealerHasArmorItemCheckboxChanged(object sender, EventArgs args)
  {
    if (sender is not SettingCheckboxItem item)
      return;
    _settingsService.DealerSettings.SetHasArmor(item.Checked);
  }  
  private void OnDealerHasWeaponsItemCheckboxChanged(object sender, EventArgs args)
  {
    if (sender is not SettingCheckboxItem item)
      return;
    _settingsService.DealerSettings.SetHasWeapons(item.Checked);
  }  
  private void OnMarketSpecialOfferChanceItemChanged(object sender, ItemChangedEventArgs<float> args)
  {
    if (sender is not SettingListItem<float> item)
      return;
    _settingsService.MarketSettings.SetSpecialOfferChance(item.SelectedItem);
  }
  private void OnMarketInventoryChangeIntervalItemChanged(object sender, ItemChangedEventArgs<int> args)
  {
    if (sender is not SettingListItem<int> item)
      return;
    _settingsService.MarketSettings.SetInventoryChangeInterval(item.SelectedItem);
  }
  private void OnMarketPriceChangeIntervalItemChanged(object sender, ItemChangedEventArgs<int> args)
  {
    if (sender is not SettingListItem<int> item)
      return;
    _settingsService.MarketSettings.SetPriceChangeInterval(item.SelectedItem);
  }
  private void OnMarketMaximumDrugPriceItemChanged(object sender, ItemChangedEventArgs<float> args)
  {
    if (sender is not SettingListItem<float> item)
      return;
    _settingsService.MarketSettings.SetMaximumDrugPrice(item.SelectedItem);
  }
  private void OnMarketMinimumDrugPriceItemChanged(object sender, ItemChangedEventArgs<float> args)
  {
    if (sender is not SettingListItem<float> item)
      return;
    _settingsService.MarketSettings.SetMinimumDrugPrice(item.SelectedItem);
  }
  private void OnPlayerExperienceMultiplierItemChanged(object sender, ItemChangedEventArgs<float> args)
  {
    if (sender is not SettingListItem<float> item)
      return;
    _settingsService.PlayerSettings.SetExperienceMultiplier(item.SelectedItem);
  }
  private void OnPlayerLooseDrugsOnDeathItemCheckboxChanged(object sender, EventArgs args)
  {
    if (sender is not SettingCheckboxItem item)
      return;
    _settingsService.PlayerSettings.SetLooseDrugsOnDeath(item.Checked);
  }  
  private void OnPlayerLooseMoneyOnDeathItemCheckboxChanged(object sender, EventArgs args)
  {
    if (sender is not SettingCheckboxItem item)
      return;
    _settingsService.PlayerSettings.SetLooseMoneyOnDeath(item.Checked);
  }  
  private void OnPlayerLooseDrugsWhenBustedItemCheckboxChanged(object sender, EventArgs args)
  {
    if (sender is not SettingCheckboxItem item)
      return;
    _settingsService.PlayerSettings.SetLooseDrugsWhenBusted(item.Checked);
  }  
  private void OnPlayerLooseMoneyWhenBustedItemCheckboxChanged(object sender, EventArgs args)
  {
    if (sender is not SettingCheckboxItem item)
      return;
    _settingsService.PlayerSettings.SetLooseMoneyWhenBusted(item.Checked);
  }  
  private void OnPlayerInventoryExpansionPerLevelItemChanged(object sender, ItemChangedEventArgs<int> args)
  {
    if (sender is not SettingListItem<int> item)
      return;
    _settingsService.PlayerSettings.SetInventoryExpansionPerLevel(item.SelectedItem);
  }
  private void OnPlayerStartingInventoryItemChanged(object sender, ItemChangedEventArgs<int> args)
  {
    if (sender is not SettingListItem<int> item)
      return;
    _settingsService.PlayerSettings.SetStartingInventory(item.SelectedItem);
  }
  private void OnTraffickingDiscoverDealerItemCheckboxChanged(object sender, EventArgs args)
  {
    if (sender is not SettingCheckboxItem item)
      return;
    _settingsService.TraffickingSettings.SetDiscoverDealer(item.Checked);
  }  
  private void OnTraffickingBustChanceItemChanged(object sender, ItemChangedEventArgs<float> args)
  {
    if (sender is not SettingListItem<float> item)
      return;
    _settingsService.TraffickingSettings.SetBustChance(item.SelectedItem);
  }
  private void OnTraffickingWantedLevelItemChanged(object sender, ItemChangedEventArgs<int> args)
  {
    if (sender is not SettingListItem<int> item)
      return;
    _settingsService.TraffickingSettings.SetWantedLevel(item.SelectedItem);
  }
}
