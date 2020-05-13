using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using RedSpiderTech.SecuritiesResearch.Charts.ChartData.Interface;
using RedSpiderTech.SecuritiesResearch.Charts.DataManager.Interface;
using RedSpiderTech.SecuritiesResearch.Charts.EventManager.Interface;

namespace RedSpiderTech.SecuritiesResearch.Charts
{
    public partial class Form1 : Form
    {
        private readonly IChartDataUIEventManager _chartDataUIEventManager;
        private readonly IValuationDataManager _valuationDataManager;

        public Form1(IChartDataUIEventManager chartDataUIEventManager, IValuationDataManager valuationDataManager)
        {
            InitializeComponent();

            lblChartInfo.Text = "";
            _chartDataUIEventManager = chartDataUIEventManager;
            _valuationDataManager = valuationDataManager;
            
            if(!File.Exists(ConfigurationManager.AppSettings["reportFile"]))
            {
                return;
            }

            tbDirectory.Text = ConfigurationManager.AppSettings["reportFile"];
            _chartDataUIEventManager.TriggerNewFileSelectedEvent(tbDirectory.Text);

            ReloadChartSeriesOptions();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Path.GetDirectoryName(ConfigurationManager.AppSettings["reportFile"])
            };

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbDirectory.Text = openFileDialog.FileName;
                _chartDataUIEventManager.TriggerNewFileSelectedEvent(openFileDialog.FileName);
            }

            ReloadChartSeriesOptions();
        }

        private void ReloadChartSeriesOptions()
        {
            lbSeries.Items.Clear();
            foreach (string chartSeries in _valuationDataManager.ChartDataSeriesCollection.Select(x => x.ChartSeriesName))
            {
                lbSeries.Items.Add(chartSeries);
            }

            chart1.Series.Clear();
        }

        private void lbSeries_SelectedValueChanged(object sender, EventArgs e)
        {
            ReloadChart();
        }

        private void ReloadChart()
        {
            chart1.Series.Clear();
            foreach (IChartDataSeries<DateTime> holdings in _valuationDataManager.ChartDataSeriesCollection)
            {
                if (lbSeries.SelectedItems.Contains(holdings.ChartSeriesName))
                {
                    AddOrInvokeChartSeries(holdings, holdings.SeriesWeight);
                }
            }
        }

        private void AddOrInvokeChartSeries<T>(IChartDataSeries<T> chartDataSeries, int borderWidth)
        {
            if (chart1.InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    AddNewChartSeries(chartDataSeries, borderWidth);
                }));
            }
            else
            {
                AddNewChartSeries(chartDataSeries, borderWidth);
            }
        }

        private void AddNewChartSeries<T>(IChartDataSeries<T> chartDataSeries, int borderWidth)
        {
            Series series = chart1.Series.Add(chartDataSeries.ChartSeriesName);

            series.ChartType = SeriesChartType.Line;
            foreach (IChartDataPoint<T> dataPoint in chartDataSeries.DataPoints)
            {
                series.Points.AddXY(dataPoint.XData, dataPoint.YData / 100);
            }

            series.BorderWidth = borderWidth;
        }

        private void Chart1_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult hit = chart1.HitTest(e.X, e.Y);
            if (hit.PointIndex >= 0)
            {
                var xv = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                var yv = chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);
                lblChartInfo.Text = "Value: " + yv;
            }
        }
    }
}
