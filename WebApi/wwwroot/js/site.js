// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function openTab(evt, tabName, tabId) {
    // Declare all variables
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        if (tabcontent[i].id.split('-')[1][0] == tabId[0])
            tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        if (tablinks[i].id.split('-')[1][0] == tabId[0])
            tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(tabName + '-' + tabId).style.display = "block";
    evt.currentTarget.className += " active";
}

function openTabById(tabName) {
    document.getElementById(tabName).click();
}

function selectImage(evt, id) {
    var thumbnails = document.getElementsByClassName("thumbnail");
    for (i = 0; i < thumbnails.length; i++) {
        thumbnails[i].setAttribute("style", "border-color: #ced4da");
    }
    document.getElementById('img-' + id).setAttribute("style", "border-color: blue");
    document.getElementById('main-picture-id').value = id;
}

$('#edit').click(function () {
    var text = $('.text-info-custom').text();
    var input = $('<input id="attribute" type="text" class="input-name" value="' + text + '" />')
    $('.text-info-custom').text('').append(input);
    input.select();

    input.blur(function () {
        var text = $('#attribute').val();
        $('#label-edit').val(text);
        $('#attribute').parent().text(text);
        $('#attribute').remove();
    });
});

var slideIndex = 1;
showSlides(slideIndex);

// Next/previous controls
function plusSlides(n) {
    showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("dot");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
}