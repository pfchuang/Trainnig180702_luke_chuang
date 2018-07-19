
$(document).ready(function () {

    //BookClass DropDownList Data
    $("#BookClass").kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: {
            transport: {
                read: {
                    type: "POST",
                    url: "/Book/GetAllBookClass",
                    dataType: "json"
                }
            }
        },
        index: 0,
        optionLabel: " "
    });

    //DatePicker
    $("#BookBoughtDate").kendoDatePicker({
        format: "yyyy/MM/dd",
        max: new Date()
    });

    //Validator
    $("#Form_Add").kendoValidator({
        messages: {
            required: "此欄位必填"
        }
    });

    //Window save
    $("#add_save_window").kendoWindow({
        title: "完成",
        visible: false,
        modal: true,
        width: "400px",
        height: "200px"
    });

    //Add event
    $("#submit").click(function () {
        var arg = {
            "BookName": $("#BookName").val(),
            "BookAuthor": $("#BookAuthor").val(),
            "BookPublisher": $("#BookPublisher").val(),
            "BookNote": $("#BookNote").val(),
            "BookBoughtDate": $("#BookBoughtDate").val(),
            "BookClass": $("#BookClass").data("kendoDropDownList").value()
        }

        if ($("#Form_Add").data("kendoValidator").validate()) {
            $.ajax({
                type: "POST",
                url: "/Book/Add",
                dataType: "json",
                data: arg,
                success: function () {
                    var windowTemplate = kendo.template($("#add_save_template").html());
                    var windowSave = $("#add_save_window").data("kendoWindow");
                    windowSave.content(windowTemplate);

                    windowSave.center().open();

                    $("#add_save_yes").click(function () {
                        windowSave.close();
                        location.replace("/Book");
                    });
                }
            });
        }
    });

    //Word Restriction
    $("#BookNote").on("input", function () {
        if ($(this).val().length > 1000) {
            alert("超過字數限制");
            $(this).val($(this).val().substring(0, 1000));
        }
    });
});