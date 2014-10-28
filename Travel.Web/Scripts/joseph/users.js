$(function () {
    UserMan.init();
});


var UserMan = (function (document) {

    var $adminList = $('#admin-list'),
        $userList = $('#user-list'),
        user = null;
    
    var requestAdminRoleChange = function () {

        var $switch = $(this);
        var promote = $(this).prop('checked');

        var url = promote ? $switch.data('promote-url') : $switch.data('demote-url');

        $.ajax({
            url : url,
            type: 'post'
        }).success(function () {
            // move user to the other role list
            var $userItem = $switch.closest('li').fadeOut(500, function () {
                (promote ? $adminList : $userList).prepend($userItem);
                $userItem.fadeIn();
            });
            
            FlashMessage.happy("User " + (promote ? "promoted" : "demoted") + " successfully");
        }).fail(function (jqXHR, textStatus, errorThrown) {            
            $switch.prop('checked', !promote); // revert switch
            FlashMessage.alert(errorThrown);   // show error
        });
    };


    var removeDeletedUser = function ($userListItem) {        
        return function (jqxhr, textStatus, errorThrown) {
            $userListItem.fadeOut();
        }
    } 

    var requestDeleteUser = function () {
      
        var $deleteBtn = $(this), $userListItem = $deleteBtn.closest('li');

        $.ajax({
            url: $deleteBtn.data('delete-url'),
            type: 'delete'            
        }).done(removeDeletedUser($userListItem))
        .fail(function (jqXHR, textStatus, errorThrown) {            
            FlashMessage.alert(errorThrown);
        });
    };

    var init = function () {
        // setup events
        $('input[type=checkbox]').on('change', requestAdminRoleChange);
        $('.icon-delete').on('click', requestDeleteUser);
    };

    return {
        init : init
    };
})(document);
