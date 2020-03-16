$(document).on("keyup", '.RowSubTotal', function (e) {

    var Currentrow = $(this).closest("tr");
    var QTY = $(this).val();


    var vat = Currentrow.find('.vat').val();

    var Totals = Currentrow.find('.RowSubTotal').val();

    if (Totals != '') {
        RowSubTalSubtotal(vat, Currentrow);
        CountTotalVat();
    }

});


$(document).on("change", '.vat', function () {
    var Currentrow = $(this).closest("tr");
    var vat = Currentrow.find('.vat').val();
    RowSubTalSubtotal(vat, Currentrow);
    CountTotalVat();
});


function RowSubTalSubtotal(vat, CurrentRow) {

    Total = 0;
    Total = CurrentRow.find('.RowSubTotal').val();
    if (parseInt(vat) == 0 && typeof (vat) != "undefined" && vat != "") {
        if (!isNaN(Total) && typeof (Total) != "undefined") {
            CurrentRow.find('.rownettotal  ').val(Total);
            CurrentRow.find('.rownettotal  ').val(parseFloat(Total).toFixed(2));
            return;
        }
    }

    if (!isNaN(Total) && Total != "" && typeof (vat) != "undefined") {
        var InputVatValue = parseFloat((Total / 100) * vat);
        var ValueWTV = parseFloat(InputVatValue) + parseFloat(Total);
        CurrentRow.find('.rownettotal').val(parseFloat(ValueWTV).toFixed(2));
    }
}


function IsOneDecimalPoint(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}


function CountTotalVat() {

    var TotalVAT = 0.00;
    var GTotal = 0.00;
    var ToatWTVAT = 0.00;

    $('#orderdetailsitems .tbodyGood tr').each(function () {

        if ($(this).find(".RowSubTotal").val() != '') {
            GTotal = parseFloat(GTotal) + parseFloat($(this).find(".rownettotal").val());
            ToatWTVAT = parseFloat(ToatWTVAT) + parseFloat($(this).find(".RowSubTotal").val());
            TotalVAT = parseFloat(GTotal) - parseFloat(ToatWTVAT);
        }

    });

    $('#TotalVAT').text(parseFloat(TotalVAT).toFixed(2));
    $('#SubTotal').text(parseFloat(ToatWTVAT).toFixed(2));
    $('#gtotal').text(parseFloat(GTotal).toFixed(2));
}


$('#mainrowgood #ExpenseType').change(function () {

    var Currentrow = $(this).closest("tr");

    if ($(this).val().trim() == "Vehicle") {
        Currentrow.find(".ChoiceBox").empty();

        ajaxRequest("POST", "/LoadCustomerVehicle", "", "json").then(function (result) {

            if (result != "Failed") {
                $('select').chosen();
                $.each(result, function (key, Item) {
                    Currentrow.find(".ChoiceBox").append('<option value=' + Item.Id + '>' + Item.TraficPlateNumber + '</option>');
                });
                $('select').trigger("chosen:updated");

            }

        });
    }
    else if ($(this).val().trim() == "Employee") {
        Currentrow.find(".ChoiceBox").empty();
        ajaxRequest("POST", "/LoadEmployeeAll", "", "json").then(function (result) {

            if (result != "Failed") {
                $('select').chosen();
                $.each(result, function (key, Item) {

                    Currentrow.find(".ChoiceBox").append('<option value=' + Item.Id + '>' + Item.Name + '</option>');
                });
                $('select').trigger("chosen:updated");
            }

        });
    }
    else if ($(this).val().trim() == "General") {

        Currentrow.find(".ChoiceBox").empty();

        ajaxRequest("POST", "/LoadGeneralExpenseCustomer", "", "json").then(function (result) {

            if (result != "Failed") {
                $('select').chosen();
                $.each(result, function (key, Item) {
                    Currentrow.find(".ChoiceBox").append('<option value=' + Item.Id + '>' + Item.ExpenseName + '</option>');
                });
                $('select').trigger("chosen:updated");
            }

        });

    }
});


$('#btnaddRow').click(function () {
    var currentrow = $(this).closest("tr");
    var vat = currentrow.find('.vat').val();

    var isAllValid = true;
    if ($("#mainrowgood #drpgoods").val() == "0") {
        isAllValid = false;
        //swal("Let op!", "Selecteer product", "warning");
        alert('select Expense Type');
        return;
    }


    if ($("#mainrowgood .RowSubTotal ").val() == 0) {
        isAllValid = false;
        // swal("Let op!", "Voer inkoopprijs in", "warning");
        alert('Add Amount')
        return;
    }

    if (isAllValid) {
        var $newRow = $("#mainrowgood").clone().removeAttr('id');
        $('.ExpenseType', $newRow).val($("#mainrowgood .ExpenseType").val());
        $('.vat', $newRow).val($("#mainrowgood .vat").val());
        $('.ChoiceBox', $newRow).val($("#mainrowgood .ChoiceBox").val());
        // $('#ExpenseType', $newRow).val($("#ExpenseType .ExpenseType ").val());
        $("#btnaddRow", $newRow).addClass('remove').val('x').removeClass('btn-success').addClass('btn-height-Remove');
        $("#ExpenseType, #Description, #rowsubtotal,#vat,#rownettotal", $newRow).removeAttr('id');

        $('.tbodyGood tr:last').before($newRow);
        $(".rowsubtotal").prop('disabled', true);
        $(".rownettotal").prop('disabled', true);
        //$('#mainrowgood #drpgoods').select2().select2('val', '0');
        $('select').chosen();
        $('#mainrowgood .ExpenseType').val(0);
        $('#mainrowgood .ChoiceBox').val(0);
        $('#mainrowgood #VAT').val(0);
        $('select').trigger("chosen:updated");
        $('#mainrowgood #rownettotal').val(0.00);
        $('#mainrowgood #RowSubTotal').val(0.00);
        $('#mainrowgood #Description').val('');


        // clearfield();
    }
});


$(document).on('click', '.remove', function () {
    var Current = $(this).closest('tr');
    Current.remove();
    CountTotalVat();
});

function validateRow(currentRow) {

    debugger;
    var isvalid = true;
    var Expense = "", RowSubTotal = 0, ChoiceBox = 0;
    Expense = currentRow.find('.ExpenseType').val();
    RowSubTotal = currentRow.find('.RowSubTotal').val();
    ChoiceBox = currentRow.find('.ChoiceBox ').val();
    if (Expense == "") {
        isvalid = false;
    }
    if (parseInt(RowSubTotal) == 0 || RowSubTotal == "") {
        isvalid = false;
    }

    if (Expense != "" && RowSubTotal > 0) {
        if (ChoiceBox == 0) {
            isvalid = false;
            alert('please select choice');
        }
    }

    return isvalid;

}



function IsDateSelected(currentRow) {
    var IsSelected = true;

    if (currentRow.find('.OnDates').val().trim() == '06/30/2019') {
        IsSelected = false;
    }
    return IsSelected;
}


var IsValidationPass = true;

function CreateExpense(Url) {
    if (1 == 1) {

        var list = [], orderItem, CurrentRow;
        var formData = new FormData();

        $('#orderdetailsitems .tbodyGood tr').each(function () {

            CurrentRow = $(this).closest("tr");

            if (validateRow(CurrentRow)) {

                if (IsDateSelected(CurrentRow)) {
                    if (1 == 1) {
                        orderItem = {

                            Id: $(this).find('.DetId').val(),
                            ExpDates: $(this).find('.OnDates').val(),
                            Category: $(this).find('.ExpenseType').val(),
                            ExpenseRefrenceId: $(this).find('.ChoiceBox').val(),
                            ExpenseType: parseInt($(this).find('.ExpenseType').val()),
                            Description: $(this).find('.Description').val(),
                            SubTotal: parseFloat($(this).find('.RowSubTotal').val()),
                            VAT: parseInt($(this).find('.vat').val()),
                            NetTotal: parseInt($(this).find('.rownettotal').val()),

                        }
                        list.push(orderItem);

                        IsValidationPass = true;
                    }
                }
                else {
                    IsValidationPass = false;
                }
            }
        });

        if (IsValidationPass == true) {

            if (list.length == 0) {
                $('#SubTotal').text('');
                $('#TotalVAT').text('');
                $('#gtotal').text('');
            }
            var empObj = {
                Id: $('#ExpId').val(),
                ExpenseNumber: $('#RefrenceNumber').val(),
                Total: $('#SubTotal').text(),
                VAT: $('#TotalVAT').text(),
                ExpensePadNumber: $('#ExpensePadNumber').val(),
                GrandTotal: $('#gtotal').text(),
                TermCondition: $('#TermCondition').val(),
                CustomerNote: $('#CustomerNote').val(),
                EmployeeId: $('#EmployeeId').val(),
                Category: $('#ExpenseFor').val(),
                ItemRefrenceId: $('#ChoiceBox').val(),
            };

            for (var key in empObj) {
                formData.append(key, empObj[key]);
            }
            for (var i = 0; i < list.length; i++) {
                formData.append('expenseDetailsList[' + i + '][Id]', list[i].Id)
                formData.append('expenseDetailsList[' + i + '][ExpDates]', list[i].ExpDates)
                formData.append('expenseDetailsList[' + i + '][ExpenseRefrenceId]', list[i].ExpenseRefrenceId)
                formData.append('expenseDetailsList[' + i + '][Category]', list[i].Category)
                formData.append('expenseDetailsList[' + i + '][ExpenseType]', list[i].ExpenseType)
                formData.append('expenseDetailsList[' + i + '][SubTotal]', list[i].SubTotal)
                formData.append('expenseDetailsList[' + i + '][VAT]', list[i].VAT)
                formData.append('expenseDetailsList[' + i + '][NetTotal]', list[i].NetTotal)
                formData.append('expenseDetailsList[' + i + '][Description]', list[i].Description)
            }
            if (list.length > 0) {

                $.ajax({
                    url: Url,
                    type: "POST",
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (result) {
                        if (result != "Failed") {
                            list = [];
                            sucessAdd();
                            //response(result, btnName, Message, returnulr, F)

                            var SendEmail = localStorage.getItem("SendByEmail");

                            if (SendEmail != null) {

                                localStorage.setItem("Id", result);
                                window.location.href = "/Email";
                            }
                            else {
                                window.location.href = "/ExpenseCustomer-Details/" + result;
                            }
                        }
                        else {
                            alert(result);
                        }
                    },
                    error: function (errormessage) {
                        alert(errormessage);
                    }
                });
            }
            else {

                alert('Please Add item to list');
            }
        }
        else {
            alert('please select valid date')
        }
    }
    else {

        swal({
            title: 'Graag uw invoer controleren!',
            text: 'Graag contact slecteren',
            type: 'warning'
        });
    }
}