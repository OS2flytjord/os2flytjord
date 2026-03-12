
// **************************************************
// LAHA: lav en knap der kalder denne funktion
// og to metoder i en controller:
// - ExportToExcel(model, data, title) 
//        (denne laver en excelfil og gemmer den i sessionen)
// - GetExcelFile(title)
//        (denne henter filen fra sessionen)
//***************************************************
function ExportToExcel(gridName, controllerName) {

  var theGrid = $("#" + gridName);
  var theDataSource = theGrid.data("kendoGrid").dataSource;
  var theGridData = theGrid.data("kendoGrid").dataSource.data();


  // Create a datasource for the export data.
  var ds = new kendo.data.DataSource({
    data: theGridData
  });
  ds.query({
    aggregate: theDataSource._aggregate,
    filter: theDataSource._filter,
    sort: theDataSource._sort
  });


  // Define the data to be sent to the server to create the spreadsheet.
  var dataene = JSON.stringify(ds._view);
  data = {
    model: JSON.stringify(theGrid.data("kendoGrid").columns),
    data: dataene,
    title: gridName
  };

  if ( dataene != "undefined" & dataene != null & dataene.length > 19000000) {
    //Datamængden er forstår
    alert('Datamængden overskridet det maksimal tilladte!\nOpdel eventuel søgningen flere mindre søgninger.');
    
  }
  else {

    // Create the spreadsheet.
    $.ajax({
      type: "POST",
      url: "/" + controllerName + "/ExportToExcel",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      data: JSON.stringify(data)
    })
        .done(function (e) {
          // Download the spreadsheet.
          window.location = "/" + controllerName + "/GetExcelFile?title=" + gridName;
        });
  }



}