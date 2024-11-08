﻿using System.Net.Http;
using System.Net.WebSockets;
using System.Text;

namespace ChatHuone;

public partial class MainPage : ContentPage
{
	static readonly HttpClient client = new();
	List<Viesti> Viestit = new List<Viesti>();
	public MainPage()
	{
		InitializeComponent();
        _ = Yhdista();
		
		Viesti Testi1 = new() {TimeStamp = DateTime.Now, Nimi = "Olli", Teksti= "Terveppä terve"};
		Viesti Testi2 = new() {TimeStamp = DateTime.Now, Nimi = "Olli2", Teksti= "Terveppä terve2"};
		Viestit.Add(Testi1);
		Viestit.Add(Testi2);
		//DataCollectionView.ItemsSource = Viestit;
		
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
	}

	void OnClick1 (object sender, EventArgs e){
		//btn1.BackgroundColor = Color.FromRgb(4, 4, 6);
		Viesti Testi3 = new() {TimeStamp = DateTime.Now, Nimi = "Olliuusi", Teksti = "Toimii"};
		Viestit.Add(Testi3);
		DataCollectionView.ItemsSource = null;
		DataCollectionView.ItemsSource = Viestit;
	}
}

