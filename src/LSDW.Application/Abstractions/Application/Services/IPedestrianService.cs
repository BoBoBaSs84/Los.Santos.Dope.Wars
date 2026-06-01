using GTA;

namespace LSDW.Application.Abstractions.Application.Services;

/// <summary>
/// Represents a collection service contract of pedestrian (<see cref="Ped"/>) entities.
/// </summary>
public interface IPedestrianService : IEnumerable<Ped>
{
	/// <summary>
	/// Gets the number of pedestrian (<see cref="Ped"/>) entities in the collection.
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Adds a pedestrian (<see cref="Ped"/>) entity to the collection.
	/// </summary>
	/// <param name="item">
	/// The pedestrian (<see cref="Ped"/>) entity to add to the collection.
	/// </param>
	void Add(Ped item);

	/// <summary>
	/// Clears the collection of pedestrian (<see cref="Ped"/>) entities.
	/// </summary>
	void Clear();

	/// <summary>
	/// Removes a pedestrian (<see cref="Ped"/>) entity from the collection.
	/// </summary>
	/// <param name="item">
	/// The pedestrian (<see cref="Ped"/>) entity to remove from the collection.
	/// </param>
	void Remove(Ped item);
}
