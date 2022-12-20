
let tableName = "";
let initializedDataTable;
let initializedDataTableSin;

function initDataTable(tblName = "") {
    if (tblName != "") tableName = tblName;
    const datatablesSimple = document.getElementById(tableName);
    if (datatablesSimple) {
        initializedDataTable = new simpleDatatables.DataTable(
            datatablesSimple,
            {
                labels: {
                    placeholder: "Buscar...",
                    perPage: "{select} Filtrar número de entradas",
                    noRows: "Sin Registros para mostrar",
                    info: "Mostrando {start} a {end} de {rows} entradas",
                },
                perPageSelect: false,
                perPage: 10,
            }
        );
    }
    var tooltipTriggerList = [].slice.call(
        document.querySelectorAll('[data-bs-toggle="tooltip"]')
    );
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

function initD(tabla = "") {
    initDataTable(tabla);
    return initializedDataTable;
}


function openModal(url, title, id, ...param) {
    $.ajax({
        url: url,
        type: 'post', //Siempre sera post, porque solo enviara datos..
        dataType: 'html',
        data: {
            title: title,            
            id: id,
            param: param,
            //"_token": "{{ csrf_token()}}"            
            //"_token":$("meta[name='csrf-token']").attr("content")
        },
        beforeSend: function () {
            showBlockUI();
        },
        success: function (response) {
            //$.unblockUI();
            if (response.includes("div")) {
                $("#exampleModal .modal-body").html(response);
                $('#title').html(title);
                //$("#exampleModal").modal({ backdrop: 'static', keyboard: false });
                $("#exampleModal").modal("show");
            }
        },
        error: function (error) {
            //$.unblockUI();
            Swal.fire(
                "Error!",
                `Error al abrir el modal: ${error}!`,
                "error"
            );
        }
    });
}   

