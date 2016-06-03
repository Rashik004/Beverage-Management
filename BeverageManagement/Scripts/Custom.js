$(function () {
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
    var $historyPage = $("#past-history-controller");
    if ($historyPage.length > 0) {

        $("#save-btn").on("click", function (e) {
            var $table = $historyPage.find("table");
            var $message = "Messages will be sent to:<br/>";
            var $header = "Beverage Payment Confirmation";
            var $numberOfSelectedEmployee = 0;
            var $tr = $table.find("tr");
            
            var $checkBoxes = $tr.find(".employee-select");
            for (var i = 0; i < $checkBoxes.length; i++) {
                var $checkbox = $($checkBoxes[i]);
                if ($checkbox.is(":checked")) {
                    $message = $message + $checkbox.attr("data-employeeName") + "<br/>";
                    $numberOfSelectedEmployee++;
                }
            }
            if ($numberOfSelectedEmployee == 0) {
                $message = "No employees has been selected";
                $header = "Attention";
            }
            $(".modal-title").html($header);
            $(".modal-body>p").html($message);
            e.preventDefault();
        });
    }
});


