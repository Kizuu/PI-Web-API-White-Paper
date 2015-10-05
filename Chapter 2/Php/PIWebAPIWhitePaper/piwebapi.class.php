<?php
class PIWebAPIWrapper {
	public static function GetCitiesName() {
		$base_url = "https://osi-serv.osibr.com/piwebapi/";
		$url = $base_url . "elements/E07dtyl5PO4EmFExFPTz6FbghNYw-7Q65Uav8IhHRRw_WQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxDSVRJRVM/elements";
		$result = PIWebAPIWrapper::MakeGetRequest ( $url );
		$itemsList = array ();
		$i = 0;
		foreach ( $result->Items as $item ) {
			$itemsList [$i] = $item->Name;
			$i = $i + 1;
		}
		return $itemsList;
	}
	public static function GetAttributesName() {
		$base_url = "https://osi-serv.osibr.com/piwebapi/";
		$url = $base_url . "elementtemplates/T07dtyl5PO4EmFExFPTz6FbgSnYaLt6aAkqQMOBOaLJPKQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxFTEVNRU5UVEVNUExBVEVTW0NJVElFUyBURU1QTEFURV0/attributetemplates";
		$result = PIWebAPIWrapper::MakeGetRequest ( $url );
		$itemsList = array ();
		$i = 0;
		foreach ( $result->Items as $item ) {
			$itemsList [$i] = $item->Name;
			$i = $i + 1;
		}
		return $itemsList;
	}
	public static function SendValue($cityName, $attributeName, $value) {
		$base_url = "https://osi-serv.osibr.com/piwebapi/";
		$url = $base_url . "attributes?path=\\\\MARC-PI2014\\AFPartnerCourseWeather\\Cities\\" . $cityName . "|" . $attributeName;
		$result = PIWebAPIWrapper::MakeGetRequest ( $url );
		$url_to_send = $result->Links->Value;
		$postData = "{'Value': " . $value . " }";
		$statusCode = PIWebAPIWrapper::MakePostRequest ( $url_to_send, $postData );
		return $statusCode;
	}
	private static function MakeGetRequest($url) {
		$ch = curl_init ( $url );
		curl_setopt ( $ch, CURLOPT_HEADER, false );
		curl_setopt ( $ch, CURLOPT_RETURNTRANSFER, true );
		curl_setopt ( $ch, CURLOPT_SSL_VERIFYPEER, false );
		// header('Content-type: application/json');
		$result = curl_exec ( $ch );
		$json_o = json_decode ( $result );
		return ($json_o);
	}
	private static function MakePostRequest($url, $postData) {
		$ch = curl_init ( $url );
		curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "POST");
		curl_setopt ( $ch, CURLOPT_POSTFIELDS, $postData );
		curl_setopt ( $ch, CURLOPT_RETURNTRANSFER, true);		
		curl_setopt ( $ch, CURLOPT_SSL_VERIFYPEER, false );
		curl_setopt($ch, CURLOPT_HTTPHEADER, array(
		'Content-Type: application/json',
		'Content-Length: ' . strlen($postData))
		);
					
		$result = curl_exec ( $ch );	
		$status = curl_getinfo($ch, CURLINFO_HTTP_CODE);
		
		curl_close($ch);		
		return (int) $status;
	}
}
?>