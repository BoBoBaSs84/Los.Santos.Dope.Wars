namespace LSDW.Abstractions.Enumerators;

/// <summary>
/// The task type enum for pedestrian actions.
/// </summary>
public enum TaskType
{
	/// <summary>
	/// The pedestrian has no task enumerator.
	/// </summary>
	NOTASK,
	/// <summary>
	/// The pedestrian idle enumerator.
	/// </summary>	
	IDLE,
	/// <summary>
	/// The pedestrian stands still enumerator.
	/// </summary>
	STAND,
	/// <summary>
	/// The pedestrian fight against enumerator.
	/// </summary>
	FIGHT,
	/// <summary>
	/// The pedestrian flee from enumerator.
	/// </summary>
	FLEE,
	/// <summary>
	/// The pedestrian guard position enumerator.
	/// </summary>	
	GUARD,
	/// <summary>
	/// The pedestrian wander around enumerator.
	/// </summary>
	WANDER,
	/// <summary>
	/// The pedestrian turn to enumerator.
	/// </summary>
	TURNTO,
	/// <summary>
	/// The pedestrian wait enumerator.
	/// </summary>
	WAIT,
}
