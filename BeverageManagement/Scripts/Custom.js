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
    //var element = document.getElementById("datetimepicker1");


    var $historyPage = $("#past-history-controller");
    var $check = "hello";
    console.log($historyPage.length);
    //$historyPage.length is returning zero 

    if ($historyPage.length > 0) {
        // we are in history page
        $("#save-btn").on("click", function (e) {
            
            alert('Hello');
            e.preventDefault();
        });
    }
});


