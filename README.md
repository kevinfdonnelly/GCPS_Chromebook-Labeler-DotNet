# GCPS Chromebook Labeler - .NET/WPF

Native Windows .NET 8/WPF version for a DYMO LabelWriter 450 Twin Turbo. No student or label data is stored.

## Labels

- Student/Chromebook: DYMO 30336 (1" x 2-1/8"), RIGHT roll, Name, Student ID, Chromebook ID, Student ID barcode, County ID barcode, County ID.
- County ID: DYMO 30332 (1" x 1"), LEFT roll, County ID barcode and County ID.
- Barcodes in the supplied templates use Code 128 Auto.

## Requirements

- Windows 11
- .NET 8 SDK
- Visual Studio 2022 with **.NET desktop development** workload, or the .NET CLI
- DYMO software/drivers
- DYMO LabelWriter 450 Twin Turbo

The project references DYMO Connect SDK 1.6.0 and its Windows packages.

## Build and run

```powershell
dotnet restore
dotnet build
dotnet run
```

Or open `GCPS_Chromebook_Labeler.csproj` in Visual Studio and press F5.

## Template object names

`Labels\student_30336.dymo`:

```text
NAME
STUDENT_ID_TEXT
CHROMEBOOK_ID
STUDENT_BARCODE
COUNTY_BARCODE
COUNTY_ID_TEXT
```

`Labels\county_30332.dymo`:

```text
COUNTY_BARCODE
COUNTY_ID_TEXT
```

The current DYMO 1.6.0 WPF sample uses Twin Turbo roll values `0=Auto`, `1=Left`, `2=Right`. This project therefore sends 30332 to `2` and 30336 to `1`.

## Publish

After printer testing succeeds:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -o publish
```

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish-singlefile
```
