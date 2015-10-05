import java.util.List;

public class Application {
	public static void main(String[] args) 
	{
	List<String> citiesNames = PIWebAPIWrapper.GetCitiesName();
    List<String> attributesNames = PIWebAPIWrapper.GetAttributesName();
    String value = "1320";
    int statusCode = PIWebAPIWrapper.SendValue(citiesNames.get(0), attributesNames.get(1), value);
  
	    if (statusCode == 202)
	    {
	    	System.out.println("Success!");
	    }
	    else
	    {
	    	System.out.println("Error");
	    }
	}
}
