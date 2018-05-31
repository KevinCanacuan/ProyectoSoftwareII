var $dialog;
$(document).ready(function () {

    //cargar();
    $('body').on("click", "a.popup", function (e) {
        e.preventDefault;
        var page = $(this).attr('href');
        OpenPopup(page);
    })
});



function OpenPopup(Page) {
    var $pageContent = $('<div/>');
    $pageContent.load(Page);
    $dialog = $('<div class="popupWindow" style="overflow:hidden"></div>')
        .html($pageContent)
        .dialog({
            draggable: false,
            autoOpen: false,
            resizable: false,
            model: true,
            height: 600,
            width: 600,
            close: function () {
                $dialog.dialog('destroy').remove();
            }
        })
    $dialog.dialog('open');
}