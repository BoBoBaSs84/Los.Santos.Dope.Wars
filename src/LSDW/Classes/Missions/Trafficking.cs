using GTA;
using LSDW.Core.Interfaces.Models;
using LSDW.Enumerators;
using LSDW.Factories;
using LSDW.Interfaces.Actors;
using LSDW.Interfaces.Missions;
using LSDW.Interfaces.Services;

namespace LSDW.Classes.Missions;

/// <summary>
/// The trafficking class.
/// </summary>
internal sealed class Trafficking : IMission
{
	private readonly ICollection<IDealer> _dealers;
	private readonly IDateTimeService _timeService;	
	private readonly ILoggerService _logger;
	private readonly IPlayer _player;
	private readonly IGameStateService _stateService;

	private Ped? _character;

	/// <summary>
	/// Initializes a instance of the trafficking class.
	/// </summary>
	/// <param name="timeService">The current date and time service to use.</param>
	/// <param name="logger">The logger service service to use.</param>
	/// <param name="player">The player instance to use.</param>
	internal Trafficking(IDateTimeService timeService, ILoggerService logger, IPlayer player)
	{
		_dealers = new HashSet<IDealer>();
		_timeService = timeService;
		_logger = logger;
		_player = player;
		_stateService = ServiceFactory.CreateGameStateService(_logger, _player, _dealers);
		
		Status = MissionStatusType.Runable;
	}

	public MissionStatusType Status { get; private set; }

	public void OnAborted(object sender, EventArgs args)
	{
		foreach (IDealer dealer in _dealers)
			dealer.Delete();
		
		_dealers.Clear();
		
		Status = MissionStatusType.Stopped;
	}

	public void OnTick(object sender, EventArgs args)
	{
		if (Status is not MissionStatusType.Started)
			return;

		_character ??= Game.Player.Character;
	}
}
