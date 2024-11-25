using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace ChatHuone;

public partial class MainPage : ContentPage
{
	static readonly HttpClient client = new();
	ObservableCollection<Dictionary<string, string>> viestit = new();
	ClientWebSocket ws = new();
	static DatabaseHandler dbHandler = new();
	public MainPage()
	{
		
		InitializeComponent();
		dbHandler.CreateDatabase();


        _ = Yhdista();
		// ViestiVertaus();
		
		DataCollectionView.ItemsSource = viestit;
		
	}

	async Task Yhdista()
	{

		Uri uri = new("ws://127.0.0.1:8080");
		await ws.ConnectAsync(uri, CancellationToken.None);
		// ws.ConnectAsync(uri, CancellationToken.None);
		Console.WriteLine("Connected");

	}

	void OnClick1 (object sender, EventArgs e){
		
		string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		Dictionary<string, string> viesti = new() {{"Tyyppi", "Viesti"}, {"TimeStamp", timeStamp}, {"Nimi", LNimi.Text}, {"Teksti", ChatViesti.Text}};
		viestit.Add(viesti);
        _ = LahetaViesti(viesti);
		
	}

	async Task LahetaViesti (Dictionary<string, string> viesti){
		
		string jsonString = JsonSerializer.Serialize(viesti);

		byte[] buffer = Encoding.UTF8.GetBytes(jsonString);
		await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

		byte[] receiveBuffer = new byte[1024];
		WebSocketReceiveResult receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
		string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
        Int32.TryParse(receivedMessage, out int id);
		dbHandler.LisaaViesti(viesti, id);
		Console.WriteLine($"Received message: {receivedMessage}");
	}
/*
*Metodiin vastauksen käsittely
*/
	public void ViestiVertaus(){
		
		int viestiCount = dbHandler.ViimeisinId();
		string stringiksiId = viestiCount.ToString(); 
		Dictionary<string, string> viesti = new() {{"Tyyppi", "Vertaus"}, {"Id", stringiksiId}};
		_ = LahetaViesti(viesti);

	}
}

