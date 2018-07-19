
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
        index: 0
    });
    //BookKeeper DropDownList Data
    $("#BookKeeper").kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: {
            transport: {
                read: {
                    type: "POST",
                    url: "/Book/GetAllBookKeeper",
                    dataType: "json"
                }
            }
        },
        index: 0,
        optionLabel: " "
    });
    //BookStatus DropDownList Data
    $("#BookStatus").kendoDropDownList({
        dataTextField: "Text",
        dataValueField: "Value",
        dataSource: {
            transport: {
                read: {
                    type: "POST",
                    url: "/Book/GetAllBookStatus",
                    dataType: "json"
                }
            }
        },
        change: onChange,
        index: 0
    });

    //DatePicker
    $("#BookBoughtDate").kendoDatePicker({
        format: "yyyy/MM/dd",
        max: new Date()
    });

    //Validator
    $("#Form_Update").kendoValidator({
        messages: {
            required: "此欄位必填"
        }
    });

    //Window Save
    $("#update_save_window").kendoWindow({
        title: "完成",
        visible: false,
        modal: true,
        width: "400px",
        height: "200px"
    });

    //Window Delete
    $("#update_delete_window").kendoWindow({
        title: "確定刪除",
        visible: false,
        modal: true,
        width: "400px",
        height: "200px"
    });

    //Window lended
    $("#update_lended_window").kendoWindow({
        title: "無法刪除",
        visible: false,
        modal: true,
        width: "400px",
        height: "200px"
    });

    //Get update book data
    $.ajax({
        type: "POST",
        url: "/Book/GetUpdateBook",
        dataType: "json",
        data: "bookId=" + window.location.pathname.split('/').pop(),
        success: function (book) {
            $("#BookId").val(book.BookId);
            $("#BookName").val(book.BookName);
            $("#BookAuthor").val(book.BookAuthor);
            $("#BookPublisher").val(book.BookPublisher);
            $("#BookNote").text(book.BookNote);
            $("#BookBoughtDate").val(book.BookBoughtDate);
            $("#BookClass").data("kendoDropDownList").value(book.BookClass);
            $("#BookStatus").data("kendoDropDownList").value(book.BookStatus);
            $("#BookKeeper").data("kendoDropDownList").value(book.BookKeeper);
            if ($("#BookStatus").data("kendoDropDownList").value() === 'A' ||
                $("#BookStatus").data("kendoDropDownList").value() === 'U') {
                $("#BookKeeper").data("kendoDropDownList").readonly();
            }
        }
    });

    //Save event
    $("#submit").click(function () {

        if ($("#BookKeeper").data("kendoDropDownList").value() === "" &&
           ($("#BookStatus").data("kendoDropDownList").value() === 'C' ||
            $("#BookStatus").data("kendoDropDownList").value() === 'B')) {
            alert("借閱人不可為空");
            return false;
        }

        var arg = {
            "BookId": $("#BookId").val(),
            "BookName": $("#BookName").val(),
            "BookAuthor": $("#BookAuthor").val(),
            "BookPublisher": $("#BookPublisher").val(),
            "BookNote": $("#BookNote").val(),
            "BookBoughtDate": $("#BookBoughtDate").val(),
            "BookClass": $("#BookClass").data("kendoDropDownList").value(),
            "BookStatus": $("#BookStatus").data("kendoDropDownList").value(),
            "BookKeeper": $("#BookKeeper").data("kendoDropDownList").value()
        }
        if ($("#Form_Update").data("kendoValidator").validate()) {
            $.ajax({
                type: "POST",
                url: "/Book/Update",
                dataType: "json",
                data: arg,
                success: function () {
                    var windowTemplate = kendo.template($("#update_save_template").html());
                    var windowSave = $("#update_save_window").data("kendoWindow");
                    windowSave.content(windowTemplate);

                    windowSave.center().open();

                    $("#update_save_yes").click(function () {
                        windowSave.close();
                    });
                }
            });
        }
        return true;
    });

    //delete event
    $("#btnDelete").click(function (e) {
        e.preventDefault();
        var bookId = $(e.currentTarget).next().val();
        if ($("#BookStatus").data("kendoDropDownList").value() === "C" || $("#BookStatus").data("kendoDropDownList").value() === "B") {
            var windowTemplate = kendo.template($("#update_lended_template").html());
            var windowLended = $("#update_lended_window").data("kendoWindow");
            windowLended.content(windowTemplate);

            windowLended.center().open();

            $("#update_lended_yes").click(function () {
                windowLended.close();
            });
        }
        else {
            var windowTemplate = kendo.template($("#update_delete_template").html());
            var windowDel = $("#update_delete_window").data("kendoWindow");
            windowDel.content(windowTemplate);

            windowDel.center().open();

            $("#update_del_yes").click(function () {
                $.ajax({
                    type: "POST",
                    url: "/Book/DeleteBook",
                    data: "bookId=" + bookId,
                    dataType: "json",
                    success: function (response) { },
                    error: function (error) {
                        alert("系統發生錯誤");
                    }
                });
                windowDel.close();
                location.replace("/Book");
            });

            $("#update_del_no").click(function () {
                windowDel.close();
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

//Detect BookStatus DropDownList
function onChange() {
    if ($("#BookStatus").data("kendoDropDownList").value() === 'A' || $("#BookStatus").data("kendoDropDownList").value() === 'U') {
        $("#BookKeeper").data("kendoDropDownList").readonly();
        $("#BookKeeper").data("kendoDropDownList").value("");
    }
    if ($("#BookStatus").data("kendoDropDownList").value() === 'B' || $("#BookStatus").data("kendoDropDownList").value() === 'C') {
        $("#BookKeeper").data("kendoDropDownList").readonly(false);
    }
}