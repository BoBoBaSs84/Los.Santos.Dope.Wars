using GTA;
using GTA.Math;
using LSDW.Core.Helpers;
using LSDW.Helpers;
using LSDW.Interfaces.Actors;

namespace LSDW.Classes.Actors.Base;

internal abstract class Pedestrian : IPedestrian
{
	/// <summary>
	/// Initializes a instance of the pedestrian class.
	/// </summary>
	/// <param name="position">The position of the pedestrian.</param>
	/// <param name="pedHash">The ped hash of the pedestrian.</param>
	protected Pedestrian(Vector3 position, PedHash pedHash)
	{
		Position = position;
		Hash = pedHash;
		Name = RandomHelper.GetFullName();
	}

	/// <summary>
	/// Initializes a instance of the pedestrian class.
	/// </summary>
	/// <param name="position">The position of the pedestrian.</param>
	/// <param name="pedHash">The ped hash of the pedestrian.</param>
	/// <param name="name">The name of the pedestrian.</param>
	protected Pedestrian(Vector3 position, PedHash pedHash, string name) : this(position, pedHash) => Name = name;

	public bool Created => Ped is not null;
	public Vector3 Position { get; }
	public PedHash Hash { get; }
	public Ped? Ped { get; private set; }
	public string Name { get; }

	public void Create(float health = 100, float armor = 0, int money = 25)
	{
		if (Created)
			return;

		Model model = ScriptHookHelper.RequestModel(Hash);
		Ped = World.CreatePed(model, Position);
		Ped.HealthFloat = health;
		Ped.ArmorFloat = armor;
		Ped.Money = money;
	}

	public void Delete()
		=> Ped?.Delete();

	public void Update(float health = 100, float armor = 0, int money = 25)
	{
		if (Ped is null)
			return;

		Ped.HealthFloat = health;
		Ped.ArmorFloat = armor;
		Ped.Money = money;
	}
}
