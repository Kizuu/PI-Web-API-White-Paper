import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.util.ArrayList;
import java.util.List;
import org.apache.http.*;
import org.apache.http.client.*;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.HttpClientBuilder;
import org.apache.http.util.EntityUtils;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;

public class PIWebAPIWrapper {
	private static String base_url = "https://osi-serv.osibr.com/piwebapi/";

	public static List<String> GetCitiesName() {
		List<String> items = new ArrayList<String>();
		String url = base_url
				+ "elements/E07dtyl5PO4EmFExFPTz6FbghNYw-7Q65Uav8IhHRRw_WQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxDSVRJRVM/elements";
		JSONArray citiesItems = (JSONArray) MakeGetRequest(url).get("Items");
		for (int i = 0; i < citiesItems.size(); i++) {
			JSONObject myCity = (JSONObject) citiesItems.get(i);
			items.add(myCity.get("Name").toString());
		}
		return items;
	}

	public static List<String> GetAttributesName() {
		List<String> items = new ArrayList<String>();
		String url = base_url
				+ "elementtemplates/T07dtyl5PO4EmFExFPTz6FbgSnYaLt6aAkqQMOBOaLJPKQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxFTEVNRU5UVEVNUExBVEVTW0NJVElFUyBURU1QTEFURV0/attributetemplates";
		JSONArray citiesItems = (JSONArray) MakeGetRequest(url).get("Items");
		for (int i = 0; i < citiesItems.size(); i++) {
			JSONObject myCity = (JSONObject) citiesItems.get(i);
			items.add(myCity.get("Name").toString());
		}
		return items;
	}

	public static int SendValue(String cityName, String attributeName,
			String value) {
		String attributePath = "\\\\MARC-PI2014\\AFPartnerCourseWeather\\Cities\\"
				+ cityName + "|" + attributeName;
		String url = null;
		try {
			url = base_url + "attributes?path="
					+ URLEncoder.encode(attributePath, "UTF-8");
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		}
		JSONObject linkItems = (JSONObject) MakeGetRequest(url).get("Links");
		String url_to_send = linkItems.get("Value").toString();
		try {
			return MakePostRequest(url_to_send, value);
		} catch (IOException e) {

			e.printStackTrace();
			return -1;
		}
	}


	private static JSONObject MakeGetRequest(String url) {
		JSONObject myJSONObject = null;
		try {
			HttpClient client = HttpClientBuilder.create().build();
			HttpGet request = new HttpGet(url);				
			HttpResponse response = client.execute(request);
			HttpEntity entity = response.getEntity();
			String responseString = EntityUtils.toString(entity);

			JSONParser myJSONParser = new JSONParser();
			myJSONObject = (JSONObject) myJSONParser.parse(responseString);

		} catch (Exception ex) {

		}
		return myJSONObject;
	}

	private static int MakePostRequest(String url, String value)
			throws ClientProtocolException, IOException {
		HttpClient client = HttpClientBuilder.create().build();
		HttpPost post = new HttpPost(url);
		post.setHeader("Content-Type", "application/json");
		post.setEntity(new StringEntity("{\"Value\":" + value + "}"));
		HttpResponse response = client.execute(post);
		int responseCode = response.getStatusLine().getStatusCode();
		return responseCode;
	}
}
