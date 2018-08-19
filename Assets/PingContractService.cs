﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using System;

public class PingToken : MonoBehaviour {

	// Here we define accountAddress (the public key) that we are going to get later from the private key
	private string accountAddress;
	// This is the secret key of the address, you can use yours or the one already assigned
	// Remember you need an account with some Ether (the one provided below might not have enough)
	private string accountPrivateKey = "0xe11e11e11e11e11e11e11e11e11e11e11e11e11e11e11e11e11e11e11e11e11e";
	// This is the testnet we are going to use for our contract, in this case kovan
	private string _url = "https://kovan.infura.io";
	// This is the Contract Transaction builder required to create our contract
	private DeployContractTransactionBuilder contractTransactionBuilder = new DeployContractTransactionBuilder();

	// Use this for initialization
	void Start () {
		// At the start we call this function, which will import the public key from the accountPrivateKey we
		// already assigned above
		importAccountFromPrivateKey ();

		// Now that we have our account imported we'll deploy the PingToken Contract into the Kovan network.
		deployEthereumContract();
	}

	public void importAccountFromPrivateKey () {
		// Here we try to get the public address from the secretKey we defined
		try {
			var address = Nethereum.Signer.EthECKey.GetPublicAddress (accountPrivateKey);
			// Then we define the accountAdress var with the public key
			accountAddress = address;

			print("Imported account SUCCESS");
		} catch (Exception e) {
			// if we catch an error when getting the public address we just display the exception in the console
			print("error" + e);
		}
	}

	public void deployEthereumContract() {
		print("Deploying contract...");

		// Here we have our ABI & bytecode, required for both creating and accessing our contract.
		var abi = @"[{""constant"":true,""inputs"":[],""name"":""name"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""totalSupply"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""pings"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""INITIAL_SUPPLY"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""decimals"",""outputs"":[{""name"":"""",""type"":""uint8""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[],""name"":""ping"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""name"":""balance"",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""symbol"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_to"",""type"":""address""},{""name"":""_value"",""type"":""uint256""}],""name"":""transfer"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":false,""name"":""pong"",""type"":""uint256""}],""name"":""Pong"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""from"",""type"":""address""},{""indexed"":true,""name"":""to"",""type"":""address""},{""indexed"":false,""name"":""value"",""type"":""uint256""}],""name"":""Transfer"",""type"":""event""}]";
		var byteCode = @"6060604052341561000f57600080fd5b601260ff16600a0a6305f5e10002600181905550601260ff16600a0a6305f5e10002600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000208190555061074f806100836000396000f300606060405260043610610099576000357c0100000000000000000000000000000000000000000000000000000000900463ffffffff16806306fdde031461009e57806318160ddd1461012c5780631e81ccb2146101555780632ff2e9dc1461017e578063313ce567146101a75780635c36b186146101d657806370a08231146101ff57806395d89b411461024c578063a9059cbb146102da575b600080fd5b34156100a957600080fd5b6100b1610334565b6040518080602001828103825283818151815260200191508051906020019080838360005b838110156100f15780820151818401526020810190506100d6565b50505050905090810190601f16801561011e5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b341561013757600080fd5b61013f61036d565b6040518082815260200191505060405180910390f35b341561016057600080fd5b610168610373565b6040518082815260200191505060405180910390f35b341561018957600080fd5b610191610379565b6040518082815260200191505060405180910390f35b34156101b257600080fd5b6101ba61038a565b604051808260ff1660ff16815260200191505060405180910390f35b34156101e157600080fd5b6101e961038f565b6040518082815260200191505060405180910390f35b341561020a57600080fd5b610236600480803573ffffffffffffffffffffffffffffffffffffffff1690602001909190505061049d565b6040518082815260200191505060405180910390f35b341561025757600080fd5b61025f6104e6565b6040518080602001828103825283818151815260200191508051906020019080838360005b8381101561029f578082015181840152602081019050610284565b50505050905090810190601f1680156102cc5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b34156102e557600080fd5b61031a600480803573ffffffffffffffffffffffffffffffffffffffff1690602001909190803590602001909190505061051f565b604051808215151515815260200191505060405180910390f35b6040805190810160405280600981526020017f50696e67546f6b656e000000000000000000000000000000000000000000000081525081565b60015481565b60005481565b601260ff16600a0a6305f5e1000281565b601281565b600080601260ff16600a0a6001029050600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205481111515156103ed57600080fd5b8060016000828254039250508190555080600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000206000828254039250508190555060008081548092919060010191905055507f58b69f57828e6962d216502094c54f6562f3bf082ba758966c3454f9e37b15256000546040518082815260200191505060405180910390a160005491505090565b6000600260008373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020549050919050565b6040805190810160405280600481526020017f50494e470000000000000000000000000000000000000000000000000000000081525081565b60008073ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff161415151561055c57600080fd5b600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205482111515156105aa57600080fd5b81600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205403600260003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000208190555081600260008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1681526020019081526020016000205401600260008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020819055508273ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef846040518082815260200191505060405180910390a360019050929150505600a165627a7a72305820de8fe20456e277e097051eb40a1239d02fd93b0f5e435de8208865c1a7ebaf890029";

		StartCoroutine(deployContract(abi, byteCode, accountAddress, (result) => {
			print("Result " + result);
		}));

	}


	public IEnumerator deployContract (string abi, string byteCode, string senderAddress, System.Action<string> callback) {
		// Ammount of gas required to create the contract
		var gas = new HexBigInteger (900000);

		// First we build the transaction
		var transactionInput = contractTransactionBuilder.BuildTransaction(abi, byteCode, senderAddress, gas, null);

		// Here we create a new signed transaction Unity Request with the url, the private and public key
		// (this will sign the transaction automatically)
		var transactionSignedRequest = new TransactionSignedUnityRequest(_url, accountPrivateKey, accountAddress);

		// Then we send the request and wait for the transaction hash
		Debug.Log("Sending Deploy contract transaction...");
		yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
		if (transactionSignedRequest.Exception == null)
		{
			// If we don't have exceptions we just return the result!
			callback(transactionSignedRequest.Result);
		}
		else
		{
			// if we had an error in the UnityRequest we just display the Exception error
			throw new System.InvalidOperationException("Deploy contract tx failed:" + transactionSignedRequest.Exception.Message);
		}
	}
}