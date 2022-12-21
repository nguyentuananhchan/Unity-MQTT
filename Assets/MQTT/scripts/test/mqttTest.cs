using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;
using UnityEngine.UI;

public class mqttTest : MonoBehaviour {
	public Text inputText;
	private MqttClient client;
	public Text receiveText;
	// Use this for initialization
	void Start() {
		// create client instance 
		client = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);
		//client = new MqttClient(IPAddress.Parse("143.185.118.233"),8080 , false , null ); 

		// register to message received 
		client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

		string clientId = Guid.NewGuid().ToString();
		client.Connect(clientId);

		// subscribe to the topic "/home/temperature" with QoS 2 
		client.Subscribe(new string[] { "hello/world" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
	}
	public IEnumerator ThisWillBeExecutedOnTheMainThread(string msg)
	{
		Debug.Log("This is executed from the main thread");
		StartMQTT(msg);
		yield return null;
	}
	void StartMQTT(string msg) {
		receiveText.text = "Received: " + msg;
	}
	void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
	{
		//receiveText.text = "Received: " + System.Text.Encoding.UTF8.GetString(e.Message);
		UnityMainThreadDispatcher.Instance().Enqueue(() => StartCoroutine(ThisWillBeExecutedOnTheMainThread(System.Text.Encoding.UTF8.GetString(e.Message))));
		Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
	}

	void OnGUI() {
		/*
		if (GUI.Button(new Rect(20, 40, 80, 20), "Level 1")) {
			Debug.Log("sending...");
			client.Publish("hello/world", System.Text.Encoding.UTF8.GetBytes("Sending"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
			Debug.Log("sent");
		} */
	}
	public void __SendToReceived() {
		string send = inputText.text;
		//Debug.Log(inputText.text);
		client.Publish("hello/world", System.Text.Encoding.UTF8.GetBytes(send), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
	}
	// Update is called once per frame
	void Update () {



	}
}
