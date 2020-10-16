using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities
{
    public class ProgressBinder : BindableBase
    {
        private double _MaxValue;
        public double MaxValue
        {
            get { return this._MaxValue; }
            set { SetProperty(ref _MaxValue, value); }
        }

        private double _ProgressValue;
        public double ProgressValue
        {
            get { return this._ProgressValue; }
            set { SetProperty(ref _ProgressValue, value); }
        }


        private bool _Visible;
        public bool Visible
        {
            get { return this._Visible; }
            set { SetProperty(ref _Visible, value); }
        }

        public CancellationTokenSource CancellationToken { get; set; }

        public const double ProgressFactor = 0.5d;

        public ProgressBinder(double maxRecords) : this()
        {
            Visible = maxRecords > 0;
            if (Visible)
                MaxValue = maxRecords * ProgressFactor;
        }
        public ProgressBinder()
        {
            Visible = false;
            CancellationToken = new CancellationTokenSource();
        }

        public void ReportProgress()
        {
            ProgressValue += ProgressFactor;
        }

        public void ReportProgress(int NoOfProcessedRecords)
        {
            ProgressValue += NoOfProcessedRecords * ProgressFactor;
        }

        public void Reset()
        {
            MaxValue = 0;
            ProgressValue = 0;
            Visible = false;
        }
    }
}
