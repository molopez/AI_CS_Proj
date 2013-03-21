using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_project
{
    class City
    {
        private string cityName, previousCity;
        private int xCoordinate, yCoordinate;
        private double distanceTraveled;
        private bool visited, omitted, deadEnd;
        private Dictionary<string, double> Neighbor = new Dictionary<string, double>();

        public City() // Constructor
        {
	
        }

        public City(string cityName, int x, int y)
        {
	        this.cityName = cityName;
	        xCoordinate = x;
	        yCoordinate = y;
	        visited = false;
	        omitted = false;
	        deadEnd = false;
	        distanceTraveled = 0;
        }

        #region public methods
        public void addNeighbor(string cityName, int x, int y) 
        {
	        Neighbor.Add(cityName, calculateDistance(x,y));
        }

        public void setVisit(bool visited)
        {
	        this.visited = visited;
        }

        public void setOmission(bool omitted)
        {
	        this.omitted = omitted;
        }

        public void setDeadEnd(bool deadEnd)
        {
	        this.deadEnd = deadEnd;
        }

        public void setDistanceTraveled(double distance)
        {
	        distanceTraveled = distance;
        }

        public void setPreviousCity(string city)
        {
	        previousCity = city;
        }

        public bool getVisit()
        {
	        return visited;
        }

        public bool getOmmission()
        {
	        return omitted;
        }

        public bool getDeadEnd()
        {
	        return deadEnd;
        }

        public string getPreviousCity()
        {
	        return previousCity;
        }

        public double getDistanceTraveled()
        {
	        return distanceTraveled;
        }

        public override string ToString()
        {
	        return cityName + " " + xCoordinate + " " + yCoordinate;
        }

        public string getCityName()
        {
	        return cityName;
        }

        public void printNeighbors()
        {
	        string neighbor = cityName + " neighbors: ";

	        foreach (KeyValuePair<string, double>neigh in Neighbor)
	        {
                Console.WriteLine(neigh.ToString());
	        }
        }

        public int getXCoordinate()
        {
	        return xCoordinate;
        }

        public int getYCoordinate()
        {
	        return yCoordinate;
        }

        public Dictionary<string, double> getNeighbors()
        {
	        return Neighbor;
        }

        #endregion

        #region private methods
        private double calculateDistance(int x, int y)
        {
            return Math.Sqrt(Math.Pow(xCoordinate - x, 2) + Math.Pow(yCoordinate - y, 2));
        }

        #endregion
    }
}
