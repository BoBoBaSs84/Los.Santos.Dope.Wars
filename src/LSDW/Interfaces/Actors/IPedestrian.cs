using GTA;
using GTA.Math;

namespace LSDW.Interfaces.Actors;

/// <summary>
/// The pedestrian interface.
/// </summary>
public interface IPedestrian
{
	/// <summary>
	/// Is the pedestrian created?
	/// </summary>
	bool Created { get; }

	/// <summary>
	/// The name of the pedestrian.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// The position of the pedestrian.
	/// </summary>
	Vector3 Position { get; }

	/// <summary>
	/// The "ped" itself.
	/// </summary>
	Ped? Ped { get; }

	/// <summary>
	/// Creates the pedestrian.
	/// </summary>
	/// <param name="pedHash"></param>
	/// <param name="health"></param>
	/// <param name="armor"></param>
	/// <param name="money"></param>

	void Create(PedHash pedHash, float health = 50f, float armor = 0f, int money = 25);

	/// <summary>
	/// Updates the pedestrian.
	/// </summary>
	/// <param name="health"></param>
	/// <param name="armor"></param>
	/// <param name="money"></param>
	void Update(float health = 50f, float armor = 0f, int money = 25);

	/// <summary>
	/// Deletes the pedestrian.
	/// </summary>
	void Delete();
}
