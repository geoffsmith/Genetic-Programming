using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Frontend
{
    class ExperimentData
    {
        public List<double> RunFitnessesImprovements = new List<double>();
        public List<double> ExperimentFitnesses = new List<double>();

        public TextBlock Text { get; set; }

        public void Update()
        {
            if (this.Text == null) return;

            this.Text.Dispatcher.BeginInvoke((Action) delegate
            {
                this.Text.Text = String.Format("Average fitness improvement: {0}, Average run fitness: {1}",
                                               this.ImprovementsAverage(), this.ExperimentAverage());
            });
        }

        private double ExperimentAverage()
        {
            if (this.ExperimentFitnesses.Count == 0) return 0;
            return this.ExperimentFitnesses.Average();
        }

        private double ImprovementsAverage()
        {
            if (this.RunFitnessesImprovements.Count == 0) return 0;
            else return this.RunFitnessesImprovements.Average();
        }
    }
}
