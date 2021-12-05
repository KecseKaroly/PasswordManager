<?php

// Check if user logged in
// Parameters: username, hashed pw
function checkLoggedIn($u, $p) {
	global $con;
	
	// Perform query
	$result = $con -> query("SELECT id, username, password, isAdmin FROM user WHERE username = '$u' AND password = '$p'");
	
	return $result->fetch_all(MYSQLI_ASSOC)[0];
}

?>