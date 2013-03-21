using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AI_project
{
     class Map
    {
        private List<string> locations = new List<string>();
        private List<string> connections = new List<string>();
        private List<City> cities = new List<City>();
        private List<string> path = new List<string>();
        private Dictionary<string, double> heuristics = new Dictionary<string, double>();
        private string startCity, endCity, omitCity;
        bool pathFound;

        public Map() { }

        public Map(string locations, string connections)
        {
            setLocations(locations);
            setConnections(connections);
        }

        #region public methods
        public void buildMap(string locations, string connections)
        {
            setLocations(locations);
            setConnections(connections);
        }

        public void buildPath()
        {
            City start, end, nextCity;

            end = getCity(endCity);
            start = getCity(startCity);
            nextCity = end;

            while (nextCity.getCityName() != start.getCityName())
            {
                path.Add(nextCity.getCityName());
                nextCity = getCity(nextCity.getPreviousCity());
            }
            path.Add(nextCity.getCityName());
        }

        public void mapCities() //This function sets the locations of cities in a map
        {
            string cityName, temp, neighbor; // neighborCity, tempNeighbor;
            //int locX, locY, numNeighbors, tempLoc; 
            int numNeighbors;


            //Building vector of cities
            //initializing the cities using the conections file first
            for (int i = 0; i < locations.Count; i++)
            {
                temp = locations[i];
                //cout << temp << " " ;

                if (temp.CompareTo("END") == 0)
                    break;

                string[] str = temp.Split(' ');

                City city = new City(str[0], int.Parse(str[1]), int.Parse(str[2]));
                cities.Add(city);

                //cout << cityName << " " << locX << " " << locY << endl;
            }

            /*cout << "verify vector of cities created properly:" << endl;
            for (itCity = cities.begin() ; itCity != cities.end(); ++itCity)
            {
                (*itCity).toString();
            }*/

            //add adjacent cities to each city
            //iterate through the connections list to add each adjacent(neighbor) city
            for (int i = 0; i < connections.Count; i++)
            {
                temp = connections[i];
                //cout << temp << " " ;

                if (temp.CompareTo("END") == 0)
                    break;

                string[] str = temp.Split(' ');
                cityName = str[0];
                numNeighbors = int.Parse(str[1]);

                //find the city to add its neighbors
                foreach (City city in cities)
                {

                    temp = city.getCityName();

                    //if this is the city i am on, add its neighbors
                    if (temp.CompareTo(cityName) == 0)
                    {

                        //add the neighbors the city has
                        for (int j = 2; i < numNeighbors; j++)
                        {
                            neighbor = str[j];

                            //find the location of the neighbor
                            foreach (string myNeighbor in locations)
                            {
                                temp = myNeighbor;
                                string[] tNeighbor = temp.Split(' ');

                                /*ssTemp << (*itLoc);
                                ssTemp >> tempNeighbor;
                                ssTemp >> tempLoc; //only used to clear buffer
                                ssTemp >> tempLoc; //only way that I could make it work
                                ssTemp.clear(); //clear stream of any input that is left
                                */

                                //if this is the neighbor, add it to the city
                                if (tNeighbor[0].CompareTo(neighbor) == 0)
                                {
                                    /*ssLoc << (*itLoc);
                                    ssLoc >> neighborCity;
                                    ssLoc >> locX;
                                    ssLoc >> locY;
                                    ssLoc.clear();
                                    */

                                    city.addNeighbor(tNeighbor[0], int.Parse(tNeighbor[1]), int.Parse(tNeighbor[2]));
                                }
                            }
                        }
                    }
                }

                //ssConn.clear(); //clear stream to capture next input string
            }
        }

        public void printMap()
        {
            //cout << "verify neighbors of cities created properly:" << endl;
            foreach (City city in cities)
            {
                city.printNeighbors();
                //cout << endl;
            }
        }

        public int findPath(string start, string finish, string omit, string heuristicType)
        {
            if (heuristicType == "Shortest Distance")
            {
                return AStar_ShortestDistance(start, finish, omit);
            }
            else
                return AStar_FewestLinks(start, finish, omit);
        }

        public void showPath() // Display path found
        {
            int j;

            if (pathFound)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    j = i + 1;
                    if (j < path.Count)
                    {
                        Console.WriteLine(path[i] + " -> " + path[j]);
                    }
                }
            }
            else
            {
                Console.WriteLine("No path was found from: " + startCity + " to: " + endCity);
            }
        }

        public List<City> getCities()
        {
            return cities;
        }

        #endregion

        #region private methods
        private int setLocations(string fileName) //Opens file and capture data into an object
        {
            string line;
            
            StreamReader streamReader = new StreamReader(fileName);

            try
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    locations.Add(line);
                }
            }
            finally
            {
                streamReader.Close();
            }

            return 0;
        }

        private int setConnections(string fileName) //Opens a file containing locations and stores in an object
        {
            string line;

            StreamReader streamReader = new StreamReader(fileName);

            try
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    connections.Add(line);
                }
            }
            finally
            {
                streamReader.Close();
            }

            return 0;
        }

        private int AStar_ShortestDistance(string start, string finish, string omit)
        {
	        startCity = start; endCity = finish; omitCity = omit;

	        bool startExists = false, finishExists = false, omitExists = false;

	        clearCityProperties(); //clear any properties that the cities may have previously set

	        foreach (City city in cities)
	        {
		        if(city.getCityName() == start) //We got the initial city
		        {
			        city.setVisit(true);
			        startExists = true;	
		        }

		        if(city.getCityName() == finish) //We got the ending city
		        {
			        finishExists = true;			
		        }

		        if(city.getCityName() == omit) // We got the omited city
		        {
			        city.setOmission(true);
			        omitExists = true;
		        }
		        if(startExists && finishExists && omitExists)
			        break;
	        }

	        {// Response if one of the cities is not in the vector
		        if(!startExists)
		        {
			        Console.WriteLine(start + " is not found. Unable to proceed.");
			        Console.WriteLine("\nPress 'ENTER' to exit program.");
			        Console.Read();
			        Environment.Exit(-1);
		        }

		        if(!finishExists)
		        {
			        Console.WriteLine(finish + " is not found. Unable to proceed.");
			        Console.WriteLine("\nPress 'ENTER' to exit program.");
			        Console.Read();
			        Environment.Exit(-1);
		        }

		        if(!omitExists)
		        {
			        Console.WriteLine(finish + " is not found. Unable to proceed.");
		        }

	        }

	        string currentCity = start;
	        Dictionary<string, double> currentNeighbors;

	        //A* Algorithm functionality
	        while(currentCity != endCity)
	        {
		        City currCity = getCity(currentCity);	
		        currCity.setVisit(true);
		        updateVisited(currCity);

		        //get the adjacent cities to the current city that we are on
		        currentNeighbors = getNeighborCities(currentCity);

		        //iterate through the adjacent cities and set up the heuristics map structure
		        foreach (KeyValuePair<string, double> it in currentNeighbors)
		        {
			        //adds neighbors to the heuristics map structure
			        setupHeuristic(it.Key, it.Value, currentCity, endCity);
		        }

		        //if the heuristics map structure is empty
		        //then we did not find a path
		        if(heuristics.Count == 0)
		        {
			        pathFound = false;
			        return -1;
		        }
		
		        currentCity = getNextCity();
	        }

	        buildPath();
	        pathFound = true;
	        return 0;
        }

        private int AStar_FewestLinks(string start, string finish, string omit)
        {
            Console.WriteLine("not implemented yet!");
            return 0;
        }

        private double heuristicDistance(City a, City b) // Calculates stright line distance between two cities
        {
            return Math.Sqrt(Math.Pow((a.getXCoordinate() - b.getXCoordinate()), 2) + Math.Pow((a.getYCoordinate() - b.getYCoordinate()), 2));
        }

        private Dictionary<string, double> getNeighborCities(string cityName)
        {
            City city = getCity(cityName);

            if (city.getNeighbors().Count == 0)
            {
                city.setDeadEnd(true);
                updateDeadEnd(city);
            }

            return city.getNeighbors();
        }

        private void setupHeuristic(string neighbor, double distFromPrevCity, string prevCity, string endCity) //This function iterates over cities in a map to find the best heuristics
        {
	        double sld, dt, heuristicDist;
	        bool added = false;

	        City thisNeighbor = getCity(neighbor);									 //get the neighbor city

	        if(!thisNeighbor.getOmmission() && !thisNeighbor.getDeadEnd() && !thisNeighbor.getVisit())			 //verify that the city is not omitted or a deadend
	        {
		        City previousCity = getCity(prevCity);								 //get the previous city from neighbor

		        City endingCity = getCity(endCity);									 //get the ending city

		        sld = heuristicDistance(thisNeighbor, endingCity);					 //find the straigh line distance form the neighbor city to the ending city
		        dt = previousCity.getDistanceTraveled() + distFromPrevCity;			 //find the distance that would be traveled if this path is shosen paths
		        heuristicDist = sld + dt; //this is the distance used for our huristic
		
		        foreach (KeyValuePair<string, double> it in heuristics)
		        {
			        if(it.Key.CompareTo(neighbor) == 0)
			        {
				        if(it.Value > heuristicDist)
				        {
					        thisNeighbor.setPreviousCity(prevCity);								//let this neighbor know from which city we arrived to it
					        updatePreviousCity(thisNeighbor);
		
					        thisNeighbor.setDistanceTraveled(dt);								 //this is the total distances traveled through this path so far
					        updateDistanceTraveled(thisNeighbor);

					        added = true;
					        heuristics.Remove(neighbor);
					        heuristics.Add(neighbor, heuristicDist);		 //add the city and the heuristic distance to the map structure
																						         //that is holding our cities to choose a from to form the path
					        break;
				        }				
			        }
		        }

		        if(!added)
		        {

			        thisNeighbor.setPreviousCity(prevCity);								//let this neighbor know from which city we arrived to it
			        updatePreviousCity(thisNeighbor);
			
			        thisNeighbor.setDistanceTraveled(dt);								 //this is the total distances traveled through this path so far
			        updateDistanceTraveled(thisNeighbor);

			
			        heuristics.Add(neighbor, heuristicDist);		 //add the city and the heuristic distance to the map structure
																				         //that is holding our cities to choose a from to form the path

		        /*cout<< "city: " << neighbor << " prev: " <<prevCity << "\tdt: " << previousCity.getDistanceTraveled()
			        << "\tcity dt: " << dt << "\tsld: " <<sld << "\theurDist: " << heuristicDist << "\n" << endl; */
		        }	
	        }
        }

        private string getNextCity()
        {
            double min = 0; 
	        string nextCity = "";

            foreach (KeyValuePair<string, double> it in heuristics)
	        {
		        min = it.Value;
                nextCity = it.Key;
                break;
	        }

	        foreach (KeyValuePair<string, double> it in heuristics)
	        {
		        if(it.Value < min)
		        {
			        min = it.Value;
			        nextCity = it.Key;
		        }
	        }
	
	        if(heuristics.Count != 0)
	        {
		        heuristics.Remove(nextCity);
		        return nextCity;
	        }
	        else
		        return "NOCITY";
        }

        private City getCity(string cityName)
        {
	        City ct = cities[0];

            foreach (City city in cities)
	        {
                if (cityName.CompareTo(city.getCityName()) == 0)
		        {
			        ct = city;
			        break;
		        }
	        }

	        return ct;
        }

        private void updateVisited(City updateCity)
        {
            foreach (City city in cities)
	        {
                if (updateCity.getCityName().CompareTo(city.getCityName()) == 0)
		        {
			        city.setVisit(updateCity.getVisit());
			        break;
		        }
	        }
        }

        private void updatePreviousCity(City updateCity)
        {
            foreach (City city in cities)
	        {
                if (updateCity.getCityName().CompareTo(city.getCityName()) == 0)
		        {
			        city.setPreviousCity(updateCity.getPreviousCity());
			        break;
		        }
	        }
        }

        private void updateDistanceTraveled(City updateCity)
        {
	        foreach (City city in cities)
	        {
                if (updateCity.getCityName().CompareTo(city.getCityName()) == 0)
		        {
			        city.setDistanceTraveled(updateCity.getDistanceTraveled());
			        break;
		        }
	        }
        }

        private void updateDeadEnd(City updateCity)
        {
	        foreach (City city in cities)
	        {
		        if(updateCity.getCityName().CompareTo(city.getCityName()) == 0)
		        {
			        city.setVisit(updateCity.getDeadEnd());
			        break;
		        }
	        }
        }

        private void clearCityProperties()
        {
	        foreach (City city in cities)
	        {
		        city.setVisit(false);
		        city.setOmission(false);
		        city.setDeadEnd(false);
		        city.setDistanceTraveled(0);
		        city.setPreviousCity("");
	        }
        }

        #endregion
    }
}
