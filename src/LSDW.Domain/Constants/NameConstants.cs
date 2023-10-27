using LSDW.Domain.Helpers;

namespace LSDW.Domain.Constants;

/// <summary>
/// The name constants class.
/// </summary>
public static class NameConstants
{
	private static readonly Lazy<string[]> maleFirstNames = new(() => new string[]
	{
		"Simon","John","Jack","Grant","Sandeep","Sylvester","Lionel","James","Garrett","Reinout","Eric","Mihail","Jan","George","Jeffrey","Kevin","Paul","Hazem","David","Tete","Zainal","François","Michael","Jack","William","Alejandro","Tawana","Willis","Jason","Hung-Fu","Michael","Stuart","Min","Rajesh","Chris","Gabe","Russell","John","Ken","Magnus","Thierry","Andreas","Pete","Ken","Zheng","Stuart","Lolan","Baris","Frank","Brian","David","Tengiz","John","Eric","Brandon","Michael","Jose","Kok-Ho","Ben","Chris","Ovidiu","Ebru","Rob","Sean","Vidur","Benjamin","Paul","Suroor","Taylor","David","David","Dragan","Bob","Chad","Michael","Pat","Karan","Shane","Sameer","Don"
	});

	private static readonly Lazy<string[]> lastNames = new(() => new string[]
	{
		"Hillmann","Richins","Norman","King","Scardelis","Campbell","Duffy","Frintu","Okelberry","Smith","Mohamed","Abercrombie","Kaliyath","Rothkugel","Mares","Mirchandani","Sam","Ellerbrock","Petculescu","Cetinok","Hite","Gibson","Yukish","Dyck","Randall","McAskill-White","Poland","Johnson","Wilson","Hagens","Keil","Song","Keyser","Cracium","Koch","Nartker","Preston","Berge","Ito","Saraiva","Hall","Raheem","Harnpadoungsataya","Zimmerman","Rettig","Hamilton","Martin","Valdez","Dobney","Culbertson","Kleinerman","Laszlo","Gubbels","Sousa","Walters","Koenigsbauer","Trenary","Tsoflias","Reátegui Alayo","Dempsey","Benshoof","Munson","Connelly","Vande Velde","Miller","Singh","Johnson","Erickson","Hay","Sunkammurali","Ting","Randall","Barbariol","Ingle","Baker","Adams","Meyyappan","Northup","Bradley","Martin"
	});

	private static readonly Lazy<string[]> femaleFirstNames = new(() => new string[]
	{
		"Karen","Kitti","Barbara","Jo","Merav","Alice","Margie","Jae","Linda","Belinda","Carol","Janeth","Kathie","Kim","Sandra","Diane","Barbara","Linda","Kimberly","Katie","Samantha","Yvonne","Erin","Shelley","Paula","Lynn","Britta","Doris","Rebecca","Wendy","Cynthia","Annette","Laura","Wanida","Elizabeth","Denise","Jill","Betsy","Susan","Jo","Stephanie","Paula","Ruth","Gigi","Linda","Nancy","Laura","Lori","Lorraine","Jillian","Lori","Susan","Linda","Carole","Pamela","Candy","Mary","JoLynn","Kim","Amy","Janet","Deborah","Rachel","Sharon","Anibal","Terri","Diane","Sheela","Mary","Olinda","Diane","Mary","Nicole","Karen","Gail","Bonnie","Danielle","Mindy","Suchitra","Brenda","Janice","Angela","Janaina","Jean"
	});

	/// <summary>
	/// Returns a randomly chossen male first name.
	/// </summary>
	public static string GetMaleFirstName()
		=> maleFirstNames.Value[RandomHelper.GetInt(0, maleFirstNames.Value.Length)];

	/// <summary>
	/// Returns a randomly chossen female first name.
	/// </summary>
	public static string GetFemaleFirstName()
		=> femaleFirstNames.Value[RandomHelper.GetInt(0, femaleFirstNames.Value.Length)];

	/// <summary>
	/// Returns a randomly chossen last name.
	/// </summary>
	public static string GetLastName()
		=> lastNames.Value[RandomHelper.GetInt(0, lastNames.Value.Length)];

	/// <summary>
	/// Returns a randomly chossen male first name and last name.
	/// </summary>
	public static string GetMaleFullName()
		=> string.Concat(GetMaleFirstName(), " ", GetLastName());

	/// <summary>
	/// Returns a randomly chossen female first name and last name.
	/// </summary>
	public static string GetFemaleFullName()
		=> string.Concat(GetFemaleFirstName(), " ", GetLastName());

	/// <summary>
	/// Returns a randomly chossen male or female first name and last name
	/// </summary>
	/// <param name="female"><see langword="true"/> if a female full name should be returned.</param>
	public static string GetFullName(bool female)
		=> female ? GetFemaleFullName() : GetMaleFullName();
}