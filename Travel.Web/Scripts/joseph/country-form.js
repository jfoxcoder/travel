$(function () {
    console.log('country-form.js begins');
    var $nameInput = $('#name');

    console.log($nameInput);

    $nameInput.on('input', function () {
        console.log('name input fired');
    });


});