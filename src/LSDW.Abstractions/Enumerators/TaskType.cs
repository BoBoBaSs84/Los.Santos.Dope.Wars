namespace LSDW.Abstractions.Enumerators;

/// <summary>
/// The task type enum for pedestrian actions.
/// </summary>
public enum TaskType
{
	/// <summary>
	/// The pedestrian has no task enumerator.
	/// </summary>
	NoTask,
	/// <summary>
	/// The pedestrian stands still enumerator.
	/// </summary>
	StandStill,
	/// <summary>
	/// The pedestrian fight against enumerator.
	/// </summary>
	FightAgainst,
	/// <summary>
	/// The pedestrian flee from enumerator.
	/// </summary>
	FleeFrom,
	/// <summary>
	/// The pedestrian guard position enumerator.
	/// </summary>	
	Guard,
	/// <summary>
	/// The pedestrian wander around enumerator.
	/// </summary>
	WanderAround,
	/// <summary>
	/// The pedestrian turn to enumerator.
	/// </summary>
	TurnTo,
	/// <summary>
	/// The pedestrian wait enumerator.
	/// </summary>
	Wait,
}
