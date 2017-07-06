$(function () {
    // We can attach the `fileselect` event to all file inputs on the page
    $(document).on('change', ':file', function () {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
        ToggleImagePreviewAndSetFileName($(input).data("name"), label);
    });

    // We can watch for our custom `fileselect` event like this
    $(document).ready(function () {
        $(".imgInp").change(function () {
            readURL(this, $(this).attr("data-target"));
        });

        $(".imgInp").each(function () {
            initializeImagePreviews($(this).attr('data-name'));
        });

        $(".krpanoInp").each(function () {
            initializeImagePreviews($(this).attr('data-name'));
        });

        $('.imgClearBtn').click(function () {
            purgeImageUpload($(this).attr('data-name'));
        });
    });
});

function ToggleImagePreviewAndSetFileName(name, label) {
    if ($('#' + name + 'Toggle') && $('#' + name + 'Toggle').hasClass("fa-toggle-off")) {
        $('#' + name + 'Toggle').click();
    }
    $('#' + name + 'FilenameInput').val(label);
};

function initializeImagePreviews(name) {
    setTimeout(function () {
        $('#' + name + 'Image').on('shown.bs.collapse', function () {
            $('#' + name + 'Toggle').removeClass("fa-toggle-off");
            $('#' + name + 'Toggle').addClass("fa-toggle-on");
        });
        $('#' + name + 'Image').on('hidden.bs.collapse', function () {
            $('#' + name + 'Toggle').removeClass("fa-toggle-on");
            $('#' + name + 'Toggle').addClass("fa-toggle-off");
        });
    }, 1000);
};

function readURL(input, target) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + target).attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
};

function purgeImageUpload(name) {
    $('#' + name + 'Input').replaceWith($('#' + name + 'Input').val('').clone(true));
    $('#' + name + 'FilenameInput').val("");
    $('#' + name + 'Image').attr('src', '');
    $('input[data-imageName=' + name + ']').attr('value', '');
    if ($('#' + name + 'Toggle').hasClass("fa-toggle-on")) {
        $('#' + name + 'Toggle').click();
    }
};