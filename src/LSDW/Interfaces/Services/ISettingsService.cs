namespace LSDW.Interfaces.Services;

public interface ISettingsService
{
	int GetDownTimeInHours();
	int GetInventoryExpansionPerLevel();
	bool GetLooseDrugsOnDeath();
	bool GetLooseDrugsWhenBusted();
	decimal GetMaximumDrugValue();
	decimal GetMinimumDrugValue();
	int GetStartingInventory();
	bool Save();
	void SetDownTimeInHours(int value);
	void SetInventoryExpansionPerLevel(int value);
	void SetLooseDrugsOnDeath(bool value);
	void SetLooseDrugsWhenBusted(bool value);
	void SetMaximumDrugValue(decimal value);
	void SetMinimumDrugValue(decimal value);
	void SetStartingInventory(int value);
}