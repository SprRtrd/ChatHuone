using System.Net.Http;

namespace ChatHuone;

public partial class MainPage : ContentPage
{
	static readonly HttpClient client = new();

	public MainPage()
	{
		InitializeComponent();
        _ = Yhdista();
	}

	static async Task Yhdista()
	{
		try
		{
			string responseBody = await client.GetStringAsync("http://127.0.0.1:8080/");
			System.Console.WriteLine(responseBody);
		}
		catch (HttpRequestException e)
		{
			System.Console.WriteLine("\nException Caught!");
			System.Console.WriteLine($"Message :{e.Message}");
		}
	}

}

