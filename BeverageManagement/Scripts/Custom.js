﻿$(function () {
    //document ready
    var $bootstrapTable = $(".bootstrap-table-convert");
    $bootstrapTable.bootstrapTable();

    tinymce.init({
        selector: '.tinymce',
        height: 500,
        menubar: false,

        toolbar: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
        content_css: [
          '//fast.fonts.net/cssapi/e6dc9b99-64fe-4292-ad98-6974f93cd2a2.css',
          '//www.tinymce.com/css/codepen.min.css'
        ]
    });


    $('.datetimepicker').datetimepicker();
    //var element = document.getElementById("datetimepicker1");


    var $historyPage = $("#past-history-controller");
   
    //for (var i = 1; i < 6; i++) {
    //    if (document.getElementById(i).checked)
    //        console.log(i + "is checked");
    //}
    if ($historyPage.length > 0) {

        $("#save-btn").on("click", function (e) {
            var $table = $historyPage.find("table");

            var $tr = $table.find("tr");
            var $message = "Messages will be sent to:<br/>";
            //console.log(message);
            var $checkBoxes = $tr.find(".employee-select");
            for (var i = 0; i < $checkBoxes.length; i++) {
                var $checkbox = $($checkBoxes[i]);
               // console.log($checkbox);
                if ($checkbox.is(":checked")) {
                    $message = $message + $checkbox.attr("data-employeeName") +"<br/>";
                    
                }
            }
            console.log($tr.length - 1);
            console.log("Before MOdal");
            $(".modal-title").html("Beverage Payment Confirmation");
            $(".modal-body>p").html($message);
            console.log("After MOdal");
            e.preventDefault();
        });
    }
});


