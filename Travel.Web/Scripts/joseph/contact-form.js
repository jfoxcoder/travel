// add valid test data to contact form
$(function () {

    var index = 0, contact = null, contacts = [
        { Name: "Arnold", Email: "arnie@gmail.com", Phone: "555-1234", Message: "I was born in Europe... and I've traveled all over the world. I can tell you that there is no place, no country, that is more compassionate, more generous, more accepting, and more welcoming than the United States of America." },
        { Name: "Bob", Email: "bob@hotmail.com", Phone: null, Message: "Hi, just want to say great website!" },
        { Name: "Kate", Email: "kate@xtra.co.nz", Phone: "", Message: "Nice website but a bit plain! You should hire someone with graphic design skills!" },
        { Name: "Baldrick", Email: "balders@turnip.net", Phone: "", Message: "I have a cunning plan!" },
        { Name: "Homer Simpson", Email: "doh@springfield.net", Phone: "555-4321", Message: "Needs more donuts!" }
    ];

    $('#add-test-data-btn').on('click', function () {
        
        if (index == contacts.length) index = 0;

        contact = contacts[index];

        for (var prop in contact) {
            if (contact.hasOwnProperty(prop)) {
                $('#' + prop).val(contact[prop]);                
            }
        }

        index++;            
    });

    
});
