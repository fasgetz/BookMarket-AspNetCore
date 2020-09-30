
function EditComment(IdBook, comment, IdComment) {
    $.ajax({
        type: "GET",
        url: '/Books/EditCommentary',

        data: {
            IdBook: IdBook, comment: comment, IdComment: IdComment
        },

        success: function (data) {
            $(".aboutBookBox__myComment").html(data);


        }
    });

}

