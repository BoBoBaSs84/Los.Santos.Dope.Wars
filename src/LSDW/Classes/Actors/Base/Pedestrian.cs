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
	protected Pedestrian(Vector3 position)
	{
		Position = position;
		Name = RandomHelper.GetFullName();
	}

	/// <summary>
	/// Initializes a instance of the pedestrian class.
	/// </summary>
	/// <param name="position">The position of the pedestrian.</param>
	/// <param name="name">The name of the pedestrian.</param>
	protected Pedestrian(Vector3 position, string name) : this(position) => Name = name;

	public bool Created => Ped is not null;

	public Vector3 Position { get; }

	public PedHash Hash { get; private set; }

	public Ped? Ped { get; private set; }

	public string Name { get; }

	public void Create(PedHash pedHash, float health = 50, float armor = 0, int money = 25)
	{
		if (Created)
			return;

		Hash = pedHash;
		Model model = ScriptHookHelper.RequestModel(pedHash);
		Ped = World.CreatePed(model, Position);
		Ped.HealthFloat = health;
		Ped.ArmorFloat = armor;
		Ped.Money = money;
	}

	public void Delete()
		=> Ped?.Delete();

	public void Update(float health = 50, float armor = 0, int money = 25)
	{
		if (Ped is null)
			return;

		Ped.HealthFloat = health;
		Ped.ArmorFloat = armor;
		Ped.Money = money;
	}
}
