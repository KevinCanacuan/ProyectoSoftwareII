var idTabla = 0;
var optionsContainer;
fntGetCbxContainerUpload();
$(function () {
    var lastsel2;
    jQuery("#rowed5").jqGrid({
        datatype: "local", height: 250, colNames: ['ID', 'Nombre', 'Nombre Alterno', 'Tipo Archivo', 'Empresa', 'Contenedor', 'Aplicativo', 'Módulo', 'Fecha Carga', 'Año Referencial', 'Mes Referencial', 'Path', 'Tags', 'Descripción'],
        colModel: [
        {name: 'Id', index: 'Id', width: 30, sorttype: "int", editable: false},
        { name: 'FileName', index: 'FileName', width: 150, editable: false, editoptions: { size: "20", maxlength: "30" }},
        { name: 'AlterName', index: 'AlterName', width: 100, editable: true, editoptions: { size: "20", maxlength: "30" }},
        {name: 'Extension', index: 'Extension', width: 30, editable: false, editoptions: { size: "20", maxlength: "30" }},
        { name: 'Company', index: 'Company', width: 80, editable: false, editoptions: { size: "20", maxlength: "30" } },
        { name: 'Container', index: 'Container', width: 80, editable: true, edittype: 'select', editoptions: { value: optionsContainer } },
        {name: 'Application', index: 'Application', width: 70, editable: true, edittype: 'select',editoptions: { value: "1:baaniv;2:dms;3:zeuz" }},
        {name: 'Module', index: 'Module', width: 70, editable: true,editoptions: { size: "20", maxlength: "30" }},
        {name: 'DocumentUploadAt', index: 'DocumentUploadAt', width: 100, editable: false, editoptions: { size: "20", maxlength: "30" }},
        {name: 'ReferencialYear', index: 'ReferencialYear', width: 100, editable: true, editoptions: { size: "20", maxlength: "30" }},
        {name: 'ReferencialMonth', index: 'ReferencialMonth', width: 100, editable: true, editoptions: { size: "20", maxlength: "30" }},
        {name: 'Path', index: 'Path', width: 100, editable: true, hidden: true, editoptions: { size: "20", maxlength: "30" }},
        {name: 'Tags', index: 'Tags', width: 70, editable: true, editoptions: { size: "20", maxlength: "30" }},
        {name: 'Description', index: 'Description', width: 200, sortable: false, editable: true, edittype: "textarea",editoptions: { rows: "2", cols: "10" }}],
        multiselect: true,
        sortname: 'id',
        viewrecords: true,
        onSelectRow: function (id) {
            if (id && id !== lastsel2) {
                jQuery('#rowed5').jqGrid('restoreRow', lastsel2);
                jQuery('#rowed5').jqGrid('editRow', id, true);
                lastsel2 = id;
            }
        },
    });
    var nameFile;
    var data;
    var lstPath;
    var dt = new Date();
    var Path;
    var fechaCarga = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
    $(function () {
        $("#fileInput").on("change", function () {
                lstPath = $("#fileInput").val();
                Path = lstPath.split(", ");
                var archivos = document.getElementById("fileInput").files;
                if (archivos.length>10) {
                    alert("Solo se permite subir 10 archivos");
                } else {
                    for (var i = 0; i < archivos.length; i++) {
                        nameFile = archivos[i].name;
                        var tipoArchivo = fntencontrarTipoArchivo(nameFile);
                        var altername = fntEliminarExtension(nameFile);
                        idTabla++;
                        data = [{
                            Id: idTabla,
                            FileName: nameFile,
                            AlterName: altername,
                            Extension: tipoArchivo,
                            Company: "maresa",
                            Container: $("#cbxContainerUpload").val(),
                            Application: $("#cbxAplicationUpload").val(),
                            Module: $("#txtNombreModulo").val(),
                            DocumentUploadAt: fechaCarga,
                            ReferencialYear: dt.getFullYear(),
                            ReferencialMonth: dt.getMonth() + 1,
                            Path: Path[i],
                            Tags: "",
                            Description: ""
                        }];
                        jQuery("#rowed5").jqGrid('addRowData', data.Id, data);
                    }

                }
                
                fntLimpiarFormularios();
        });
    });
});

function fntencontrarTipoArchivo(cadena) {
    var result;
    for (var i = 0; i < cadena.length; i++) {
        if (cadena[i] == ".") {
            result = cadena.substring(i + 1);
        }
    }
    return result;
};

function fntLimpiarFormularios() {
    $("#cbxContainerUpload").val("Seleccione el Contenedor"),
    $("#cbxAplicationUpload").val("Seleccione aplicación"),
    $("#txtNombreModulo").val("");
}

function fntEliminarExtension(cadena) {
    var result;
    for (var i = 0; i < cadena.length; i++) {
        if (cadena[i] == ".") {
            result = cadena.substring(0, i);
        }
    }
    return result;
};

$("#cbxContainerUpload").change(function () {
    var rowData = $("#rowed5").getDataIDs();
    for (var i = 0; i < rowData.length; i++) {
        $("#rowed5").jqGrid('setRowData', rowData[i], { Container: $("#cbxContainerUpload").val() });
    }
    
});

$("#cbxAplicationUpload").change(function () {
    var rowData = $("#rowed5").getDataIDs();
    for (var i = 0; i < rowData.length; i++) {
        $("#rowed5").jqGrid('setRowData', rowData[i], { Application: $("#cbxAplicationUpload").val() });
    }
});

$("#txtNombreModulo").change(function () {
    var rowData = $("#rowed5").getDataIDs();
    if (rowData!=0) {
        for (var i = 0; i < rowData.length; i++) {
            $("#rowed5").jqGrid('setRowData', rowData[i], { Module: $("#txtNombreModulo").val() });
            $("#rowed5").jqGrid('setRowData', rowData[i], { Id: i + 1 });
        }
    $("#txtNombreModulo").val("");
    }

    
});

$("#DeleteRow").click(function () {
    var row = jQuery("#rowed5").jqGrid('getGridParam', 'selrow');
    var i;
    if (row != null) {
        jQuery("#rowed5").jqGrid('delRowData', row);
        var rowData = $("#rowed5").getDataIDs();
        for (i = 0; i < rowData.length; i++) {
           
            $("#rowed5").jqGrid('setRowData', rowData[i], { Id: i + 1 });
        }
    }
    else {
        alert("Por favor seleccione un archivo para borrar");
    }
    idTabla =i;   
});


$("#DeleteGrid").click(function () {
    $("#rowed5").jqGrid("clearGridData");
    idTabla = 0;
});

function fntGetCbxContainerUpload() {
    var number = 1;
    var containers;
    $("#cbxContainerUpload option").each(function () {
        if ($(this).val() != "") {
            containers += number + ":" + $(this).val() + ";";
            number++;
        }
    });
    optionsContainer = containers.substring(9);
};

function validationExtensionFiles() {
    var validationExtensionFile = $("#rowed5").jqGrid("getCol", "Extension");
    var position;
    for (var i = 0; i < validationExtensionFile.length; i++) {
        if (validationExtensionFile[i] == "pdf") {
            return true;
        } else {
            $("#rowed5").jqGrid("setCell", 1, "Extension", "", {background:"black"});
            return false;
        }
    }
};

function validationAplicationContainerFiles() {
    var validationAplicationFile = $("#rowed5").jqGrid("getCol", "Application");
    var validationContainerFile = $("#rowed5").jqGrid("getCol", "Container");
    var validationModuleFile = $("#rowed5").jqGrid("getCol", "Module");
    for (var i = 0; i < validationContainerFile.length; i++) {
        if (validationContainerFile[i] == "Seleccione el Contenedor" || validationAplicationFile[i] == "Seleccione aplicación" || validationModuleFile =="") {
            return false;
        } else {
            return true;
        }
    }
};


$("#btnUploadFiles").click(function () {
    
    var rowKey = $("#rowed5").getGridParam("selrow");
    if (!rowKey) {
        alert("Debe seleccionar todos los archivos");
    } else {
        if (validationAplicationContainerFiles() != true || validationExtensionFiles() != true) {
            alert("Debe seleccionar un campo válido");
        } else {
            var lstObjetosDatos = jQuery('#rowed5').jqGrid('getGridParam', 'data');
            $.ajax({
                type: "POST",
                url: "/Container/fntUploadFiles",
                data: { lstDocuments: lstObjetosDatos },
                success: function (result) {
                    alert(result);
                    window.location.href = "/Container/Container";
                    $("#MyModalUpload").modal("hide").remove();

                }
            });
            $("#wait").show();
        }
    }

    
    
});

