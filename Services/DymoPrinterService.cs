using System.IO;
using DymoSDK.Implementations;
using DymoSDK.Interfaces;
namespace GCPS_Chromebook_Labeler.Services;
public sealed class DymoPrinterService
{
    // Current DYMO SDK 1.6.0 WPF sample: 0=Auto, 1=Left, 2=Right.
    private const int LeftRoll=1;
    private const int RightRoll=2;
    private readonly string _labelsDirectory;
    public DymoPrinterService(){ DymoSDK.App.Init(); _labelsDirectory=Path.Combine(AppContext.BaseDirectory,"Labels"); }
    public IPrinter? FindTwinTurboPrinter(){var printers=DymoPrinter.Instance.GetPrinters()?.ToList()??new List<IPrinter>();return printers.FirstOrDefault(p=>p.Name.Contains("450 Twin Turbo",StringComparison.OrdinalIgnoreCase))??printers.FirstOrDefault(p=>p.Name.Contains("Twin Turbo",StringComparison.OrdinalIgnoreCase))??printers.FirstOrDefault();}
    public Task<bool> PrintStudentLabelAsync(string name,string studentId,string chromebookId,string countyId)=>PrintAsync("student_30336.dymo",new Dictionary<string,string>{{"TextObject0",name},{"TextObject1",studentId},{"TextObject3",chromebookId},{"BarcodeObject0",studentId},{"BarcodeObject1",countyId},{"TextObject2",countyId}},LeftRoll);
    public Task<bool> PrintCountyLabelAsync(string countyId)=>PrintAsync("county_30332.dymo",new Dictionary<string,string>{{"BarcodeObject0",countyId},{"TextObject12",countyId}},RightRoll);
    private async Task<bool> PrintAsync(string templateFile,IReadOnlyDictionary<string,string> values,int rollSelected){var printer=FindTwinTurboPrinter()??throw new InvalidOperationException("No DYMO LabelWriter printer was found. Verify the printer is connected and DYMO software/drivers are installed.");var path=Path.Combine(_labelsDirectory,templateFile);if(!File.Exists(path))throw new FileNotFoundException($"Label template was not found: {path}",path);IDymoLabel label=DymoLabel.LabelSharedInstance;label.LoadLabelFromFilePath(path);var objects=label.GetLabelObjects().ToList();foreach(var pair in values){var obj=objects.FirstOrDefault(o=>string.Equals(o.Name,pair.Key,StringComparison.OrdinalIgnoreCase));if(obj is null)throw new InvalidOperationException($"The label template does not contain an object named '{pair.Key}'.");if(!label.UpdateLabelObject(obj,pair.Value))throw new InvalidOperationException($"DYMO could not update label object '{pair.Key}'.");}var result=await DymoPrinter.Instance.PrintLabel(label,printer.Name,copies:1,collate:false,mirror:false,rollSelected:rollSelected,chainMarks:false,barcodeGraphsQuality:true);if(!result)throw new InvalidOperationException("DYMO reported that the label could not be printed.");return result;}
}
