using LemonUI;
using LemonUI.Menus;
using LSDW.Abstractions.Application.Managers;
using LSDW.Abstractions.Domain.Models;
using LSDW.Abstractions.Domain.Services;
using LSDW.Abstractions.Enumerators;
using LSDW.Abstractions.Presentation.Items;
using LSDW.Abstractions.Presentation.Menus;
using LSDW.Domain.Factories;
using LSDW.Presentation.Items;
using SMH = LSDW.Presentation.Helpers.SideMenuHelper;

namespace LSDW.Presentation.Menus;

/// <summary>
/// The side menu class.
/// </summary>
internal sealed class SideMenu : NativeMenu, ISideMenu
{
	private readonly Size _screenSize = GTA.UI.Screen.Resolution;
	private readonly TransactionType _type;
	private readonly IProviderManager _providerManager;

	private IInventory? source;
	private IInventory? target;
	private IPlayer? player;
	private ITransactionService? transactionService;
	private int maximumQuantity;

	/// <summary>
	/// Initializes a instance of the side menu class.
	/// </summary>
	/// <param name="type">The type of the transaction.</param>
	/// <param name="providerManager">The provider manager instance to use.</param>
	internal SideMenu(TransactionType type, IProviderManager providerManager) : base(SMH.GetTitle(type))
	{
		_type = type;
		_providerManager = providerManager;

		Alignment = SMH.GetAlignment(type);
		ItemCount = CountVisibility.Never;
		Offset = new PointF(_screenSize.Width / 64, _screenSize.Height / 36);
		UseMouse = false;
		BannerText.Font = GTA.UI.Font.Pricedown;

		SwitchItem = new SwitchItem(_type);
	}

	public ISwitchItem SwitchItem { get; private set; }
	public bool Initialized { get; private set; }

	public void Add(ObjectPool processables)
		=> processables.Add(this);

	public void CleanUp()
	{
		if (!Initialized)
			return;

		if (source is not null && target is not null)
		{
			source.PropertyChanged -= OnInventoryPropertyChanged;
			target.PropertyChanged -= OnInventoryPropertyChanged;
		}
		Clear();
		Initialized = false;
	}

	public void Initialize(IPlayer player, IInventory inventory)
	{
		if (Initialized)
			return;

		this.player = player;
		(source, target) = SMH.GetInventories(_type, player, inventory);
		source.PropertyChanged += OnInventoryPropertyChanged;
		target.PropertyChanged += OnInventoryPropertyChanged;
		maximumQuantity = SMH.GetMaximumQuantity(_type, player);
		transactionService = DomainFactory.CreateTransactionService(_providerManager, _type, source, target, maximumQuantity);
		Name = SMH.GetName(_type, target.Money);
		Add((SwitchItem)SwitchItem);
		AddDrugListItems(source, target);
		Initialized = true;
	}

	private void OnMenuItemActivated(object sender, EventArgs args)
	{
		if (transactionService is not null && player is not null)
		{
			if (sender is not SideMenu menu || menu.SelectedItem is not DrugListItem item || item.SelectedItem.Equals(0) || item.Tag is not DrugType drugType)
				return;

			int price = target.Where(x => x.Type.Equals(drugType)).Select(x => x.CurrentPrice).Single();
			int quantity = item.SelectedItem;
			bool succes = transactionService.Commit(drugType, quantity, price);

			if (succes)
			{
				DateTime dateTime = _providerManager.WorldProvider.Now;
				ITransaction transaction = DomainFactory.CreateTransaction(dateTime, _type, drugType, quantity, price);
				player.AddTransaction(transaction);
				transactionService.BustOrNoBust();
			}
		}
	}

	private void OnInventoryPropertyChanged(object sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName.Equals(nameof(target.Money), StringComparison.Ordinal) && target is not null)
			Name = SMH.GetName(_type, target.Money);
	}

	/// <summary>
	/// Adds the drug list items to the menu.
	/// </summary>
	/// <param name="source">The source inventory.</param>
	/// <param name="target">The target inventory.</param>
	private void AddDrugListItems(IInventory source, IInventory target)
	{
		var drugs = from s in source
								join t in target
								on s.Type equals t.Type
								select new { SourceDrug = s, t.CurrentPrice };

		foreach (var drug in drugs)
		{
			int comparisonPrice = _type switch
			{
				TransactionType.BUY => drug.SourceDrug.AveragePrice,
				TransactionType.SELL => drug.CurrentPrice,
				TransactionType.TAKE => drug.SourceDrug.AveragePrice,
				_ => default
			};

			DrugListItem item = new(drug.SourceDrug, comparisonPrice);
			item.Activated += OnMenuItemActivated;
			Add(item);
		}
	}
}
