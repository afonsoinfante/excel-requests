excel-json-client
=================

A thin Excel client Add-In to retrieve JSON data in Excel.
Uses Excel-DNA and is written in VB.NET.
Check out http://optionexplicitvba.com/2014/05/09/developing-a-json-excel-add-in-with-vb-net/ for an overview.


Requirements
------------
- Windows
- Excel (2007+)
- .NET (4.0+)


Exposed Excel Functions
-----------------------
DISTINCT(array): remove duplicates from a 1/2-d array
OBJ_GET(url): get value from a URL (assumes that the url points to a JSON)
OBJ_FIELDS(url): list fields of an object
OBJ_CLEAR(): clear object cache

Example
-------
An example spreadsheet is available at [https://spreadgit.com/bjoern/excel-json-client/sheets/json](https://spreadgit.com/bjoern/excel-json-client/sheets/json). It shows the basic functionality by retrieving data from the [http://www.quandl.com](http://www.quandl.com) API.

Usage
-----
Open Excel and drag & drop or file/open the XLL in your Excel session (or install via the Add-In manager)

### Excel 32-bit
Use excel-json-client32.xll

### Excel 64-bit
Use excel-json-client64.xll


License & Authors
-----------------
- Author: Bjoern Stiel (<bjoern.stiel@pathio.com>) (@path_io)