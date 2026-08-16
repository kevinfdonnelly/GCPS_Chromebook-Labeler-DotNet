using System.Windows;
using System.Windows.Media;
using GCPS_Chromebook_Labeler.Services;
namespace GCPS_Chromebook_Labeler;
public partial class MainWindow : Window
{
    private readonly DymoPrinterService _printerService;
    public MainWindow(){ InitializeComponent(); _printerService = new DymoPrinterService(); Loaded += (_, _) => RefreshPrinterStatus(); StudentNameTextBox.Focus(); }
    private void RefreshPrintersButton_Click(object sender, RoutedEventArgs e) => RefreshPrinterStatus();
    private void RefreshPrinterStatus(){ try { var p=_printerService.FindTwinTurboPrinter(); if(p is null){PrinterStatusText.Text="No DYMO LabelWriter printer was found.";PrinterStatusText.Foreground=Brushes.DarkRed;} else {PrinterStatusText.Text=$"Ready: {p.Name}";PrinterStatusText.Foreground=Brushes.DarkGreen;} } catch(Exception ex){PrinterStatusText.Text=$"DYMO error: {ex.Message}";PrinterStatusText.Foreground=Brushes.DarkRed;} }
    private async void PrintStudentButton_Click(object sender,RoutedEventArgs e){ var name=StudentNameTextBox.Text.Trim();var sid=StudentIdTextBox.Text.Trim();var cb=ChromebookIdTextBox.Text.Trim();var cid=StudentCountyIdTextBox.Text.Trim(); if(new[]{name,sid,cb,cid}.Any(string.IsNullOrWhiteSpace)){SetStudent("Please complete all four fields.",true);return;} try{PrintStudentButton.IsEnabled=false;SetStudent("Printing...",false);await _printerService.PrintStudentLabelAsync(name,sid,cb,cid);SetStudent("Student label sent to the LEFT roll.",false);StudentNameTextBox.Clear();StudentIdTextBox.Clear();ChromebookIdTextBox.Clear();StudentCountyIdTextBox.Clear();StudentNameTextBox.Focus();}catch(Exception ex){SetStudent(ex.Message,true);}finally{PrintStudentButton.IsEnabled=true;} }
    private async void PrintCountyButton_Click(object sender,RoutedEventArgs e){var cid=CountyIdTextBox.Text.Trim();if(string.IsNullOrWhiteSpace(cid)){SetCounty("Please enter a County ID.",true);return;}try{PrintCountyButton.IsEnabled=false;SetCounty("Printing...",false);await _printerService.PrintCountyLabelAsync(cid);SetCounty("County ID label sent to the RIGHT roll.",false);CountyIdTextBox.Clear();CountyIdTextBox.Focus();}catch(Exception ex){SetCounty(ex.Message,true);}finally{PrintCountyButton.IsEnabled=true;}}
    private void SetStudent(string m,bool err){StudentMessageText.Text=m;StudentMessageText.Foreground=err?Brushes.DarkRed:Brushes.DarkGreen;}
    private void SetCounty(string m,bool err){CountyMessageText.Text=m;CountyMessageText.Foreground=err?Brushes.DarkRed:Brushes.DarkGreen;}
}
