using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Linq;
using Sdkbox;

public class PurchaseHandler : MonoBehaviour
{
	public Text messageText;
	private Sdkbox.IAP _iap;
	private String mLogBuffer;
	private const int MAX_LOG_LINE = 5;

	// Use this for initialization
	void Start()
	{
		_iap = FindObjectOfType<Sdkbox.IAP>();
		if (_iap == null)
		{
			log("Failed to find IAP instance");
		}
	}

	public void getProducts()
	{
		if (_iap != null)
		{
			log ("About to getProducts, will trigger onProductRequestSuccess event");
			_iap.getProducts ();
		}
	}

	public void Purchase(string item)
	{
		if (_iap != null)
		{
			log("About to purchase " + item);
			_iap.purchase(item);
		}
	}

	public void Refresh()
	{
		if (_iap != null)
		{
			log("About to refresh");
			_iap.refresh();
		}
	}

	public void Restore()
	{
		if (_iap != null)
		{
			log("About to restore");
			_iap.restore();
		}
	}

	private void log(string msg)
	{
		mLogBuffer += msg;
		mLogBuffer += Environment.NewLine;
		int numLines = mLogBuffer.Split('\n').Length;
		if(numLines > MAX_LOG_LINE){
			string[] lines = mLogBuffer.Split(Environment.NewLine.ToCharArray()).Skip(numLines - MAX_LOG_LINE).ToArray();
			mLogBuffer = string.Join(Environment.NewLine, lines);
		}
			
		if (messageText)
		{
			messageText.text = mLogBuffer;
		}

		Debug.Log(msg);
	}

	//
	// Event Handlers
	//

	public void onInitialized(bool status)
	{
		log("Init " + status);
	}

	public void onSuccess(Product product)
	{
		log("onSuccess: " + product.name);
	}

	public void onFailure(Product product, string message)
	{
		log("onFailure " + message);
	}

	public void onCanceled(Product product)
	{
		log("onCanceled product: " + product.name);
	}

	public void onRestored(Product product)
	{
		log("onRestored: " + product.name);
	}

	public void onProductRequestSuccess(Product[] products)
	{
		foreach (var p in products)
		{
			log("Product: " + p.name + " price: " + p.price);
		}
	}

	public void onProductRequestFailure(string message)
	{
		log("onProductRequestFailure: " + message);
	}

	public void onRestoreComplete(string message)
	{
		log("onRestoreComplete: " + message);
	}
}
