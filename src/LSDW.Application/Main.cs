using GTA;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Application.Models.Missions;
using LSDW.Application.Constants;
using LSDW.Application.Factories;

namespace LSDW.Application;

/// <summary>
/// The Main class.
/// </summary>
[ScriptAttributes(Author = ApplicationConstants.Author, SupportURL = ApplicationConstants.SupportURL)]
public sealed class Main : Script
{
	private readonly IProviderManager _providerManager;
	private readonly IServiceManager _serviceManager;
	private readonly ITrafficking _trafficking;

	/// <summary>
	/// Determines if the application is developer mode or not.
	/// </summary>
	public static bool IsDevelopment { get; private set; }

	/// <summary>
	/// Initializes a instance of the main class.
	/// </summary>
	public Main()
	{
#if DEBUG
		IsDevelopment = true;
#else
		IsDevelopment = false;
#endif
		_providerManager = ApplicationFactory.GetProviderManager();
		_serviceManager = ApplicationFactory.GetServiceManager();
		_trafficking = ApplicationFactory.CreateTraffickingMission(_serviceManager, _providerManager);

		Interval = 10;

		KeyUp += OnKeyUp;
		Tick += OnTick;

		Aborted += _trafficking.OnAborted;
		Tick += _trafficking.OnTick;
	}

	private void OnTick(object sender, EventArgs e)
	{ }

	private void OnKeyUp(object sender, KeyEventArgs args)
	{ }
}
