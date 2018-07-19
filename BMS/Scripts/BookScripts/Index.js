
$(document).ready(function () {

    //BookName AutoComplete Data
    $("#BookName").kendoAutoComplete({
        dataTextField: "Text",
        filter: "startswith",
        filtering: function (e) {
            if (!e.filter.value) {
                e.preventDefault();
            }
        },
        dataSource: {
            transport: {
                read: {
                    type: "POST",
                    url: "/Book/GetAllBookName",
                    dataType: "json"
                }
            }
        }
    });

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
        index: 0,
        optionLabel: " "
    });

    //search event
    $("#search").click(function () {
        var arg = {
            "BookName": $("#BookName").val(),
            "BookClass": $("#BookClass").val(),
            "BookKeeper": $("#BookKeeper").val(),
            "BookStatus": $("#BookStatus").val()
        }

        if ($("book_grid").data("kendoGrid") != undefined) {
            $("book_grid").data("kendoGrid").dataSource.read(arg);
            $("book_grid").data("kendoGrid").refresh();
        }
        else {
            var dataSrc = new kendo.data.DataSource({
                batch: true,
                transport: {
                    read: {
                        type: "POST",
                        url: "/Book/SearchBook",
                        dataType: "json",
                        data: arg
                    }
                },
                schema: {
                    model: {
                        fields: {
                            BookId: { type: "string" },
                            BookClass: { type: "string" },
                            BookName: { type: "string" },
                            BookBoughtDate: { type: "string" },
                            BookStatus: { type: "string" },
                            BookKeeper: { type: "string" }
                        }
                    }
                },
                pageSize: 20
            });
        }

        $("#book_grid").kendoGrid({
            dataSource: dataSrc,
            height: 550,
            sortable: true,
            pageable: {
                input: true,
                numeric: false
            },
            columns: [
                { field: "BookId", width: "0%" },
                { field: "BookClass", title: "圖書類別", width: "20%" },
                { field: "BookName", title: "書名", width: "41%" },
                { field: "BookBoughtDate", title: "購書日期", width: "12%" },
                { field: "BookStatus", title: "借閱狀態", width: "15%" },
                { field: "BookKeeper", title: "借閱人", width: "12%" },
                { command: { text: "編輯", click: updateBook }, title: " ", width: "100px" },
                { command: { text: "刪除", click: deleteBook }, title: " ", width: "100px" }
            ]
        });
    });

    //Window delete
    $("#search_delete_window").kendoWindow({
        title: "確定刪除",
        visible: false,
        modal: true,
        width: "400px",
        height: "200px"
    });

    //Window lended
    $("#search_lended_window").kendoWindow({
        title: "無法刪除",
        visible: false,
        modal: true,
        width: "400px",
        height: "200px"
    });
});

//update book button
function updateBook(e) {
    e.preventDefault();
    var bookId = $(e.currentTarget).closest("tr").find("td:eq(0)").text();
    window.location.replace("/Book/UpdateBook/" + bookId);
}

//delete book button
function deleteBook(e) {
    e.preventDefault();
    var tr = $(e.currentTarget).closest("tr");
    var bookId = tr.find("td:eq(0)").text();
    if (tr.find("td:eq(4)").text() === "已借出" || tr.find("td:eq(4)").text() === "已借出(未領)") {
        var windowTemplate = kendo.template($("#search_lended_template").html());
        var windowLended = $("#search_lended_window").data("kendoWindow");
        windowLended.content(windowTemplate);

        windowLended.center().open();

        $("#search_lended_yes").click(function () {
            windowLended.close();
        });
    }
    else {
        var windowTemplate = kendo.template($("#search_delete_template").html());
        var windowDel = $("#search_delete_window").data("kendoWindow");
        windowDel.content(windowTemplate);

        windowDel.center().open();

        $("#search_del_yes").click(function () {
            $.ajax({
                type: "POST",
                url: "/Book/DeleteBook",
                data: "bookId=" + bookId,
                dataType: "json",
                success: function (response) {
                    $(tr).remove();
                }, error: function (error) {
                    alert("系統發生錯誤");
                }
            });

            windowDel.close();
        });

        $("#search_del_no").click(function () {
            windowDel.close();
        });
    }
}