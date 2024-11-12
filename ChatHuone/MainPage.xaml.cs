using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;

namespace ChatHuone;

public partial class MainPage : ContentPage
{
	static readonly HttpClient client = new();
	ObservableCollection<Viesti> viestit = new();
	ClientWebSocket ws = new();
	public MainPage()
	{
		InitializeComponent();
        _ = Yhdista();
		
		Viesti Testi1 = new() {TimeStamp = DateTime.Now, Nimi = "Olli", Teksti= "Terveppä terve"};
		Viesti Testi2 = new() {TimeStamp = DateTime.Now, Nimi = "Olli2", Teksti= "Terveppä terve2"};
		viestit.Add(Testi1);
		viestit.Add(Testi2);
		DataCollectionView.ItemsSource = viestit;
		
	}

	async Task Yhdista()
	{

		Uri uri = new("ws://127.0.0.1:8080");
		await ws.ConnectAsync(uri, CancellationToken.None);

		Console.WriteLine("Connected");

	}

	void OnClick1 (object sender, EventArgs e){
		//btn1.BackgroundColor = Color.FromRgb(4, 4, 6);
		Viesti testi3 = new() {TimeStamp = DateTime.Now, Nimi = LNimi.Text, Teksti = ChatViesti.Text};
		viestit.Add(testi3);
		_ = LahetaViesti(testi3);
		
	}

	async Task LahetaViesti (Viesti testi3){
		string message = testi3.Teksti;

		byte[] buffer = Encoding.UTF8.GetBytes(message);
		await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

		byte[] receiveBuffer = new byte[1024];
		WebSocketReceiveResult receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
		string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);

		Console.WriteLine($"Received message: {receivedMessage}");
	}
}

