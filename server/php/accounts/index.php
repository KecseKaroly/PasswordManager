<?php

include("../db.php");
include("../common.php");

// HTTP METHODS HASZN�LATA

$request = $_SERVER['REQUEST_METHOD'];

switch ($request) {
	case "GET":
		$accounts = getAccounts();
		echo json_encode($accounts);
		break;
	case "POST":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true);
		$user = checkLoggedIn($data["userUsername"], $data["userPassword"]);
		if (!$user) {
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}
		$appid = $data["appid"];
		$accUsername = $data["accUsername"];
		$accPassword = $data["accPassword"];
		addAcc($appid, $accUsername, $accPassword);
		break;
		break;
	case "PUT":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true); 
		$user = checkLoggedIn($data["userUsername"], $data["userPassword"]);
		if (!$user) {
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}
		$accId = $data["accid"];
		$accUsername = $data["accUsername"];
		$accPassword = $data["accPassword"];
		updateAcc($accId,$accUsername,$accPassword);
		break;
	case "DELETE":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true); 
		$user = checkLoggedIn($data["username"], $data["password"]);
		if(!$user)
		{
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}
		$accId = $data["accid"];
		delAccount($accId);
		break;
	default:
		header('HTTP/1.1 405 Method Not Allowed');
		header('Allow: GET, POST, PUT, DELETE');
		break;
}



function getAccounts() {
	global $con;
	
	// Perform query
	$result = $con -> query("SELECT accId, appId, username, password FROM account");
	return $result->fetch_all(MYSQLI_ASSOC);
}

function addAcc($appid, $accUsername, $accPassword) {
	global $con;
	
	// Perform query
	$uid = intval($userid);
	$result = $con->query("INSERT INTO account (appId, username, password) VALUES ('".$appid."', '".$accUsername."', '".$accPassword."')");
	echo json_encode($result);
}

function delAccount($id) {
	global $con;

	// Perform query
	$con -> query("DELETE FROM account WHERE accId = '$id'");
}

function updateAcc($accId,$accUsername,$accPassword){
	global $con;
	$result = $con -> query("UPDATE account SET username='$accUsername', password='$accPassword' WHERE accId='$accId'");
	echo json_encode($result);
}
?>