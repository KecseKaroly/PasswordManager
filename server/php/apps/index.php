<?php

include("../db.php");
include("../common.php");

// HTTP METHODS HASZN�LATA

$request = $_SERVER['REQUEST_METHOD'];

switch ($request) {
	case "GET":
		$apps = getApps();
		echo json_encode($apps);
		break;
	case "POST":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true);
		$user = checkLoggedIn($data["username"], $data["password"]);
		if (!$user) {
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}
		$userId = $user["id"];
		$appName = $data["appname"];
		addApp($userId, $appName);
		break;
	case "PUT":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true);
		$user = checkLoggedIn($data["username"], $data["password"]);
		if (!$user) {
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}
		$appId = $data["appid"];
		$appName = $data["appname"];
		updateApp($appId, $appName);
		break;
	case "DELETE":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true);
		$user = checkLoggedIn($data["username"], $data["password"]);
		if (!$user) {
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}
		delApp($data["id"]);
		break;
	default:
		header('HTTP/1.1 405 Method Not Allowed');
		header('Allow: GET, POST, PUT, DELETE');
		break;
}


//KÉSZ
function getApps() {
	global $con;
	
	// Perform query
	
	$result = $con -> query("SELECT appId, userId, appName FROM app");
	
	return $result->fetch_all(MYSQLI_ASSOC);
	
}
//KÉSZ
function addApp($userid,$title) {
	global $con;
	
	// Perform query
	$result = $con->query("INSERT INTO app (userId, appName) VALUES ('".$userid."', '".$title."')");
	echo json_encode($result);
}
//KÉSZ
function delApp($id) {
	global $con;

	// Perform query
	$result = $con -> query("DELETE FROM app WHERE appId = '$id'");
	echo json_encode($con->error);
}
//KÉSZ
function updateApp($appId, $appName){
	global $con;

	// Perform query
	$result = $con -> query("UPDATE app SET appName='$appName' WHERE appId='$appId'");
	echo json_encode($result);
}
?>