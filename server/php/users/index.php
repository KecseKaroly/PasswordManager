<?php

include("../db.php");
include("../common.php");

// HTTP METHODS HASZN�LATA

$request = $_SERVER['REQUEST_METHOD'];

switch ($request) {
	case "GET":
		if (!empty($_GET["username"])) {
			$users = login($_GET["username"], $_GET["password"]);
		}
		else{
			$users = getUsers();
		}
		
		echo json_encode($users);
		break;
	case "POST":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true);
		$user = checkLoggedIn($data["username"], $data["password"]);
		if (!$user or $user['isAdmin'] != '1') {
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}
		$isAdmin = $data["isAdmin"];
		$Addusername = $data["Addusername"];
		$Addpassword = $data["Addpassword"];
		addUser($isAdmin, $Addusername, $Addpassword);
		break;
		break;
	case "PUT":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true);
		$user = checkLoggedIn($data["username"], $data["password"]);
		if (!$user or $user["isAdmin"] != '1') {
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}
		$userId = $data["userid"];
		$UpdateUsername = $data["UpdateUsername"];
		$UpdatePassword = $data["UpdatePassword"];
		$isAdmin = $data["isAdmin"];
		updateApp($userId, $UpdateUsername, $UpdatePassword,$isAdmin);
		break;
	case "DELETE":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true);
		$user = checkLoggedIn($data["username"], $data["password"]);
		if (!$user or $user["isAdmin"] != '1') {
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}
		delUser($data["id"]);
		break;
	default:
		header('HTTP/1.1 405 Method Not Allowed');
		header('Allow: GET, POST, PUT, DELETE');
		break;
}


function login($u, $p) {
	global $con;
	
	// Perform query
	$result = $con -> query("SELECT id, username, password, isAdmin FROM user WHERE username = '$u' AND password = MD5('$p')");
	$response = $result->fetch_all(MYSQLI_ASSOC);
	if($response == null)
	{
		return false;
	}
	else
		return $response[0];
}
function getUsers(){
	global $con;
	// Perform query
	$result = $con -> query("SELECT id, username, password, isAdmin FROM user");
	return $result->fetch_all(MYSQLI_ASSOC);
}
function addUser($isAdmin, $username, $password) {
	global $con;
	
	// Perform query
	$result = $con->query("INSERT INTO user (username, password, isAdmin) VALUES ('".$username."', MD5('".$password."'), '".$isAdmin."')");
	echo json_encode($result);
}

function delUser($id) {
	global $con;

	// Perform query
	$result = $con -> query("DELETE FROM user WHERE id = '$id'");
	echo json_encode($result);
}

function updateApp($userId, $UpdateUsername, $UpdatePassword, $isAdmin){
	global $con;

	// Perform query
	if($UpdatePassword == "")
	{
		$result = $con -> query("UPDATE user SET username='$UpdateUsername', isAdmin='$isAdmin' WHERE id='$userId'");
	}
	else 	$result = $con -> query("UPDATE user SET username='$UpdateUsername', password=MD5('$UpdatePassword'), isAdmin='$isAdmin' WHERE id='$userId'");

	echo json_encode($result);
}
?>