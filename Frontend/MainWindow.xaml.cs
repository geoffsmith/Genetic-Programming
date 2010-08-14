using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GeneticProgramming;
using GeneticProgramming.FitnessFunctions;
using GeneticProgramming.Interfaces;
using GeneticProgramming.NodeMutators;
using GeneticProgramming.Populations;
using GeneticProgramming.Selection;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;

namespace Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private readonly ObservableDataSource<Point> data1 = new ObservableDataSource<Point>();

        private void Window_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this.data1.SetXYMapping(p => p);
            //this.chart.AddLineGraph(this.data1, "Best Fitness");

            // Kick off the simulation
            Thread gp = new Thread(this.GeneticProgrammingLoop) {IsBackground = true};
            gp.Start();
        }

        private void GeneticProgrammingLoop()
        {
            int populationSize = 100;
            int generations = 500;
            this.standardData.Text = this.standardText;
            this.twoIslandsData.Text = this.twoIslandText;
            foreach (int i in Enumerable.Range(0, 50))
            {
                ObservableDataSource<Point> data = new ObservableDataSource<Point>();
                data.SetXYMapping(p => p);
                this.Dispatcher.Invoke((Action) delegate
                {
                    this.chart.AddLineGraph(data, Colors.Red, 1, "Standard");
                });
                IPopulation population = new Population(populationSize * 2, new StandardDeviationFitnessFunction(),
                                                       new SimpleReproduction(),
                                                       new SimpleNodeMutator(), new ProbabilisticSelection());
                int run = this.DoLengthRun(data, generations, populationSize * 2, population, this.standardData);
                this.standardRuns.Add(run);

                this.Dispatcher.BeginInvoke((Action) delegate
                {
                    this.standardAverageText.Text = String.Format("{0} (fails: {1})", this.ListAverage(this.standardRuns), this.standardRuns.Count(g => g == generations));
                });

                data = new ObservableDataSource<Point>();
                data.SetXYMapping(p => p);
                this.Dispatcher.Invoke((Action) delegate
                {
                    this.chart.AddLineGraph(data, Colors.Blue, 1, "Two islands");
                });
                population = new TwoIslandsPopulation(populationSize, new StandardDeviationFitnessFunction(),
                                                       new SimpleReproduction(),
                                                       new SimpleNodeMutator(), new ProbabilisticSelection());
                run = this.DoLengthRun(data, generations, populationSize, population, this.twoIslandsData);
                this.twoIslandsRuns.Add(run);

                this.Dispatcher.BeginInvoke((Action) delegate
                {
                    this.twoIslandAverageText.Text = String.Format("{0} (Fails: {1})", this.ListAverage(this.twoIslandsRuns), this.twoIslandsRuns.Count(g => g == generations));
                });
            }
        }

        private void MutationRateExperiment()
        {
            List<int> mutationRates = new List<int>{1,5,25,50};
            foreach (int mutationRate in mutationRates)
            {
                ObservableDataSource<Point> data = new ObservableDataSource<Point>();
                data.SetXYMapping(p => p);
                this.Dispatcher.Invoke((Action) delegate
                {
                    this.chart.AddLineGraph(data, String.Format("Mutation at {0}", mutationRate));
                });
                this.DoExperimentOnMutationRate(data, mutationRate);

            }
        }

        private void PopulationSizeExperiment()
        {
            List<int> populationSizes = new List<int>{10,50,100,250,500,1000};
            foreach (int populationSize in populationSizes)
            {
                ObservableDataSource<Point> data = new ObservableDataSource<Point>();
                data.SetXYMapping(p => p);
                this.Dispatcher.Invoke((Action) delegate
                {
                    this.chart.AddLineGraph(data, String.Format("Population Size: {0}", populationSize));
                });
                this.DoExperimentOnPopulationSize(data, populationSize);

            }
        }

        private void DoExperimentOnMutationRate(ObservableDataSource<Point> data, int mutationRate)
        {
            IPopulation population = new TwoIslandsPopulation(100, new StandardDeviationFitnessFunction(), new SimpleReproduction(),
                                                   new SimpleNodeMutator(mutationRate), new RouletteSelection());
            for (int i = 0; i < 100; ++i)
            {
                population.ProcessGeneration();

                // Show a new node on the graph
                data.AppendAsync(this.Dispatcher, new Point(i, population.BestFitness));

                this.Dispatcher.BeginInvoke((Action) delegate
                {
                    this.bestIndiviualRepresentation.Text = population.BestIndividual.ToString();
                    this.generationTextBlock.Text = i.ToString();
                    this.bestFitnessTextblock.Text = population.BestFitness.ToString();
                });
            }
        }

        private void DoExperimentOnPopulationSize(ObservableDataSource<Point> data, int populationSize)
        {
            Population population = new Population(populationSize, new StandardDeviationFitnessFunction(),
                                                   new SimpleReproduction(),
                                                   new SimpleNodeMutator(), new RouletteSelection());
            for (int i = 0; i < 100; ++i)
            {
                population.ProcessGeneration();

                // Show a new node on the graph
                data.AppendAsync(this.Dispatcher, new Point(i, population.BestFitness));

                this.Dispatcher.BeginInvoke((Action) delegate
                {
                    this.bestIndiviualRepresentation.Text = population.BestIndividual.ToString();
                    this.generationTextBlock.Text = i.ToString();
                    this.bestFitnessTextblock.Text = population.BestFitness.ToString();
                });
            }
        }

        private int DoLengthRun(ObservableDataSource<Point> data, int generations, int populationSize, IPopulation population,
                                ExperimentData experimentData)
        {
            double? lastFitness = null;

            for (int i = 0; i < generations; ++i)
            {
                population.ProcessGeneration();

                if (lastFitness.HasValue)
                {
                    experimentData.RunFitnessesImprovements.Add(lastFitness.Value - population.BestFitness);
                    experimentData.Update();
                }
                lastFitness = population.BestFitness;

                // Show a new node on the graph
                data.AppendAsync(this.Dispatcher, new Point(i, population.BestFitness));

                this.Dispatcher.BeginInvoke((Action) delegate
                {
                    this.bestIndiviualRepresentation.Text = population.BestIndividual.ToString();
                    this.generationTextBlock.Text = i.ToString();
                    this.bestFitnessTextblock.Text = population.BestFitness.ToString();
                });

                // Stop if done
                if (population.BestFitness < Double.Epsilon)
                {
                    return i;
                }
            }

            experimentData.ExperimentFitnesses.Add(population.BestFitness);
            experimentData.Update();

            return generations;
        }

        private List<int> standardRuns = new List<int>();
        private List<int> twoIslandsRuns = new List<int>(); 

        private ExperimentData standardData = new ExperimentData();
        private ExperimentData twoIslandsData = new ExperimentData();


        private double ListAverage(List<int> data)
        {
            if (data.Count == 0) return 0;
            return data.Sum()/(double)data.Count;
        }
    }
}
