using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace kmeans_clustering
{
    public class Coordinate
    {
        int x;
        int y;


        // basic coordinate
        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // setters and getters 
        public int GetX() { return x; }
        public int GetY() { return y; }

        // measures the distance between two coordinate 
        public double Measure(Coordinate k)
        {
            double power = 2;
            return (double)Math.Sqrt(Math.Pow(this.x - k.x, power) + Math.Pow(this.y - k.y, power));
        }

    }

    public class KMeans{

        int[] x;
        int[] y;
        // number of clusters
        // which number is reasonable... on which data set
        int k;
        public List<Coordinate> dataset = new List<Coordinate>();
        List<Coordinate> clusterPoints = new List<Coordinate>();
        List<int> labels = new List<int>();

        public KMeans(int[]x, int[]y, int k)
        {
            this.x = x;
            this.y = x;
            this.k = k;

        }


        // calculations 
        public void Calculations(Random rnd)
        {
            //forms the coordinates 
            FormDataSet();

            bool notready;

            // random point between a reasonable range 
            clusterPoints = RandomClusters(rnd);

            do
            {
                notready = false;

                // measures the distance between the clusterpoints and the data points
                // returns the labels 
                List<int> labels = Measure(dataset, clusterPoints);


                // calculate the mean of each group and returns the new clusterpoints
                List<Coordinate> newClusterpoints =  AvrageCalculations(labels);

                // measure again with the avg coordinates
                //returns the new labels
                List<int> newLabels = Measure(dataset, newClusterpoints);


                // see if there is a convergence
                // better -> Math.Pow ( (newClusterPoints-ClusterPoints), 2  )  < 
                for (int i = 0; i < clusterPoints.Count(); i++)
                {
                    if (clusterPoints[i] != newClusterpoints[i])
                    {
                        notready = true;
                        break;
                    }

                }

                if (notready)
                {
                    // replace the old coordinates with the new ones 
                    clusterPoints = newClusterpoints;
                }
                else
                { // just for the output 
                    labels = newLabels;
                }


            } while (notready);

        }

        

        // generate the coordinates 
        private void FormDataSet()
        {
            for (int i = 0; i < x.Count(); i++)
            { dataset.Add(new Coordinate(x[i], y[i])); }
        }

        // generates the init random clusters
        private List<Coordinate> RandomClusters(Random rnd)
        {
            List<Coordinate> randomPoints = new List<Coordinate>();
            int x_max = x.Max();
            int y_max = y.Max();
            for (int i = 0; i < k; i++)
            { randomPoints.Add(new Coordinate(rnd.Next(-1, x_max), rnd.Next(-1, y_max))); }

            return randomPoints;
        }

        // measures the distance between the datapoint and every cluster and returns the labels 
        public List<int> Measure(List<Coordinate> data, List<Coordinate> possibleClusters)
        {
            List<double> distances = new List<double>();
            List<int> labels = new List<int>();

            // loop through all the possbible coordinates
            for (int i = 0; i < data.Count; i++)
            {

                // measure the distance between the clusterpoint and the data point one by one 
                for (int j = 0; j < clusterPoints.Count; j++)
                {
                    distances.Add(data[i].Measure(possibleClusters[j]));
                }

                // choose the right cluster with the least distance and labels it 
                labels.Add(distances.IndexOf(distances.Min()));
                distances.Clear();
  
            }

            return labels;
        }


        // calculates the avrage in every cluster
        private List<Coordinate> AvrageCalculations(List<int> labels)
        {
            List<Coordinate> avr = new List<Coordinate>();
            for (int i = 0; i <= clusterPoints.Count; i++)
            {
                List<int> avgX = new List<int>();
                List<int> avgY = new List<int>();

                for (int j = 0; j <= dataset.Count - 1; j++)
                {
                    if (labels[j] == i)
                    {
                        avgX.Add(dataset[i].GetX());
                        avgY.Add(dataset[i].GetY());
                    }

                }
                avr.Add(new Coordinate((int)avgX.Average(), (int)avgY.Average()));
            }

            return avr;
        }

        // just shows input & output 
        public void Show()
        {

            Console.WriteLine("All cluster points:");
            for (int i = 0; i < clusterPoints.Count; i++)
            {
                var item = clusterPoints[i];
                Console.WriteLine("The number of clusterpoint is " + i + " x =  " + item.GetX() + "  y=  " + item.GetY());
            }

            Console.WriteLine();
            Console.WriteLine("All the data points");
            for (int i = 0; i < dataset.Count; i++)
            {
                Console.WriteLine("The number of the coordinate is " + i);
                Console.WriteLine("x = " + dataset[i].GetX() + "  y = " + dataset[i].GetY() + "  and the group number is " + labels[i]);
            }
        }


    }
    class Program
    {
        
        static void Main(string[] args)
        {

            Random rnd = new Random();

            //smaller test data
            // everything is based on positive coordinates 
            int[] point_x =
            {1, 2, 4, 5, 10, 12};
            int[] point_y =
            {1, 1, 3, 4 , 8, 12};

            int k = 3; 



            KMeans test = new KMeans(point_x, point_y, k);
            if (point_x.Length == point_y.Length)
            {
                test.Calculations(rnd);
                test.Show();
            }
            else { Console.WriteLine("Fault in the dataset"); }

            /*  actual data set 
            int[] x = { 25, 34, 22, 27, 33, 33, 31, 22, 35, 34, 67, 54, 57, 43, 50, 57, 59, 52, 65, 47, 49, 48, 35, 33, 44, 45, 38, 43, 51, 46 };
            int[] y = { 79, 51, 53, 78, 59, 74, 73, 57, 69, 75, 51, 32, 40, 47, 53, 36, 35, 58, 59, 50, 25, 20, 14, 12, 20, 5, 29, 27, 8, 7 };
            int clusters = 7;

            KMeans actualSet = new KMeans(x, y, clusters);
            if (x.Length == y.Length)
            {
                actualSet.Calculations(rnd);
                actualSet.Show();
            }
            else { Console.WriteLine("Fault in the dataset"); } */


        }
    }



  

}


