using System.Net.Http;
using System.Net.WebSockets;
using System.Text;

namespace ChatHuone;

public partial class MainPage : ContentPage
{
	static readonly HttpClient client = new();

	public MainPage()
	{
		InitializeComponent();
        _ = Yhdista();
		List<Viesti> Viestit = new List<Viesti>();
		Viesti Testi1 = new() {TimeStamp = DateTime.Now, Nimi = "Olli", Teksti= "Terveppä terve"};
		Viestit.Add(Testi1);
		DataCollectionView.ItemsSource = Viestit;
		
	}

	static async Task Yhdista()
	{

		using ClientWebSocket ws = new();

		Uri uri = new("ws://127.0.0.1:8080");
		await ws.ConnectAsync(uri, CancellationToken.None);

		Console.WriteLine("Connected");

		string message = "Hello from client!";

		byte[] buffer = Encoding.UTF8.GetBytes(message);
		await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

		byte[] receiveBuffer = new byte[1024];
		WebSocketReceiveResult receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
		string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);

		Console.WriteLine($"Received message: {receivedMessage}");

		/*try
		{
			string responseBody = await client.GetStringAsync("http://127.0.0.1:8080/");
			System.Console.WriteLine(responseBody);
		}
		catch (HttpRequestException e)
		{
			System.Console.WriteLine("\nException Caught!");
			System.Console.WriteLine($"Message :{e.Message}");
		}*/
	}

}

