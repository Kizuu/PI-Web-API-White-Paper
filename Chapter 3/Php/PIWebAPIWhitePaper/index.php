<?php
include_once ("piwebapi.class.php");
$citiesNames = PIWebAPIWrapper::GetCitiesName ();
$attributesNames = PIWebAPIWrapper::GetAttributesName ();
$postAction = 0;
if (isset ( $_POST ["value"] )) {
	$valueToSend = $_POST ["value"];
	$selectedCity = urlencode ($_POST ["city_name"]);
	$selectedAttribute = urlencode ($_POST ["attribute_name"]);
	$statusCode = PIWebAPIWrapper::SendValue ( $selectedCity, $selectedAttribute, $valueToSend);
	$postAction = 1;
}

?>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<link href="default.css" rel="stylesheet" />
<title>Sending Value to the PI System through PI Web API</title>
</head>
<body>
	<h1>Sending Value to the PI System through PI Web API</h1>
	<form action="index.php" method="post">
		<p>Select the element and attribute that you want to send your value:</p>
		<label for="city_name">Select city</label> <select name="city_name"
			id="city_name" size="1">
    <?php
				foreach ( $citiesNames as $cityName ) {
					echo "<option value=\"$cityName\">" . $cityName . "</option>";
				}
				?>
  
    </select> <label for="attribute_name">Select attribute:</label> <select
			name="attribute_name" id="attribute_name" size="1">
    <?php
				foreach ( $attributesNames as $attributeName ) {
					echo "<option value=\"$attributeName\">" . $attributeName . "</option>";
				}
				?>
	</select> <label for="value">Value to send:</label> <input type="text"
			name="value" id="value" value="" /> <br /> <input type="submit"
			id="UpdateBtn" value="Send Value" />
	</form>
	<?php
	if ($postAction == 1) {
		echo "<script>";
		if ($statusCode==202)
		{		
			echo "alert('Success')";
		}
		else 
		{
			echo "alert('Error')";
		}
		echo "</script>";
	}
	?>
</body>
</html>