<?php
if (isset($_SESSION['username'])) {
echo 'Hello' . $_SESSION['username'];
		header("Location: index.php");
		exit();
}
if (isset($_COOKIE['username'])) {
		echo 'Hello' . $_COOKIE['username'];
		header("Location: index.php");
		exit();
	}

$user = 'u361244280_root';
$password = 'avssn123';
$db = 'u361244280_airbd';
$con = new mysqli('localhost',$user,$password,$db) or die("Unable to connect");
$email = trim(mysql_real_escape_string($_GET['email']));
$key = trim(mysql_real_escape_string($_GET['key']));
$query_verify_email = "SELECT * FROM register WHERE email ='$email' and isactive = 1";
$result_verify_email = mysqli_query($con,$query_verify_email);
if (mysqli_num_rows($result_verify_email) == 1)
{
	echo '<div>Your Account already exists. Please <a href="login.php">Login Here</a></div>'; 
} else {

if (isset($email) && isset($key))
{
	mysqli_query($con, "UPDATE register SET isactive=1 WHERE email ='".$email."' AND hash='".$key."' ") or die(mysql_error());
	if (mysqli_affected_rows($con) == 1)
	{
		echo '<div>Your Account has been activated. Please <a href="login.php">Login Here</a></div>';
	} else {
	echo '<div>Account couldnot be activated.</div>';
	}
}

mysqli_close($con);

}?>
