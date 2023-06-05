namespace LSDW.Domain.Interfaces.Services;

public interface ISettingsService
{
	void Load();
	void Save();

	int GetDownTimeInHours();
  void SetDownTimeInHours(int value);
	bool GetHasArmor();
  void SetHasArmor(bool value);
	bool GetHasWeapons();
  void SetHasWeapons(bool value);
	float GetMaximumDrugValue();
  void SetMaximumDrugValue(float value);
	float GetMinimumDrugValue();
  void SetMinimumDrugValue(float value);
	float GetExperienceMultiplier();
  void SetExperienceMultiplier(float value);
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
