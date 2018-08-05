using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GPXSplit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            inpFilePath.Text = Properties.Settings.Default.savedInputFile;
            inpOutFilePath.Text = Properties.Settings.Default.savedOutputFile;
            if(Properties.Settings.Default.savedStartDateTime == DateTime.MinValue)
                Properties.Settings.Default.savedStartDateTime = DateTime.Now.AddDays(-1);
            if (Properties.Settings.Default.savedEndDateTime == DateTime.MinValue)
                Properties.Settings.Default.savedEndDateTime = DateTime.Now;
            Properties.Settings.Default.Save();

            inpStartDate.Value = Properties.Settings.Default.savedStartDateTime; ;
            inpEndDate.Value = Properties.Settings.Default.savedEndDateTime;
        }

        private void btnSelectInputFile_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "csv files|*.csv";
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    //TxtFile.Text = file;
                    inpFilePath.Text = file;
                    inpFilePath.ToolTip = file;
                    Properties.Settings.Default.savedInputFile = file.ToString();
                    Properties.Settings.Default.Save();
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    inpFilePath.Text = null;
                    inpFilePath.ToolTip = null;
                    break;
            }
        }

        private void btnSelectInputFile1_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var folder = fileDialog.SelectedPath;
                    //var file = fileDialog.FileName;
                    //TxtFile.Text = file;
                    inpOutFilePath.Text = folder.ToString();
                    inpOutFilePath.ToolTip = folder.ToString();
                    Properties.Settings.Default.savedOutputFile = folder.ToString();
                    Properties.Settings.Default.Save();
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    
                    inpOutFilePath.Text = null;
                    inpOutFilePath.ToolTip = null;
                    break;
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            inpStatus.Text = string.Empty;

            var inputFile = Properties.Settings.Default.savedInputFile;
            var outputFile = Properties.Settings.Default.savedOutputFile;
            var startDateTime = Properties.Settings.Default.savedStartDateTime;
            var endDateTime = Properties.Settings.Default.savedEndDateTime;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine(inputFile);
            builder.AppendLine(outputFile);
            builder.AppendLine(startDateTime.ToString());
            builder.AppendLine(endDateTime.ToString());
            
            var doc = new gpx();
            doc.loadFile(inputFile, startDateTime, endDateTime);
            doc.outputFile(outputFile);

            builder.AppendLine("Output written to "+ outputFile + "\\"+ "output.gpx");
            inpStatus.Text = builder.ToString();

        }

        private void inpStartDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Properties.Settings.Default.savedStartDateTime = (DateTime)inpStartDate.Value;
            Properties.Settings.Default.Save();
        }

        private void inpEndDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Properties.Settings.Default.savedEndDateTime = (DateTime)inpEndDate.Value;
            Properties.Settings.Default.Save();
        }
    }
}
