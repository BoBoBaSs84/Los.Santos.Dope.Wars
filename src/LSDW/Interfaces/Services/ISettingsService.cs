namespace LSDW.Interfaces.Services;

public interface ISettingsService
{
	void Load();
	void Save();

	int GetDownTimeInHours();
  void SetDownTimeInHours(int value);	
	bool GetWearsArmor();
  void SetWearsArmor(bool value);	
	bool GetWearsWeapons();
  void SetWearsWeapons(bool value);	
	decimal GetMaximumDrugValue();
  void SetMaximumDrugValue(decimal value);	
	decimal GetMinimumDrugValue();
  void SetMinimumDrugValue(decimal value);	
	decimal GetExperienceMultiplier();
  void SetExperienceMultiplier(decimal value);	
	bool GetLooseDrugsOnDeath();
  void SetLooseDrugsOnDeath(bool value);	
	bool GetLooseMoneyOnDeath();
  void SetLooseMoneyOnDeath(bool value);	
	bool GetLooseDrugsWhenBusted();
  void SetLooseDrugsWhenBusted(bool value);	
	bool GetLooseMoneyWhenBusted();
  void SetLooseMoneyWhenBusted(bool value);	
	int GetInventoryExpansionPerLevel();
  void SetInventoryExpansionPerLevel(int value);	
	int GetStartingInventory();
  void SetStartingInventory(int value);	
}
